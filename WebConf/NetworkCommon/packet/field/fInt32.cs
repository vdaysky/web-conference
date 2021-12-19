using System;
using System.Collections.Generic;
using System.Text;

namespace common.field
{
    class fInt32 : SerializableField
    {
        private bool initialized = false;

        private Int64? _value = null;
        
        public Int64? value {
            get {
                return _value;
            }
            set {
                initialized = true;
                _value = value;
            }
        }

        public bool is_initialized() {
            return initialized;
        }

        public fInt32() {

        }

        public fInt32(Int64? x) {
            value = x;
        }

        public void deserialize(Dictionary<string, object> x)
        {
            this.value = (Int64?) x["value"];
        }

        public Dictionary<string, object> serialize()
        {
            return new Dictionary<string, object>() { { "value", value } };
        }
    }
}
