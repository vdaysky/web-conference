using System;
using System.Collections.Generic;
using System.Text;

namespace common.field
{
    interface SerializableField
    {
        Dictionary<string, object> serialize();
        void deserialize(Dictionary<string, object> x);
        bool is_initialized();
    }
}
