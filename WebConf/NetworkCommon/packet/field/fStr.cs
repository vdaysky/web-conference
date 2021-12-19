using System;
using System.Collections.Generic;
using System.Text;

namespace common.field
{
    class fStr : SerializableField
    {
        private string _value;
        private bool initialized = false;

        public string value {
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

        public fStr() {

        }

        public fStr(string x) {
            this.value = x;
        }

        public void deserialize(Dictionary<string, object> x)
        {
            this.value = (string) x["value"];
        }

        public Dictionary<string, object> serialize()
        {
            return new Dictionary<string, object>() { {"value", value} };
        }
    }
}
