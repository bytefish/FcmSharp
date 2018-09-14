// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FcmSharp.Settings
{
    public class FcmClientSettings : IFcmClientSettings
    {
        public string Project { get; private set; }

        public string Credentials { get; private set; }

        public FcmClientSettings(string project, string credentials)
        {
            Project = project;
            Credentials = credentials;
        }
    }
}