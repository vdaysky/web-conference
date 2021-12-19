using System;
using System.Collections.Generic;
using System.Text;
using common.field;

namespace common
{
    class MessageListPacket : Packet
    {
        private fList<Message> messages_field = new fList<Message>();

        public List<Message> messages {
            get {
                return messages_field.value;
            }
            set {
                messages_field.value = value;
            }
        }


        public MessageListPacket() {

        }

        public MessageListPacket(List<Message> messages) {
            this.messages = messages;
        }

        public MessageListPacket(IEnumerable<Message> messages)
        {
            this.messages = new List<Message>(messages);
        }
    }
}
