using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConf.network.packet
{
    class AckResponsePacket : Packet
    {

        public override byte getPacketType()
        {
            return 2;
        }

    }
}
