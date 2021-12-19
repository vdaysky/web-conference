using System;
using System.Collections.Generic;
using System.Text;

namespace common.field
{
    class fList<T> : SerializableField where T : SerializableField
    {
        public List<T> _value = new List<T>();
        private bool initialized = false;

        public List<T> value {
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

        public void deserialize(Dictionary<string, object> x)
        {
            foreach (object o in (IEnumerable<object>) x["value"]) {
                T instance = (T) Activator.CreateInstance(typeof(T));
                instance.deserialize( (Dictionary<string, object>) o);
                this.value.Add(instance);
            }
        }

        public Dictionary<string, object> serialize()
        {
            Dictionary<string, object> x = new Dictionary<string, object>();
            List<object> l = new List<object>();
            x.Add("value", l);

            foreach (T y in this.value) {
                l.Add(y.serialize());
            }
            return x;
        }
    }
}
