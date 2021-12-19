using System;
using System.Collections.Generic;
using System.Text;
using common.field;

namespace common
{
    public class User : fObj
    {
        private fStr name_field = new fStr();
        private fInt32 id_field = new fInt32();

        public string name {
            get {
                return name_field.value;
            }
            set {
                name_field.value = value;
                set_initialized(true);
            }
        }

        public long? id {
            get {
                if (!id_field.is_initialized()) {
                    return null;
                }
                return id_field.value;
            }
            set {
                if (value != null)
                {
                    id_field.value = (int) value;
                    set_initialized(true);
                }
            }
        }

        public User() {

        }

        public User(Int32? id, string name) {
            this.name = name;
            if (id != null)
                this.id = (int) id;
        }
    }
}
