using common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfServer.database
{
    class LoadedMesage : Message, LoadedObject
    {

        public LoadedMesage() { }
        public LoadedMesage(Message message) :
            base(message.id, message.text, message.author, message.date, message.parent_id, message.room_id) { }

        public void save(DataStorage database)
        {
            database.addMessage(this);
        }
    }
}
