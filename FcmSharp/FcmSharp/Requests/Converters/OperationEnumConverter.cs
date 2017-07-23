// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FcmSharp.Requests.DeviceGroup;
using Newtonsoft.Json;

namespace FcmSharp.Requests.Converters
{
    public class OperationEnumConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            OperationEnum operation = (OperationEnum)value;

            switch (operation)
            {
                case OperationEnum.Add:
                    writer.WriteValue("add");
                    break;
                case OperationEnum.Create:
                    writer.WriteValue("create");
                    break;
                case OperationEnum.Remove:
                    writer.WriteValue("remove");
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

            return Enum.Parse(typeof(OperationEnum), enumString, true);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}