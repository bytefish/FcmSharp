// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace FcmSharp.Exceptions
{
    public class FcmBadRequestException : FcmException
    {
        public FcmBadRequestException()
        {
        }

        public FcmBadRequestException(string message) : base(message)
        {
        }

        public FcmBadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
