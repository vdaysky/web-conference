using System;
using System.Collections.Generic;
using System.Text;

namespace common.field
{
    class fDateTime : SerializableField
    {
        private long ts;
        private bool initialized = false;

        public fDateTime() {

        }

        public DateTime value {
            get {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return dateTime.AddSeconds(ts).ToLocalTime();
            }
            set {
                ts = ((DateTimeOffset)value).ToUnixTimeSeconds();
                initialized = true;
            }
        }

        public Boolean is_initialized() {
            return initialized;
        }

        public fDateTime(DateTime dt) {
            ts = ((DateTimeOffset)dt).ToUnixTimeSeconds();
        }

        public void deserialize(Dictionary<string, object> x)
        {
            ts = (long) x["value"];
        }

        public Dictionary<string, object> serialize()
        {
            return new Dictionary<string, object>() { { "value", ts } };
        }
    }
}
