using common;
using NetworkCommon.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WebConf.network.packet;
using WebConf.screens;

namespace WebConf
{
    public class WebConferenceModel
    {
        private App app;
        private User _me;
        public Room ActiveRoom = null;

        public PacketManager pManager = new PacketManager();

        public delegate void handleReceivedMessage(Message message);
        public event handleReceivedMessage OnReceivedMessage;

        public delegate void handleRoomCreated(Room room);
        public event handleRoomCreated OnRoomCreated;

        void onMessage(Packet packet) {
            if (packet is MessageReceivedPacket) {
                MessageReceivedPacket _packet = (MessageReceivedPacket)packet;

                if (_packet.Message.room_id != App.Model.ActiveRoom.id)
                    return;
                
                OnReceivedMessage?.Invoke(_packet.Message);
            }

            if (packet is RoomCreatedPacket) {
                RoomCreatedPacket _packet = (RoomCreatedPacket)packet;
                OnRoomCreated?.Invoke(_packet.Room);
            }

        }

        public string Token = "none";

        public User TheUser {
            get { return _me; }
        }

        public WebConferenceModel() {
            app = (App)Application.Current;
            pManager.onMessage += onMessage;
        }

        public async Task<Room> JoinRoom(long room_id) {
            GetRoomPacket packet = new GetRoomPacket(room_id);
            RoomPacket response = (RoomPacket)await pManager.send(packet);
            ActiveRoom = response.room;
            return response.room;
        }
    
        public async Task<Message> SendMessage(string text, long? parent_id=null)
        {
            MessageReceivedPacket response = (MessageReceivedPacket)await pManager.send(
                new SendMessagePacket(new Message(null, text, null, DateTime.Now, parent_id, ActiveRoom.id))
            );
            return response.Message;
        }

        public async Task<List<Message>> GetMessages(long room_id, long? parent_id=null)
        {
            GetMessagesPacket getMessagesPacket = new GetMessagesPacket(room_id, parent_id);
            MessageListPacket mesagesPacket = (MessageListPacket) await pManager.send(getMessagesPacket);
            return mesagesPacket.messages;
        }

        public void onAppLoad() {

        }

        public async Task<bool> LogIn(string username, string password) {
            Packet auth = new AuthPacket(username, password);
            AuthResponsePacket response = (AuthResponsePacket) await pManager.send(auth);

            if (response.isSuccess()) {
                _me = response.user;
            }

            return response.isSuccess();
        }

        public async Task<bool> Register(string username, string password) {
            Packet auth = new RegisterPacket(username, password);
            AuthResponsePacket response = (AuthResponsePacket) await pManager.send(auth);

            if (response.isSuccess())
            {
                _me = response.user;
            }

            return response.isSuccess();
        }

        public async Task<Room> CreateRoom(string name) {
            Packet packet = new CreateRoomPacket(name);
            RoomCreatedPacket response = (RoomCreatedPacket) await pManager.send(packet);
            return response.Room;
        }

        public async Task<List<Room>> GetRooms() {
            return ((RoomsListPacket)await pManager.send(new GetRoomsPacket())).rooms;
        }

        public void LogOut() {
            Token = "none";
        }
    }
}
