// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FcmSharp.Responses;

namespace FcmSharp.Exceptions
{
    public abstract class IidException : Exception
    {
        protected IidException(IidErrorCodeEnum error)
        {
        }

        protected IidException(string message) : base(message)
        {
        }

        protected IidException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
