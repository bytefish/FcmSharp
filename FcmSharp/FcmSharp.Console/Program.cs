// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using FcmSharp.Requests;
using FcmSharp.Settings;

namespace FcmSharp.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
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

                // Print the Result to the Console:
                Console.WriteLine("Data Message ID = {0}", result.Name);

                Console.WriteLine("Press Enter to exit ...");
                Console.ReadLine();
            }

            SendBatchMessages();
        }

        public static void SendBatchMessages()
        {
            // Read the Credentials from a File, which is not under Version Control:
            var settings = FileBasedFcmClientSettings.CreateFromFile(@"D:\serviceAccountKey.json");

            // Construct the Client:
            using (var client = new FcmClient(settings))
            {
                const string token1 = "";
                const string token2 = "";

                var notification = new Notification
                {
                    Title = "Notification Title",
                    Body = "Notification Body Text"
                };

                var messages = new[]
                {
                    new Message
                    {
                        Token = token1,
                        Notification = notification
                    },
                    new Message
                    {
                        Token = token2,
                        Notification = notification
                    },
                };

                // Finally send the Message and wait for the Result:
                CancellationTokenSource cts = new CancellationTokenSource();

                // Send the Message and wait synchronously:
                var result = client.SendBatchAsync(messages, false, cts.Token).GetAwaiter().GetResult();

                // Print the Result to the Console:
                Console.WriteLine($"Batch SuccessCount = {result.SuccessCount}, FailureCount = {result.FailureCount}");

                Console.WriteLine("Batch Responses:");

                foreach (var response in result.Responses)
                {
                    Console.WriteLine($"SendResponse MessageId = {response.MessageId}, Success = {response.Success}");
                }

                Console.WriteLine("Press Enter to exit ...");
                Console.ReadLine();
            }
        }
    }
}
