// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Linq;
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
                var notification = new Notification
                {
                    Title = "Notification Title",
                    Body = "Notification Body Text"
                };

                var messages = tokens
                    // Create the Messages to be sent for each of the Tokens:
                    .Select(token => new Message
                    {
                        Token = token,
                        Notification = notification
                    })
                    // And turn it into an Array:
                    .ToArray();

                // Finally send the Message and wait for the Result:
                CancellationTokenSource cts = new CancellationTokenSource();

                // Send the Message and wait synchronously:
                var result = client.SendBatchAsync(messages, false, cts.Token).GetAwaiter().GetResult();

                // Print the Result to the Console:
                foreach (var response in result.Responses)
                {
                    Assert.IsNotNull(response.Name);
                }
            }
        }
    }
}
