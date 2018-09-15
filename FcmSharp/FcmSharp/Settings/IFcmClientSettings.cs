// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.BackOff;

namespace FcmSharp.Settings
{
    public interface IFcmClientSettings
    {
        string Project { get; }
        
        string Credentials { get; }

        ExponentialBackOffSettings ExponentialBackOffSettings { get; }
    }
}