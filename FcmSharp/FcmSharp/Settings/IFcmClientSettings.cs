// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FcmSharp.Settings
{
    public interface IFcmClientSettings
    {
        string FcmUrl { get; }

        string ApiKey { get; }
    }
}