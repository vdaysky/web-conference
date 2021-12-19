using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfServer.database
{
    interface DataStorage {
        LoadedMesage getMessageById(long message_id);
        List<LoadedMesage> GetMessages(long room_id);
        LoadedRoom getRoomById(long room_id);
        List<LoadedRoom> GetRooms(long user_id);
        LoadedUser getUser(string cookie);
        LoadedUser getUserById(long id);
        LoadedUser getUserByLogin(string login);

        LoadedUser addOrEditUser(LoadedUser x);
        LoadedMesage addMessage(LoadedMesage x);
        LoadedRoom addRoom(LoadedRoom x);
        void save();
    }

}
