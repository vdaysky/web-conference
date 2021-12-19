using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConf.network.packet
{
    class PacketManager
    {
        private static Dictionary<Int64, TaskCompletionSource<Packet>> packets = new Dictionary<Int64, TaskCompletionSource<Packet>>();

        public static Task<Packet> send(Packet packet) {
            TaskCompletionSource<Packet> tcs = new TaskCompletionSource<Packet>();
            packets.Add(packet.getPacketID(), tcs);
            return tcs.Task;
        }

        public static void onReceive(byte[] bytes) {
            Packet packet = null;
            byte packetTypeID = bytes[0];
            switch (packetTypeID) {
                case 1:
                    packet = Packet.deserialize(new ArraySegment<byte>(bytes, 1, bytes.Length).ToArray());
                    break;
                default:
                    break;
            }

            Int64 ID = packet.getPacketID();
            // todo packets that are not responses
            var tsc = packets[ID];
            tsc.SetResult(packet);
        }
    }
}
