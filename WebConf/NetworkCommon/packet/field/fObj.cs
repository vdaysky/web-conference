using NetworkCommon.extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace common.field
{
    public abstract class fObj : SerializableField
    {
        protected bool initialized = false;

        private List<FieldInfo> getFieldsRecursive(Type type) {
            List<FieldInfo> fields = new List<FieldInfo>();

            if (type.BaseType != null) {
                fields.AddRange(getFieldsRecursive(type.BaseType));
            }
            fields.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
            return fields.DistinctBy(x => x.Name).ToList();
        }

        public void deserialize(Dictionary<string, object> x)
        {
            initialized = true;

            var fields = getFieldsRecursive(GetType());

            foreach (var f in fields.Where(f => typeof(SerializableField).IsAssignableFrom(f.FieldType)))
            {
                SerializableField field_instance = (SerializableField)Activator.CreateInstance(f.FieldType);
                Dictionary<string, object> primitive_repr = (Dictionary<string, object>)x[f.Name];

                if (primitive_repr != null) {
                    field_instance.deserialize(primitive_repr);
                }
                
                f.SetValue(this, field_instance);
            }
        }

        public bool is_initialized() {
            return initialized;
        }

        public void set_initialized(bool x) {
            initialized = x;
        }

        public Dictionary<string, object> serialize()
        {
            Dictionary<string, object> x = new Dictionary<string, object>();

            var fields = getFieldsRecursive(GetType());

            foreach (var f in fields.Where(f => typeof(SerializableField).IsAssignableFrom(f.FieldType)))
            {
                SerializableField field_instance = (SerializableField) f.GetValue(this);
                Console.WriteLine("Add {0}", f.Name);
                if (field_instance == null) {
                    x.Add(f.Name, null);
                } else {
                    x.Add(f.Name, field_instance.is_initialized() ? field_instance.serialize() : null);
                }
            }

            return x;
        }
    }
}
