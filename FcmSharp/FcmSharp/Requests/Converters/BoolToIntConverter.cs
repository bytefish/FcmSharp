using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FcmSharp.Requests.Converters
{
    public class BoolToIntConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            bool booleanValue = (bool)value;
           
            writer.WriteStartObject();

            writer.WriteValue(Convert.ToInt32(booleanValue));
            
            writer.WriteEndObject();

        }
        

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TimeSpan?) == objectType;
        }
    }
}
