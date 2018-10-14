// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace FcmSharp.Serializer
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings settings;

        private JsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, settings);
        }

        public TTargetType DeserializeObject<TTargetType>(string value)
        {
            return JsonConvert.DeserializeObject<TTargetType>(value, settings);
        }

        public static JsonSerializer Default
        {
            get
            {
                return new JsonSerializer(new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
        }
    }
}