// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace FcmSharp.Http.Retry
{
    public class RetryConditionValue
    {
        public readonly DateTimeOffset? Date;
        public readonly TimeSpan? Delta;

        public RetryConditionValue(DateTimeOffset date)
        {
            Date = date;
        }

        public RetryConditionValue(TimeSpan delta)
        {
            Delta = delta;
        }
    }
}