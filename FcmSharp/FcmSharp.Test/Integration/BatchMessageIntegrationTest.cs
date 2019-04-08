// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Threading;
using FcmSharp.Requests;
using FcmSharp.Settings;
using NUnit.Framework;

namespace FcmSharp.Test.Integration
{
    [TestFixture]
    class BatchMessageIntegrationTest
    {
        [Test]
        public void SendBatchMessages()
        {
            // This is a list of Tokens I want to send the Batch Message to:
            var tokens = File.ReadAllLines(@"D:\device_tokens.txt");

            // Read the Credentials from a File, which is not under Version Control:
            var settings = FileBasedFcmClientSettings.CreateFromFile(@"D:\serviceAccountKey.json");

            // Construct the Client:
            using (var client = new FcmClient(settings))
            {

                Message message = new Message
                {
                    Notification = new Notification
                    {
                        Title = "Notification Title",
                        Body = "Notification Body Text"
                    }
                };

                // Finally send the Message and wait for the Result:
                CancellationTokenSource cts = new CancellationTokenSource();

                // Send the Message and wait synchronously:
                var result = client.SendMulticastMessage(tokens, message, false, cts.Token).GetAwaiter().GetResult();

                // Print the Result to the Console:
                foreach (var response in result.Responses)
                {
                    Assert.IsNotNull(response.Name);
                }
            }
        }
    }
}
