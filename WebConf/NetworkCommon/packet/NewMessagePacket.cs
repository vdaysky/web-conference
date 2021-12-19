using common.field;
using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    class NewMessagePacket : Packet
    {
        public Message message;

        public NewMessagePacket() { }
        public NewMessagePacket(Message message)
        {
            this.message = message;
        }
    }
}
