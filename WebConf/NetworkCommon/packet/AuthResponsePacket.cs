using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.field;

namespace common
{
    class AuthResponsePacket : Packet
    {
        private fInt32 result_value = new fInt32();
        public User user = null;

        public Int64? result {
            get {
                return result_value.value;
            }
            set {
                result_value.value = value;
            }
        }

        public AuthResponsePacket(int result, User user) {
            this.result = result;
            this.user = user;
        }

        public AuthResponsePacket() {

        }

        public bool isSuccess() {
            return result == 1;
        }
    }
}
