using common.field;
using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    public class Room : fObj
    {
        private fInt32 room_id_field = new fInt32();
        private fStr room_name_field = new fStr();

        public User Owner = null;

        public long? id {
            get {
                if (room_id_field.is_initialized())
                    return room_id_field.value;
                return null;
            }
            set {
                room_id_field.value = (long) value;
                set_initialized(true);
            }
        } 

        public string name {
            set {
                room_name_field.value = value;
                set_initialized(true);
            }
            get {
                return room_name_field.value;
            }
        }

        public Room() { }
        public Room(int? room_id, string name, User owner) {
            if (room_id != null)
                this.id = (int) room_id;
            this.name = name;
            this.Owner = owner;
        }
    }
}
