// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using System.Threading;
using FcmSharp.Http.Client;
using FcmSharp.Http.Proxy;
using FcmSharp.Requests;
using FcmSharp.Settings;

namespace FcmSharp.Test.Integration
{
    [Explicit("This Test uses Fiddler Proxy to test Proxy Functionality")]
    public class ProxyIntegrationTest
    {
        [Test]
        [Description("This Test uses Fiddler to enforce a Proxy and sends a Message using the Proxy settings")]
        public void SendFcmMessageUsingProxyTest()
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
            var fcmHttpClient = new FcmHttpClient(settings, httpClientFactory);

            // Finally
            using (var client = new FcmClient(settings, fcmHttpClient))
            {

                // Construct the Data Payload to send:
                var data = new Dictionary<string, string>()
                {
                    {"A", "B"},
                    {"C", "D"}
                };

                // The Message should be sent to the News Topic:
                var message = new FcmMessage()
                {
                    ValidateOnly = false,
                    Message = new Message
                    {
                        Topic = "news",
                        Data = data
                    }
                };

                // Finally send the Message and wait for the Result:
                CancellationTokenSource cts = new CancellationTokenSource();

                // Send the Message and wait synchronously:
                var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();

                Console.WriteLine(result);
            }
        }

    }
}