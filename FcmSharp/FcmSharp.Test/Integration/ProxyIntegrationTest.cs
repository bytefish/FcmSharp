// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.BackOff;
using FcmSharp.Exceptions;
using FcmSharp.Http.Builder;
using FcmSharp.Http.Client;
using FcmSharp.Http.Proxy;
using FcmSharp.Settings;
using Google.Apis.Http;

namespace FcmSharp.Test.Integration
{
    [Explicit("This Test uses Fiddler Proxy to test Proxy Functionality")]
    public class ProxyIntegrationTest
    {
        [Test]
        [Description("This Test uses Fiddler to enforce a Proxy and sends a Message using the Proxy settings")]
        public void ExponentialBackoff503Test()
        {
            // This needs to be a valid Service Account Credentials File. Can't mock it away:
            var settings = FileBasedFcmClientSettings.CreateFromFile("your_project_id", @"D:\serviceAccountKey.json");

            // Define the Proxy URI to be used:
            var proxy = new Uri("http://localhost:8888");

            // Define the Username and Password ("1", because I am using Fiddler for Testing):
            var credentials = new NetworkCredential("1", "1");

            // Build the HTTP Client Factory:
            var httpClientFactory = new ProxyHttpClientFactory(proxy, credentials);
            
            // Initialize a new FcmHttpClient to send to localhost:
            var client = new FcmHttpClient(settings, httpClientFactory);

            
        }

    }
}