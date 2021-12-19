using common;
using common.field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfServer.database
{
    class LoadedUser : User, LoadedObject
    {
        private fStr _password = new fStr();
        private fStr _cookie = new fStr();

        public string password {
            get {
                return _password.value;
            }
            set {
                _password.value = value;
            }
        }

        public string cookie {
            get {
                return _cookie.value;
            }
            set {
                _cookie.value = value;
            }
        }

        public LoadedUser(int? id, string login, string password, string cookie) : base(id, login) {
            this.password = password;
            if (cookie != null)
                this.cookie = cookie;
        }

        public LoadedUser() { }

        public void save(DataStorage database)
        {
            database.addOrEditUser(this);
        }
    }
}
