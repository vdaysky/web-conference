using System;
using System.Collections.Generic;
using System.Text;
using common.field;

namespace common
{
    public class Message : fObj {
  
 
        private fInt32 id_field = new fInt32();
        private fStr text_field = new fStr();
        private fDateTime date_field = new fDateTime();
        private fInt32 parent_id_field = new fInt32();
        private fInt32 room_id_field = new fInt32();
        public User author;

        public long? room_id {
            get {
                return room_id_field.value;
            }
            set {
                room_id_field.value = value;
            }
        }
        
        public long? id {
            get {
                return id_field.value;
            }
            set {
                id_field.value = value;
                set_initialized(true);
            }
        }

        public long? parent_id
        {
            get
            {
                return parent_id_field.value;
            }
            set
            {
                if (value != null) {
                    parent_id_field.value = (long) value;
                    set_initialized(true);
                }
            }
        }

        public string text {
            get { return text_field.value; }
            set { text_field.value = value; set_initialized(true); }
        }

        public DateTime date
        {
            get { return date_field.value; }
            set { date_field.value = value; initialized = true; }
        }

        public Message(long? id, string text, User user, DateTime? date, long? parent_id, long? room_id) {
            if (id != null)
                this.id = (int)id;

            this.text = text;

            if (user != null)
                this.author = user;

            if (date != null)
                this.date = (DateTime) date;

            if (parent_id != null) {
                this.parent_id = (int) parent_id;
            }
            if (room_id != null) {
                this.room_id = (int) room_id;
            }
        }

        public Message() {

        }
    }
}
