# FcmSharp #

[FcmSharp]: https://github.com/bytefish/FcmSharp
[Firebase Cloud Messaging HTTP Protocol]: https://firebase.google.com/docs/cloud-messaging/http-server-ref

[FcmSharp] is a .NET library for the Firebase Cloud Messaging (FCM) API. 

It implements the entire [Firebase Cloud Messaging HTTP Protocol] and supports:

* Downstream HTTP Messages
* Notification Payloads
* Topic Messages
* Device Group Messages

[FcmSharp] supports .NET Core as of Version 1.0.0.

## Installing FcmSharp ##

You can use [NuGet](https://www.nuget.org) to install [FcmSharp]. Run the following command 
in the [Package Manager Console](http://docs.nuget.org/consume/package-manager-console).

```
PM> Install-Package FcmSharp
```

## Quickstart ##

The Quickstart shows you how to work with [FcmSharp].

```csharp
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using FcmSharp.Model.Options;
using FcmSharp.Model.Topics;
using FcmSharp.Requests.Notification;
using FcmSharp.Requests.Topics;
using FcmSharp.Settings;

namespace FcmSharp.Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Read the Settings from a File, which is not under Version Control:
            var settings = new FileBasedFcmClientSettings("/Users/bytefish/api.key");

            // Construct the Client:
            using (var client = new FcmClient(settings))
            {
                // Construct the Notification to display:
                var notification = new NotificationPayload()
                {
                    Title = "ABC"
                };

                // Construct the Data Payload to send:
                var data = new
                {
                    A = new
                    {
                        a = 1,
                        b = 2
                    },
                    B = 2,
                };

                // Options for the Message:
                var options = FcmMessageOptions.Builder()
                    .setTimeToLive(TimeSpan.FromDays(1))
                    .Build();

                // The Message should be sent to the News Topic:
                var message = new TopicUnicastMessage<dynamic>(options, new Topic("news"), data);

                // Finally send the Message and wait for the Result:
                CancellationTokenSource cts = new CancellationTokenSource();

                // Send the Message and wait synchronously:
                var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();

                // Print the Result to the Console:
                System.Console.WriteLine("Result = {0}", result);
            }
        }
    }
}
```

## Synchronous API Calls ##

The ``FcmClient`` only provides an Asynchronous API. This is intentional and a Synchronous API won't be added. But I know that 
async programming can be tough for beginners. So here is how you can turn an async call into a synchronous one:

```csharp
var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();
```