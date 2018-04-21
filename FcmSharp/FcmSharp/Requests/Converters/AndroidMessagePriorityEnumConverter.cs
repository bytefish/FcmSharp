// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FcmSharp.Requests.DeviceGroup;
using Newtonsoft.Json;

namespace FcmSharp.Requests.Converters
{
    public class AndroidMessagePriorityEnumConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            AndroidMessagePriorityEnum operation = (AndroidMessagePriorityEnum)value;

            switch (operation)
            {
                case AndroidMessagePriorityEnum.HIGH:
                    writer.WriteValue("HIGH");
                    break;
                case AndroidMessagePriorityEnum.NORMAL:
                    writer.WriteValue("NORMAL");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;

            if (string.IsNullOrWhiteSpace(enumString))
            {
                return null;
            }

            return Enum.Parse(typeof(AndroidMessagePriorityEnum), enumString, true);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}