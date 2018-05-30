// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FcmSharp.Responses;

namespace FcmSharp.Exceptions
{
    public class FcmMessageException : Exception
    {
        public readonly FcmMessageErrorResponse Error;
        
        public FcmMessageException(FcmMessageErrorResponse error, string message) 
            : base(message)
        {
            Error = error;
        }
    }
}
