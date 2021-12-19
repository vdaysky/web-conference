using common.field;
using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    class GetRoomsPacket : Packet
    {
        private fList<Room> room_list_field = new fList<Room>();
        public List<Room> rooms {
            get {
                return room_list_field.value;
            }
            set {
                room_list_field.value = value;
            }
        }

        public GetRoomsPacket() { }
        public GetRoomsPacket(List<Room> rooms) {
            this.rooms = rooms;
        }
    }
}
