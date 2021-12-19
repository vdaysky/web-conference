using System;
using System.Collections.Generic;
using System.Text;

using common;
using common.field;

namespace common
{
    class GetMessagesPacket : Packet {
        private fInt32 room_id_field = new fInt32();
        private fInt32 parent_message_id_field = new fInt32();

        public long? RoomID {
            get {
                return room_id_field.value;
            }
            set {
                room_id_field.value = value;
            }
        }
        public long? parent_id {
            get {
                return parent_message_id_field.value;
            }
            set {
                parent_message_id_field.value = value;
            }
        }

        public GetMessagesPacket() {

        }

        public GetMessagesPacket(long room_id, long? parent_message_id) {
            RoomID = room_id;
            if (parent_message_id != null) {
                this.parent_id = (long) parent_message_id;
            }
        }
    }
}
