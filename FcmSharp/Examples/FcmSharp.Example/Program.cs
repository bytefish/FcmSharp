using System;
using System.Collections.Generic;
using System.Threading;
using FcmSharp.Requests;
using FcmSharp.Settings;

namespace FcmSharp.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Read the Credentials from a File, which is not under Version Control:
            var settings = FileBasedFcmClientSettings.CreateFromFile(@"your_project_id", @"D:\serviceAccountKey.json");

            // Construct the Client:
            using (var client = new FcmClient(settings))
            {
                // Construct the Data Payload to send:
                var data = new Dictionary<string, string>()
                {
                    {"A", "B"},
                    {"C", "D"}
                };

                // Get the Registration from Console:
                Console.Write("Device Token: ");

                string registrationId = Console.ReadLine();

                // The Message should be sent to the given token:
                var message = new FcmMessage()
                {
                    ValidateOnly = false,
                    Message = new Message
                    {
                        Token = registrationId,
                        Data = data
                    }
                };

                // Finally send the Message and wait for the Result:
                CancellationTokenSource cts = new CancellationTokenSource();

                // Send the Message and wait synchronously:
                var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();

                // Print the Result to the Console:
                Console.WriteLine("Message ID = {0}", result.Name);

                Console.ReadLine();
            }
        }
    }
}
