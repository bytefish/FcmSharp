// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Http;
using Google.Apis.Http;

namespace FcmSharp.Http.Proxy
{
    public class ProxyHttpClientFactory : IHttpClientFactory
    {
        private readonly IWebProxy webProxy;

        public ProxyHttpClientFactory(IWebProxy webProxy)
        {
            this.webProxy = webProxy;
        }

        public ProxyHttpClientFactory(Uri proxy, ICredentials credentials)
        {
            this.webProxy = new WebProxy(proxy, credentials);
        }

        public ConfigurableHttpClient CreateHttpClient(CreateHttpClientArgs args)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler()
            {
                UseProxy = true,
                Proxy = webProxy
            };

            ConfigurableMessageHandler httpMessageHandler = new ConfigurableMessageHandler(httpClientHandler);

            return new ConfigurableHttpClient(httpMessageHandler);
        }
    }
}
