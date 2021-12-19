using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebConf.network.packet
{
    abstract class Packet
    {
        public Int64 packetID = 0;

        public Packet() {
        }

        public byte[] serialize() {

            BinaryFormatter b = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                b.Serialize(ms, this);
                ms.Position = 0;
                byte[] buff = new byte[ms.Length];
                ms.Read(buff, 0, (int) ms.Length);
                return buff;
            }
        }

        public static Packet deserialize(byte[] bytes) {
            BinaryFormatter b = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return (Packet) b.Deserialize(ms);
            }
        }

        public Int64 getPacketID() {
            if (packetID == 0) {
                Random r = new Random();
                packetID = r.Next(1, 2147483647);
            }
            return packetID;
        }

        public abstract byte getPacketType();
    }

}
