// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FcmSharp.Settings
{
    public class FcmClientSettings
    {
        public readonly string FcmUrl;

        public FcmClientSettings(string projectId)
        {
            FcmUrl = $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send";
        }
        
    }
}