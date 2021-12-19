using common;
using common.field;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfServer.database
{
    class JSONDataStorage : DataStorage {
        private Dictionary<string, object> _database = new Dictionary<string, object>();
        private Dictionary<string, object> _usersTable = new Dictionary<string, object>();
        private Dictionary<string, object> _messagesTable = new Dictionary<string, object>();
        private Dictionary<string, object> _roomsTable = new Dictionary<string, object>();

        public JSONDataStorage() {

            File.AppendText("database.json").Close();

            using (StreamReader r = new StreamReader("database.json"))
            {
                string json = r.ReadToEnd();
                _database = JsonConvert.DeserializeObject<Dictionary<string, object>> (json);
                try {
                    _usersTable = (Dictionary<string, object>) _database["users"];
                }
                catch {}
                try
                {
                    _messagesTable = (Dictionary<string, object>)_database["messages"];
                }
                catch { }
                try
                {
                    _roomsTable = (Dictionary<string, object>)_database["rooms"];
                }
                catch { }
            }
        }

        private string generateCookie() {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[16];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return "wbcnf-cookie-[" + new String(stringChars) + "]";
        }
        

        public LoadedMesage getMessageById(long message_id)
        {
            var x = new LoadedMesage();
            var y = _messagesTable[message_id.ToString()];
            if (y == null) {
                return null;
            }
            x.deserialize((Dictionary<string, object>)y);
            return x;
        }

        public List<LoadedMesage> GetMessages(long room_id)
        {
            List<LoadedMesage> messages = new List<LoadedMesage>();

            var x = new fList<LoadedMesage>();
            foreach (var y in _messagesTable.Keys) {
                LoadedMesage m = new LoadedMesage();
                m.deserialize((Dictionary<string, object>) _messagesTable[y]);
                if (m.room_id == room_id) {
                    messages.Add(m);
                }
            }
            return messages;
        }

        public LoadedRoom getRoomById(long room_id)
        {
            LoadedRoom m = new LoadedRoom();
            try
            {
                m.deserialize((Dictionary<string, object>)_roomsTable[room_id.ToString()]);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Room with ID {0} not found", room_id);
                return null;
            }

            return m;
        }

        public List<LoadedRoom> GetRooms(long user_id)
        {
            List<LoadedRoom> rooms = new List<LoadedRoom>();

            var x = new fList<LoadedRoom>();
            foreach (var y in _roomsTable.Keys)
            {
                LoadedRoom r = new LoadedRoom();
                r.deserialize((Dictionary<string, object>)_messagesTable[y]);
                if (r.Owner.id == user_id || r.members.Any(m => m.id == user_id))
                {
                    rooms.Add(r);
                }
            }
            return rooms;
        }

        public LoadedUser getUser(string cookie)
        {
            var x = new fList<LoadedUser>();
            foreach (var y in _usersTable.Keys)
            {
                LoadedUser r = new LoadedUser();
                r.deserialize((Dictionary<string, object>)_usersTable[y]);
                if (r.cookie == cookie)
                {
                    return r;
                }
            }
            return null;
        }

        public LoadedUser getUserById(long id)
        {
            LoadedUser user = new LoadedUser();
            Dictionary<string, object> d = (Dictionary<string, object>) _usersTable[id.ToString()];
            user.deserialize(d);
            return user;
        }

        public LoadedUser getUserByLogin(string login)
        {
            foreach (var y in _usersTable.Keys)
            {
                LoadedUser r = new LoadedUser();
                r.deserialize((Dictionary<string, object>)_usersTable[y]);
                if (r.name == login)
                {
                    return r;
                }
            }
            return null;
        }

        // TODO: only set ID on create

        public LoadedMesage addMessage(LoadedMesage x)
        {
            if (_messagesTable.Keys.Count > 0)
            {
                string last = _messagesTable.Keys.Last();
                int last_id = Int32.Parse(last);
                x.id = last_id + 1;
            }
            else {
                x.id = 1;
            }
            
            _messagesTable.Add(x.id.ToString(), x.serialize());
            return x;
        }

        public LoadedRoom addRoom(LoadedRoom x)
        {
            if (x.id == null) {
                if (_roomsTable.Keys.Count > 0)
                {
                    string last = _roomsTable.Keys.Last();
                    int last_id = Int32.Parse(last);
                    x.id = last_id + 1;
                }
                else
                {
                    x.id = 1;
                }
            }

            if (x.members == null) {
                x.members = new List<LoadedUser>();
            }
            Console.WriteLine("Add room with ID {0}", x.id);
            _roomsTable.Add(x.id.ToString(), x.serialize());
            return x;
        }   

        /// <summary>
        /// Update fieds of given user in database or create new one and set 
        /// neccesary fields, such as cookie and database id.
        /// </summary>
        public LoadedUser addOrEditUser(LoadedUser x)
        {
            if (x.id == null)
            {
                if (_usersTable.Keys.Count > 0)
                {
                    string last = _usersTable.Keys.Last();
                    int last_id = Int32.Parse(last);
                    x.id = last_id + 1;
                }
                else
                {
                    x.id = 1;
                }
            }
            

            if (x.cookie == null) {
                x.cookie = generateCookie();
            }

            _usersTable.Add(x.id.ToString(), x.serialize());
            return x;
        }

        public void save()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("users", _usersTable);
            data.Add("messages", _messagesTable);
            data.Add("rooms", _roomsTable);

            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText("database.json", json);
        }
    }
}
