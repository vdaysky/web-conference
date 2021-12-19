using common.field;
using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    class SendMessagePacket : Packet
    {
        public Message message;

        public SendMessagePacket() { }
        public SendMessagePacket(Message message) {
            this.message = message;
        }
    }
}
