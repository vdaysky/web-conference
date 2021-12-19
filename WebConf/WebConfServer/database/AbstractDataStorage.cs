using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;
using System.Security.Cryptography;

namespace WebConfServer.database
{
    abstract class AbstractDataStorage : DataStorage
    {
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

      
        public User authentcateUser(string login, string password)
        {
            LoadedUser user = getUserByLogin(login);
            if (user.password == GetHashString(password))
                return user;

            return null;
        }

        public abstract LoadedMesage getMessageById(long message_id);
        public abstract LoadedRoom getRoomById(long room_id);

        public abstract List<LoadedMesage> GetMessages(long room_id);
        public abstract List<LoadedRoom> GetRooms(long user_id);

        public abstract LoadedUser getUser(string cookie);
        public abstract LoadedUser getUserById(long id);
        public abstract LoadedUser getUserByLogin(string login);

        public abstract LoadedUser addOrEditUser(LoadedUser x);
        public abstract LoadedMesage addMessage(LoadedMesage x);
        public abstract LoadedRoom addRoom(LoadedRoom x);

        public abstract void save();
    }
}
