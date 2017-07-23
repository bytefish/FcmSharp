// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FcmSharp.Requests;

namespace FcmSharp.Model.Options
{
    public class FcmMessageOptionsBuilder
    {

        private string condition;
        private string collapseKey;
        private PriorityEnum? priorityEnum;
        private bool? contentAvailable;
        private bool? delayWhileIdle;
        private int timeToLive = 60;
        private string restrictedPackageName = null;
        private bool? dryRun;

        public FcmMessageOptionsBuilder setCondition(string condition) {
            this.condition = condition;

            return this;
        }

        public FcmMessageOptionsBuilder setCollapseKey(string collapseKey) {
            this.collapseKey = collapseKey;

            return this;
        }

        public FcmMessageOptionsBuilder setPriorityEnum(PriorityEnum priorityEnum) {
            this.priorityEnum = priorityEnum;

            return this;
        }

        public FcmMessageOptionsBuilder setContentAvailable(bool contentAvailable) {
            this.contentAvailable = contentAvailable;

            return this;
        }

        public FcmMessageOptionsBuilder setDelayWhileIdle(bool delayWhileIdle) {
            this.delayWhileIdle = delayWhileIdle;

            return this;
        }

        public FcmMessageOptionsBuilder setTimeToLive(TimeSpan timeToLive) {
            this.timeToLive = (int) timeToLive.TotalSeconds;

            return this;
        }

        public FcmMessageOptionsBuilder setRestrictedPackageName(String restrictedPackageName) {
            this.restrictedPackageName = restrictedPackageName;

            return this;
        }

        public FcmMessageOptionsBuilder setDryRun(bool dryRun) {
            this.dryRun = dryRun;

            return this;
        }

        public FcmMessageOptions Build() {
            return new FcmMessageOptions(condition, collapseKey, priorityEnum, contentAvailable, delayWhileIdle, timeToLive, restrictedPackageName, dryRun);
        }
    }
}