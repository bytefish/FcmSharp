// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FcmSharp.Requests
{
    public enum FcmErrorCodes
    {
        RegistrationTokenNotRegistered = 0,

        MismatchedCredential = 1,

        MessageRateExceeded = 2,

        InvalidApnsCredentials = 3,

        InternalError = 4,

        InvalidArgument = 5,

        ServerUnavailable = 6
    }
}
