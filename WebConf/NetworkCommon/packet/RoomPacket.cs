using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    class RoomPacket : Packet
    {
        public Room room;

        public RoomPacket() {

        }

        public RoomPacket(Room room) {
            this.room = room;
        }
    }
}
