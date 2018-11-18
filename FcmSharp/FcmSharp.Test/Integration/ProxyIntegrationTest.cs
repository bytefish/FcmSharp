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
    [TestFixture]
    public class ProxyIntegrationTest
    {
        /// <summary>
        /// 
        /// Test Instructions for Windows 10 (https://superuser.com/questions/180480/how-to-simulate-corporate-proxy-server-on-my-development-machine):
        /// 
        /// > 1. Download and run Fiddler proxy (it's free). It will automatically set itself as a system proxy in Windows on 
        /// >    each run. Also click Rules -> Require Proxy Authentication in the top menu if you want to test authentication 
        /// >    to the proxy (username and password are "1").
        /// >
        /// > 2. Open Windows Firewall, then Advanced settings -> Windows Firewall Properties. Block all outbound connections 
        /// >    for all profiles you need (domain, private, public) and click OK.
        /// >
        /// > 3. Add new outbound firewall rule to allow all access for 8888 port (default Fiddler port) or "%LocalAppData%\Programs\Fiddler\Fiddler.exe" app.
        /// >
        /// > That's it, only the programs which use your proxy settings (http://1:1@127.0.0.1:8888) will work.
        /// </summary>
        [Test]
        [Description("This Test uses Fiddler to enforce a Proxy and sends a Message using the Proxy settings")]
        [Ignore("This Test uses Fiddler Proxy to test Proxy Functionality")]
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

            // Construct the Firebase Client:
            using (var client = new FcmClient(settings, fcmHttpClient))
            {
                // Construct the Notification Payload to send:
                var notification = new Notification
                {
                    Title = "Title Text",
                    Body = "Notification Body Text"
                };

                // The Message should be sent to the News Topic:
                var message = new FcmMessage()
                {
                    ValidateOnly = false,
                    Message = new Message
                    {
                        Topic = "news",
                        Notification = notification
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