// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Newtonsoft.Json;

namespace FcmSharp.Requests.Converters
{
    public class PriorityEnumConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PriorityEnum? priority = (PriorityEnum?)value;
            if (priority.HasValue)
            {
                switch (priority)
                {
                    case PriorityEnum.Normal:
                        writer.WriteValue("normal");
                        break;
                    case PriorityEnum.High:
                        writer.WriteValue("high");
                        break;
                }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;

            if (string.IsNullOrWhiteSpace(enumString))
            {
                return null;
            }

            return Enum.Parse(typeof(PriorityEnum), enumString, true);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}