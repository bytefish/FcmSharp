// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using FcmSharp.Requests.Converters;
using Newtonsoft.Json;

namespace FcmSharp.Requests.DeviceGroup
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class DeviceGroupMessage
    {
        [JsonProperty("operation")]
        [JsonConverter(typeof(OperationEnumConverter))]
        public readonly OperationEnum Operation;

        [JsonProperty("registration_ids")]
        public readonly IList<string> RegistrationIds;

        [JsonProperty("notification_key_name")]
        public readonly string NotificationKeyName;

        public DeviceGroupMessage(OperationEnum operation, IList<string> registrationIds, string notificationKeyName)
        {
            Operation = operation;
            RegistrationIds = registrationIds;
            NotificationKeyName = notificationKeyName;
        }
    }
}