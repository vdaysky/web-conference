using common;
using common.field;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCommon.packet
{
    class RoomCreatedPacket : Packet
    {
        public Room Room;

        public RoomCreatedPacket() { }
        public RoomCreatedPacket(Room room) {
            this.Room = room;
        }
    }
}
