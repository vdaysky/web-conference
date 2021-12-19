using common.field;
using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    class CreateRoomPacket : Packet
    {
        private fStr name_field = new fStr();
        public string RoomName {
            get {
                return name_field.value;
            }
            set {
                name_field.value = value;
            }
        }

        public CreateRoomPacket() { }
        public CreateRoomPacket(string name) {
            RoomName = name;
        }
    }
}
