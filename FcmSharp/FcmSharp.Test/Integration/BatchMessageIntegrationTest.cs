using System;
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
            // Read the Credentials from a File, which is not under Version Control:
            var settings = FileBasedFcmClientSettings.CreateFromFile(@"D:\serviceAccountKey.json");

            // Construct the Client:
            using (var client = new FcmClient(settings))
            {
                const string token1 = "XXX";
                const string token2 = "XXX";

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
