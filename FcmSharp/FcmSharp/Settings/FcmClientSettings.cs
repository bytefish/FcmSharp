// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.BackOff;

namespace FcmSharp.Settings
{
    public class FcmClientSettings : IFcmClientSettings
    {
        public string Project { get; }

        public string Credentials { get; }

        public ExponentialBackOffSettings ExponentialBackOffSettings { get; }

        public FcmClientSettings(string project, string credentials)
            : this(project, credentials, ExponentialBackOffSettings.Default)
        {
        }

        public FcmClientSettings(string project, string credentials, ExponentialBackOffSettings exportExponentialBackOffSettings)
        {
            Project = project;
            Credentials = credentials;
            ExponentialBackOffSettings = exportExponentialBackOffSettings;
        }
    }
}