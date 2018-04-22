// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Newtonsoft.Json;

namespace FcmSharp.Requests.Converters
{
    public class DurationFormatConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TimeSpan? timeSpan = (TimeSpan?)value;

            if (!timeSpan.HasValue)
            {
                return;
            }

            string timeToLiveInSeconds = string.Format("{0}s", (int) timeSpan.Value.TotalSeconds);
            
            writer.WriteValue(timeToLiveInSeconds);

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
