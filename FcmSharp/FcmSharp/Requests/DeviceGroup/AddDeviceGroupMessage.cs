// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FcmSharp.Requests.DeviceGroup
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AddDeviceGroupMessage : DeviceGroupMessage
    {
        public AddDeviceGroupMessage(IList<string> registrationIds, string notificationKeyName, string notificationKey)
            : base(OperationEnum.Add, registrationIds, notificationKeyName)
        {
            NotificationKey = notificationKey;
        }

        [JsonProperty("notification_key")]
        public string NotificationKey { get; set; }
    }
}
