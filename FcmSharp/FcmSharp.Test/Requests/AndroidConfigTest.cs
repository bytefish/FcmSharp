// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using FcmSharp.Requests;
using FcmSharp.Serializer;
using NUnit.Framework;

namespace FcmSharp.Test.Requests
{
    [TestFixture]
    public class AndroidConfigTest
    {
        private static readonly IJsonSerializer serializer = JsonSerializer.Default;

        [Test]
        public void AndroidConfigSerializationDeserializationTest()
        {
            AndroidConfig config = new AndroidConfig()
            {
                CollapseKey = "collapse_key",
                Data = new Dictionary<string, string>() { { "A", "B" } },
                Priority = AndroidMessagePriorityEnum.HIGH,
                Notification = new AndroidNotification()
                {
                    BodyLocArgs = new[] { "1", "2"},
                    Body = "body",
                    Color = "color",
                    Tag = "tag",
                    BodyLocKey = "body_loc_key",
                    ClickAction = "click_action",
                    Sound = "sound",
                    Icon = "icon",
                    Title = "title",
                    TitleLocArgs = new [] { "3", "4"},
                    TitleLocKey = "title_loc_key"
                },
                TimeToLive = TimeSpan.FromSeconds(10),
                RestrictedPackageName = "restricted_package_name"
            };

            var result = serializer.SerializeObject(config);

            Assert.IsNotNull(result);
        }
    }
}
