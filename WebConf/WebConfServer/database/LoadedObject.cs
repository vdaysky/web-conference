using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebConfServer.database
{
    interface LoadedObject
    {
        void save(DataStorage database);
    }
}
