using System;
using System.Collections.Generic;
using System.Linq;
using FcmSharp.Requests;
using Newtonsoft.Json;

// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FcmSharp.Responses.Converters
{
    public class TopicManagementResponseErrorEnumConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var errorString = (string) reader.Value;
            
            switch (errorString)
            {
                case "INVALID_ARGUMENT":
                    return TopicManagementResponseErrorEnum.InvalidArgument;
                case "NOT_FOUND":
                    return TopicManagementResponseErrorEnum.NotFound;
                case "INTERNAL":
                    return TopicManagementResponseErrorEnum.Internal;
                case "TOO_MANY_TOPICS":
                    return TopicManagementResponseErrorEnum.TooManyTopics;
                default:
                    return TopicManagementResponseErrorEnum.Unknown;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}