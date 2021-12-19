using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using common;
using NetworkCommon.packet;

namespace WebConf.network.packet
{
    public class PacketManager
    {
        public PacketManager() {
            // subscribe to event
            ((App) App.Current).byteReadHandler += onReceive;
        }

        // Event for server message received without a request
        public event onMessageReceive onMessage;

        public delegate void onMessageReceive(Packet x);


        private Dictionary<Int64, TaskCompletionSource<Packet>> packets = new Dictionary<Int64, TaskCompletionSource<Packet>>();

        public Task<Packet> send(Packet packet) {
         
            packet.auth_token = App.Model.Token;

            Trace.WriteLine(String.Format("Send Packet: {0}", packet));
            TaskCompletionSource<Packet> tcs = new TaskCompletionSource<Packet>();
            packets.Add( (long) packet.getPacketID(), tcs);
            
            byte[] ser = packet.serialize(); 
            byte[] p = new byte[ser.Length + 5];
            int i = 0;
            foreach (var b in ser) {
                p[i++] = b;
            }
            string end = "<EOF>";
            for (var j = 0; j < end.Length; j++) {
                p[i + j] = (byte) end[j];
            }   
            ((App)App.Current).send(p);
            return tcs.Task;
        }

        public void onReceive(byte[] bytes, int size) {
            Packet packet = Packet.deserialize(new ArraySegment<byte>(bytes, 0, size).ToArray());
            
            Int64 ID = (long) packet.getPacketID();
            // todo packets that are not responses
            try
            {
                TaskCompletionSource<Packet> tsc = packets[ID];
                Trace.WriteLine(String.Format("Receive Response Packet: {0}", packet));

                packets.Remove(ID);
                // Only response packets can update auth token
                App.Model.Token = packet.auth_token;
                tsc.SetResult(packet);
            }
            catch (KeyNotFoundException e) {
                Trace.WriteLine(String.Format("Receive Anonymous Packet: {0}", packet));
                // call handlers from main thread
                App.Current.Dispatcher.Invoke(new Action(() =>onMessage?.Invoke(packet)));
            }
        }
    }
}
