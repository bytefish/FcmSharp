// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace FcmSharp.Exceptions
{
    public class FcmGeneralException : FcmException
    {
        public FcmGeneralException()
        {
        }

        public FcmGeneralException(string message) : base(message)
        {
        }

        public FcmGeneralException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}