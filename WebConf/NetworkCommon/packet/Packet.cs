using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using Newtonsoft.Json;
using common.field;
using Newtonsoft.Json.Linq;

namespace common
{
   
    public class Packet
    {
        private fInt32 packetID_field = new fInt32(0);
        private fStr auth_token_field = new fStr("none");

        public long? packetID {
            get {
                return packetID_field.value;
            }
            set {
                packetID_field.value = value;
            }
        }

        public Packet withToken(string token) {
            auth_token = token;
            return this;
        }

        public string auth_token {
            get {
                return auth_token_field.value;
            }
            set {
                auth_token_field.value = value;
            }
        }

        public Packet() {
            packetID = getPacketID();
        }

        // Packet serialization. Won't work with inheritance if fields are private in middle class.
        public byte[] serialize() {
            Dictionary<string, object> packet = new Dictionary<string, object>();
            Dictionary<String, object> body = new Dictionary<string, object>();
            packet.Add("BODY", body);
            packet.Add("TYPE", this.GetType().Name);

            // Add all serializable fields from concrete class
            var x = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var f in x.Where(f => typeof(SerializableField).IsAssignableFrom(f.FieldType)))
            {
                SerializableField field = (SerializableField)f.GetValue(this);
                if (field == null)
                {
                    body.Add(f.Name, null);
                }
                else {
                    body.Add(f.Name, field.serialize());
                }
            }

            // Add all serializable fields from packet
            x = typeof(Packet).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var f in x.Where(f => typeof(SerializableField).IsAssignableFrom(f.FieldType)))
            {
                SerializableField field = (SerializableField)f.GetValue(this);
                if (field == null) {
                    body.Add(f.Name, null);
                }
                else {
                    body.Add(f.Name, field.serialize());
                }
                
            }

            return Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(packet));
        }

        public static Packet deserialize(byte[] bytes) {
            JObject receivedJson = (JObject) JsonConvert.DeserializeObject(Encoding.ASCII.GetString(bytes));

            IDictionary<string, object> packet = receivedJson.ToDictionary();

            var body = (Dictionary<String, object>) packet["BODY"];
            var class_name = (string) packet["TYPE"];

            var cls = Assembly.GetAssembly(typeof(Packet)).GetTypes().Where(t => t.Name == class_name).First();
            Packet packet_instance = (Packet) Activator.CreateInstance(cls);

            // Add fields from concrete class
            var x = cls.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var f in x.Where(f => typeof(SerializableField).IsAssignableFrom(f.FieldType)))
            {
                if (body[f.Name] == null) {
                    f.SetValue(packet_instance, null);
                    continue;
                }

                SerializableField field_instance = (SerializableField) Activator.CreateInstance(f.FieldType);
                field_instance.deserialize((Dictionary<string, object>) body[f.Name]);
                f.SetValue(packet_instance, field_instance);
            }

            // Add all serializable fields from packet
            x = typeof(Packet).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var f in x.Where(f => typeof(SerializableField).IsAssignableFrom(f.FieldType)))
            {
                if (body[f.Name] == null)
                {
                    f.SetValue(packet_instance, null);
                    continue;
                }

                SerializableField field_instance = (SerializableField)Activator.CreateInstance(f.FieldType);
                field_instance.deserialize((Dictionary<string, object>) body[f.Name]);
                f.SetValue(packet_instance, field_instance);
            }

            return packet_instance;
        }

        public long? getPacketID() {
            if (packetID == 0) {
                Random r = new Random();
                packetID = r.Next(1, 11474836);
            }
            return packetID;
        }

        public override string ToString() {
            return String.Format("<{0} ID={1} TOKEN={2}>", GetType().ToString(), getPacketID(), auth_token);
        }
    }

}
