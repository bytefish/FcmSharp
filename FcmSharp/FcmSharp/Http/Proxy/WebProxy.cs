// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace FcmSharp.Http.Proxy
{
    public class WebProxy : IWebProxy
    {
        public WebProxy(Uri proxy, ICredentials credentials)
        {
            Proxy = proxy;
            Credentials = credentials;
        }

        public Uri GetProxy(Uri destination)
        {
            return Proxy;
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }

        public Uri Proxy { get; set; }

        public ICredentials Credentials { get; set; }
    }
}
