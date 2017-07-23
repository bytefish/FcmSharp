// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Requests;

namespace FcmSharp.Model.Options
{
    public class FcmMessageOptions
    {
        public readonly string Condition;
        public readonly string CollapseKey;
        public readonly PriorityEnum? PriorityEnum;
        public readonly bool? ContentAvailable;
        public readonly bool? DelayWhileIdle;
        public readonly int? TimeToLive;
        public readonly string RestrictedPackageName;
        public readonly bool? DryRun;

        public FcmMessageOptions(string condition, string collapseKey, PriorityEnum? priorityEnum, bool? contentAvailable, bool? delayWhileIdle, int? timeToLive, string restrictedPackageName, bool? dryRun) {
            Condition = condition;
            CollapseKey = collapseKey;
            PriorityEnum = priorityEnum;
            ContentAvailable = contentAvailable;
            DelayWhileIdle = delayWhileIdle;
            TimeToLive = timeToLive;
            RestrictedPackageName = restrictedPackageName;
            DryRun = dryRun;
        }

        public static FcmMessageOptionsBuilder Builder() {
            return new FcmMessageOptionsBuilder();
        }
    }
}