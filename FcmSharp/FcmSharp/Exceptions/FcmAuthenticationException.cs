// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace FcmSharp.Exceptions
{
    public class FcmAuthenticationException : FcmException
    {
        public FcmAuthenticationException()
        {
        }

        public FcmAuthenticationException(string message) : base(message)
        {
        }

        public FcmAuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}