// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Newtonsoft.Json;

namespace FcmSharp.Requests.Converters
{
    public class BoolToIntConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            bool booleanValue = (bool)value;
           
            writer.WriteValue(Convert.ToInt32(booleanValue));
        }
        

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Convert.ToBoolean(reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(bool) == objectType;
        }
    }
}
