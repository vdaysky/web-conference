using common;
using common.field;
using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    class GetRoomPacket : Packet
    {
        private fInt32 room_id_field = new fInt32();
        public Int64? room_id {
            get {
                return room_id_field.value;
            }
            set {
                room_id_field.value = value;
            }
        }

        public GetRoomPacket() { }
        public GetRoomPacket(long room_id) {
            this.room_id = room_id;
        }
    }
}
