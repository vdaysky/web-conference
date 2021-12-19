using common;
using common.field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfServer.database
{
    class LoadedRoom : Room, LoadedObject
    {
        private fList<LoadedUser> _members = new fList<LoadedUser>();
        public List<LoadedUser> members {
            get {
                return _members.value;
            }
            set {
                _members.value = value;
            }
        }

        public LoadedRoom(int? room_id, string title, List<LoadedUser> users, LoadedUser owner) : base(room_id, title, owner) {
            members = users;
        }

        public LoadedRoom() { }

        public void save(DataStorage database)
        {
            database.addRoom(this);
        }
    }
}
