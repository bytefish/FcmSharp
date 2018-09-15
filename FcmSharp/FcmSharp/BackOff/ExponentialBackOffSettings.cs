// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace FcmSharp.BackOff
{
    public class ExponentialBackOffSettings
    {
        /// <summary>
        /// The Maximum Number of Retries.  The default value is 10.
        /// </summary>
        public readonly int MaxNumberOfRetries;

        /// <summary>
        /// Gets the delta time span used to generate a random milliseconds to add to the next back-off.
        /// If the value is <see cref="System.TimeSpan.Zero"/> then the generated back-off will be exactly 1, 2, 4,
        /// 8, 16, etc. seconds. A valid value is between zero and one second. The default value is 250ms, which means
        /// that the generated back-off will be [0.75-1.25]sec, [1.75-2.25]sec, [3.75-4.25]sec, and so on.
        /// </summary>
        public readonly TimeSpan DeltaBackOff;

        /// <summary>
        /// Gets or sets the maximum time span to wait. If the back-off instance returns a greater time span than
        /// this value, the Request is cancelled. The default value is 16 seconds per a retry request.
        /// </summary>
        public readonly TimeSpan MaxTimeSpan;

        public ExponentialBackOffSettings(int maximumNumberRetries, TimeSpan deltaBackOff, TimeSpan maxTimeSpan)
        {
            MaxNumberOfRetries = maximumNumberRetries;
            DeltaBackOff = deltaBackOff;
            MaxTimeSpan = maxTimeSpan;
        }

        public static ExponentialBackOffSettings Default
        {
            get
            {
                return new ExponentialBackOffSettings(maximumNumberRetries: 10, deltaBackOff: TimeSpan.FromMilliseconds(250), maxTimeSpan: TimeSpan.FromSeconds(16));
            }
        }
    }
}
