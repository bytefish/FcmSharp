// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace FcmSharp.Exceptions
{
    public class FcmUnavailableException : FcmException
    {
        public FcmUnavailableException()
        {
        }

        public FcmUnavailableException(string message) : base(message)
        {
        }

        public FcmUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
