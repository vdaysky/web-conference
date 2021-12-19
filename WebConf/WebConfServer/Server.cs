using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using common;
using NetworkCommon.packet;
using WebConfServer.database;

// State object for reading client data asynchronously  
public class StateObject
{
    // Size of receive buffer.  
    public const int BufferSize = 1024;

    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];

    public List<byte> message = new List<byte>();

    // Client socket.
    public Socket workSocket = null;

    public void reset() {
        message.Clear();
    }
}

public class AsynchronousSocketListener
{
    // Thread signal.  
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    static DataStorage database = new JSONDataStorage();
    static Dictionary<long, Socket> Connections = new Dictionary<long, Socket>();

    public AsynchronousSocketListener()
    {
    }

    public static void StartListening()
    {
        IPEndPoint localEndPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1}), 8000);

        // Create a TCP/IP socket.  
        Socket listener = new Socket(SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and listen for incoming connections.  
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true)
            {
                // Set the event to nonsignaled state.  
                allDone.Reset();

                // Start an asynchronous socket to listen for connections.  
                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);

                // Wait until a connection is made before continuing.  
                allDone.WaitOne();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }

    public static void AcceptCallback(IAsyncResult ar)
    {
        // Signal the main thread to continue.  
        allDone.Set();

        // Get the socket that handles the client request.  
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);
        Console.WriteLine("Conected: {0}", handler.RemoteEndPoint.ToString());
        // Create the state object.  
        StateObject state = new StateObject();
        state.workSocket = handler;

        Console.WriteLine("Begin Receive");
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        
    }

    static void SaveConnection(User user, Socket sock) {
        Connections.Add((long)user.id, sock);
    }

    static Packet handlePacket(Packet packet, Socket sock) {
        Console.WriteLine("Received packet: {0}", packet);

        LoadedUser user = database.getUser(packet.auth_token);
        
        if (packet is AuthPacket) {
            AuthPacket _packet = (AuthPacket)packet;
            
            if (user == null) {
                user = database.getUserByLogin(_packet.username);
                if (user?.password != _packet.password) {
                    user = null;
                }
            }

            if (user == null) {
                return new AuthResponsePacket(0, null);
            }

            SaveConnection(user, sock);
            return new AuthResponsePacket(1, user).withToken(user.cookie);
        }

        if (packet is RegisterPacket) {
            RegisterPacket _packet = (RegisterPacket)packet;

            if (database.getUserByLogin(((RegisterPacket)packet).username) != null) {
                return new AuthResponsePacket(0, null);
            }

            LoadedUser createdUser = new LoadedUser(null, _packet.username, _packet.password, null);
            database.addOrEditUser(createdUser);

            SaveConnection(createdUser, sock);
            return new AuthResponsePacket(1, createdUser).withToken(createdUser.cookie);
        }

        if (packet is GetMessagesPacket) {
            GetMessagesPacket request = (GetMessagesPacket) packet;
            if (request.RoomID == null) {
                return null;
            }
            return new MessageListPacket(database.GetMessages((long)request.RoomID).Cast<Message>());
        }

        if (packet is GetRoomsPacket) {
            return new RoomsListPacket(
              database.GetRooms((int)user.id).Cast<Room>()
           );
        }
        if (packet is GetRoomPacket) {
            GetRoomPacket request = (GetRoomPacket)packet;


            if (request.room_id == null) {
                return null;
            }

            return new RoomPacket(
                database.getRoomById((long) request.room_id)
            );
        }
        if (packet is SendMessagePacket) {
            if (user == null) 
                return null;

            // add to database
            Message received = ((SendMessagePacket)packet).message;
            received.author = user;
            LoadedMesage message = new LoadedMesage(received);
            database.addMessage(message);
            Broadcast(new MessageReceivedPacket(message));

            return new MessageReceivedPacket(message);
        }

        if (packet is CreateRoomPacket) {

            CreateRoomPacket _packet = (CreateRoomPacket)packet;
            LoadedRoom createdRoom = database.addRoom(new LoadedRoom(null, _packet.RoomName, null, user));

            Broadcast(new RoomCreatedPacket(createdRoom));

            return new RoomCreatedPacket(createdRoom);
        }

        return new AckResponsePacket();
    }

    static void SendPacket(Socket socket, Packet packet) {
        Send(socket, packet.serialize());
    }

    static void Broadcast(Packet packet) {
        foreach (var sock in Connections.Values) {
            SendPacket(sock, packet);
        }
    }

    public static void ReadCallback(IAsyncResult ar)
    {
        List<byte> content = new List<byte>();

        // Retrieve the state object and the handler socket  
        // from the asynchronous state object.  
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        // Read data from the client socket.
        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0)
        {
            state.message.AddRange(new ArraySegment<byte>(state.buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read
            // more data.  
            content = state.message;
            
            if (Encoding.ASCII.GetString(content.GetRange(content.Count - 5, 5).ToArray(), 0, 5) == "<EOF>")
            {
                // All the data has been read from the
                // client. Display it on the console.  
                Console.WriteLine("Read {0} bytes from socket.", content.Count);

                // exclude eof marker
                byte[] bytes = new byte[content.Count - 5];
                for (int i = 0; i < content.Count - 5; i++) {
                    bytes[i] = content[i];
                }

                Console.WriteLine(Encoding.ASCII.GetString(bytes));

                Packet request = Packet.deserialize(bytes);
                Packet response = handlePacket(request, handler);

                if (response == null) {
                    response = new AckResponsePacket();
                }

                // if handler did not set token, keep same one
                if (response.auth_token == "none") {
                    response.auth_token = request.auth_token;
                }
                
                response.packetID = request.packetID;

                state.reset();
                database.save();

                Console.WriteLine("Send packet: {0}", response);
                Console.WriteLine(Encoding.ASCII.GetString(response.serialize()));
                SendPacket(handler, response);
            }
            //else
            //{
                // Not all data received. Get more.  
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            //}
        }
    }

    private static void Send(Socket handler, byte[] data)
    {
        // Begin sending the data to the remote device.
        string end = "<EOF>";
        byte[] terminated = new byte[data.Length + end.Length];
        for (int i = 0; i < data.Length; i++) {
            terminated[i] = data[i];
        }
        for (int i = 0; i < end.Length; i++) {
            terminated[data.Length + i] = Encoding.ASCII.GetBytes(end.ToCharArray(), 0, end.Length)[i];
        }
        handler.BeginSend(terminated, 0, terminated.Length, 0, new AsyncCallback(SendCallback), handler);
    }

    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket handler = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = handler.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);

            //handler.Shutdown(SocketShutdown.Both);
            //handler.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}