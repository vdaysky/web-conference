using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.field;

namespace common
{   
    class AuthPacket : Packet
    {
        private fStr username_field = new fStr();
        private fStr password_field = new fStr();

        public string username {
            get {
                return username_field.value;
            }
            set {
                password_field.value = value;
            }
        }

        public string password
        {
            get
            {
                return password_field.value;
            }
            set
            {
                password_field.value = value;
            }
        }


        public AuthPacket(string username, string password) {
            this.username = username;
            this.password = password;
        }

        public AuthPacket() {

        }
    }
}
