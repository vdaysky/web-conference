using common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCommon.packet
{
    class MessageReceivedPacket : Packet
    {
        public Message Message;

        public MessageReceivedPacket() {

        }

        public MessageReceivedPacket(Message message) {
            Message = message;
        }
    }
}
