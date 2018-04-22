// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;

namespace FcmSharp.Exceptions
{
    public class FcmHttpException : Exception
    {
        public FcmHttpException(HttpResponseMessage response)
        {
        }

        public FcmHttpException(HttpResponseMessage response, string message) 
            : base(message)
        {
        }
    }
}
