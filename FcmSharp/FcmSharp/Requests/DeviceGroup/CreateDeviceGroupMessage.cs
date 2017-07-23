// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FcmSharp.Requests.DeviceGroup
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CreateDeviceGroupMessage : DeviceGroupMessage
    {
        public CreateDeviceGroupMessage(IList<string> registrationIds, string notificationKeyName)
            : base(OperationEnum.Create, registrationIds, notificationKeyName)
        {
        }
    }
}
