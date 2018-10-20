// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FcmSharp.Responses;

namespace FcmSharp.Exceptions
{
    public class FcmMessageException : Exception
    {
        public readonly FcmMessageError Error;

        /// <summary>
        /// The raw body of the error HTTP response
        /// </summary>
        public readonly string ResponseBody;
        
        public FcmMessageException(FcmMessageError error, string responseBody, string message) 
            : base(message)
        {
            Error = error;
            ResponseBody = responseBody;
        }
    }
}
