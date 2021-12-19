using common.field;
using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    class RegisterPacket : Packet
    {
        private fStr username_field = new fStr();
        private fStr password_field = new fStr();

        public string username
        {
            get
            {
                return username_field.value;
            }
            set
            {
                username_field.value = value;
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


        public RegisterPacket(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public RegisterPacket()
        {

        }
    }
}
