// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FcmSharp.Requests;
using FcmSharp.Serializer;
using NUnit.Framework;

namespace FcmSharp.Test.Requests
{
    [TestFixture]
    public class ApnsConfigTest
    {
        private static readonly IJsonSerializer serializer = JsonSerializer.Default;

        [Test]
        public void ApnsConfigSerializationTest()
        {
            ApnsConfig config = new ApnsConfig()
            {
                Payload = new ApnsConfigPayload()
                {
                    Aps = new Aps()
                    {
                        Badge = "badge",
                        Alert = new ApsAlert()
                        {
                            TitleLocKey = "title_loc_key",
                            ActionLocKey = "action_loc_key",
                            TitleLocArgs = new[] { "1", "2" },
                            Title = "Title",
                            Body = "Body",
                            LaunchImage = "LaunchImage",
                            LocArgs = new[] { "3", "4" },
                            LocKey = "LocKey"
                        },
                        Category = "category",
                        Sound = "sound",
                        CustomData = new Dictionary<string, object>()
                    {
                        {"CustomKey1", "CustomValue1"}
                    },
                        ContentAvailable = true,
                        MutableContent = true,
                        ThreadId = "1"
                    },
                    CustomData = new Dictionary<string, object>()
                {
                    {"CustomKey2", "CustomValue2"}
                }
                }
            };

            string result = serializer.SerializeObject(config);

            Assert.IsNotNull(result);
        }
    }
}
