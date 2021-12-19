using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = new Ser1(1, 2).serialize();
            var s = Ser1.deserialize(b);
        }
    }
    [Serializable]
    abstract class Ser {
        private int c;

        public Ser() {
            this.c = abs();
        }

        public byte[] serialize()
        {

            BinaryFormatter b = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                b.Serialize(ms, this);
                ms.Position = 0;
                byte[] buff = new byte[ms.Length];
                ms.Read(buff, 0, (int)ms.Length);
                return buff;
            }
        }

        public static Ser deserialize(byte[] bytes)
        {
            BinaryFormatter b = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return (Ser)b.Deserialize(ms);
            }
        }

        public abstract int abs();
    }

    [Serializable]
    class Ser1 : Ser {
        private int a = 2;
        private int b = 3;

        public Ser1(int a, int b) {
            this.a = a;
            this.b = b;
        }

        public override int abs() {
            return 42;
        }
    }
}
