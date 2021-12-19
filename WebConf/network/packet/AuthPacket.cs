using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConf.network.packet
{
    class AuthPacket : Packet
    {
        public string username;
        public string password;

        public AuthPacket(string username, string password) {
            this.username = username;
            this.password = password;
        }

        public override byte getPacketType() {
            return 1;
        }
    }
}
