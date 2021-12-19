using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConf.network.packet
{
    class AuthResponsePacket : Packet
    {
        public int result;

        public AuthResponsePacket(int result) {
            this.result = result;
        }

        public bool isSuccess() {
            return result == 1;
        }

        public override byte getPacketType() {
            return 2;
        }


    }
}
