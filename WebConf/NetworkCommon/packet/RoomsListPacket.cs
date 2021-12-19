using common;
using common.field;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCommon.packet
{
    class RoomsListPacket : Packet
    {
        private fList<Room> rooms_list_field = new fList<Room>();

        public List<Room> rooms {
            get {
                return rooms_list_field.value;
            }
            set {
                rooms_list_field.value = value;
            }
        }
         
        public RoomsListPacket() { }

        public RoomsListPacket(List<Room> rooms) {
            this.rooms = rooms;
        }

        public RoomsListPacket(IEnumerable<Room> rooms)
        {
            this.rooms = new List<Room>(rooms);
        }
    }
}
