// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;

namespace FcmSharp.Exceptions
{
    public class FcmHttpException : Exception
    {
        public readonly HttpResponseMessage HttpResponseMessage;

        public FcmHttpException(HttpResponseMessage httpResponseMessage)
        {
            HttpResponseMessage = httpResponseMessage;
        }

        public FcmHttpException(HttpResponseMessage httpResponseMessage, string message) 
            : base(message)
        {
            HttpResponseMessage = httpResponseMessage;
        }
    }
}
