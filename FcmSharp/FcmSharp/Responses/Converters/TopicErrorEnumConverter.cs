// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Newtonsoft.Json;

namespace FcmSharp.Responses.Converters
{
    public class TopicErrorEnumConverter : JsonConverter
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
                    return TopicManagementResponse.Error.InvalidArgument;
                case "NOT_FOUND":
                    return TopicManagementResponse.Error.NotFound;
                case "INTERNAL":
                    return TopicManagementResponse.Error.Internal;
                case "TOO_MANY_TOPICS":
                    return TopicManagementResponse.Error.TooManyTopics;
                case "PERMISSION_DENIED":
                    return TopicManagementResponse.Error.PermissionDenied;
                default:
                    return TopicManagementResponse.Error.Unknown;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}