# FcmSharp #

[![stable](https://img.shields.io/nuget/v/FcmSharp.svg?label=stable)](https://www.nuget.org/packages/FcmSharp/)

[FcmSharp]: https://github.com/bytefish/FcmSharp
[Firebase Cloud Messaging HTTP Protocol]: https://firebase.google.com/docs/cloud-messaging/http-server-ref

[FcmSharp] is a .NET library for the Firebase Cloud Messaging (FCM) API. 

It implements the Firebase Cloud Messaging HTTP v1 API:

* https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages

[FcmSharp] supports .NET Core as of Version 1.0.0.


## Why is this repository archived? ##

This library was written at a time, when the official Firebase Admin SDK for .NET didn't support the Firebase Cloud Messaging API. The Messaging Features have been added to the [Firebase Admin SDK for .NET](https://github.com/firebase/firebase-admin-dotnet) lately, so you should use the official [Firebase Admin SDK for .NET](https://github.com/firebase/firebase-admin-dotnet) instead of this library: 

* [https://github.com/firebase/firebase-admin-dotnet](https://github.com/firebase/firebase-admin-dotnet)

## Installing FcmSharp ##

You can use [NuGet](https://www.nuget.org) to install [FcmSharp]. Run the following command 
in the [Package Manager Console](http://docs.nuget.org/consume/package-manager-console).

```
PM> Install-Package FcmSharp
```

## On Disposing the FcmClient ##

[Simon Timms]: https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
[this great post]: https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/

You should reuse the ``FcmClient``, instead of disposing it for each request. Internally the ``FcmClient`` uses the 
``HttpClient``, which shouldn't be disposed for every request. Please read [this great post] by [Simon Timms] to understand the 
details of this behavior in .NET. 

The ``FcmClient`` only uses thread-safe operations, so it is safe to be shared between threads.

## Quickstart: Sending Notifications ##

```csharp
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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
        }
    }
}
```

## Complete Example for Sending, Receiving and Displaying a Notification ##

### Setting up the Firebase Project ###

Please follow the official documentation for creating a Firebase project:

* [https://firebase.google.com/docs/](https://firebase.google.com/docs/)

### Android-side  ###

[quickstart-android]: https://github.com/firebase/quickstart-android
[messaging]: https://github.com/firebase/quickstart-android/tree/master/messaging/
[Android Studio]: https://developer.android.com/studio
[Examples/Android/messaging-app]: https://github.com/bytefish/FcmSharp/tree/master/FcmSharp/Examples/Android/messaging-app

I have prepared an Android project called ``messaging-app`` at [Examples/Android/messaging-app], that you can use to easily get started with Android Firebase Messaging. It subscribes to a topic, receives incoming Notifications and displays them. It's basically a simplified version of the [quickstart-android] example.

You can use [Android Studio] to open the project and deploy it on your device. In the application you can click on the "SUBSCRIBE TO NEWS" Button to subscribe on a Topic called "news", that we will send a notification to:

<a href="https://raw.githubusercontent.com/bytefish/FcmSharp/master/FcmSharp/Examples/Images/Screenshot_20181118-103856.png">
    <img src="https://raw.githubusercontent.com/bytefish/FcmSharp/master/FcmSharp/Examples/Images/Screenshot_20181118-103856.png" alt="Messaging App Started Screenshot" width="50%" />
</a>

#### Adding google-services.json to the Project ####

[Firebase Console]: https://console.firebase.google.com

The only thing, that needs to be done on the Android-side is downloading the ``google-services.json`` file and adding it to the project.

You start by opening the [Firebase Console] and going to the **Project Settings** of your project:

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_project_settings.jpg">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_project_settings.jpg" alt="Firebase Project Settings"/>
</a>

Then select the **General** Tab and click on the **google-services.json** download link:

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_download_google_services_json.jpg">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_download_google_services_json.jpg" alt="Projects google-services.json" class="mediacenter" />
</a>

Put it in the ``app`` folder of your project. My ``messaging`` project is located at ``D:\github\quickstart-android\messaging``, 
so the final link will look like this: ``D:\github\quickstart-android\messaging\app\google-services.json``. 

### FcmSharp-side ###

I have added an example project for [FcmSharp] to the GitHub repository at:

* [https://github.com/bytefish/FcmSharp/tree/master/FcmSharp/Examples](https://github.com/bytefish/FcmSharp/tree/master/FcmSharp/Examples)

In the example you only need to set the path to the Service Account Key.

#### Downloading the Service Account Key ####

All messages to the Firebase Cloud Messaging API need to be signed. This requires you to first download the Service Account Key for 
you project. 

Open the **Project Settings** and then go to the **Service Accounts** Tab. On this page click on **Generate New Private Key** for generating 
the required credentials.

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_private_key.jpg">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_private_key.jpg" alt="Private Key for Message Signing" />
</a>

A warning will be shown, which reminds you to store the key securely. This is imporant, so be sure to never leak the Private Key into the public.

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_download_key.JPG">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_download_key.JPG" alt="Download the Private Key" class="mediacenter" />
</a>

I have stored the Private Key to ``D:\serviceAccountKey.json``.

#### Sending a Notification ####

Now [FcmSharp] is used to send the Notification to the "news" topic.

```csharp
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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
                    Body = "Notifcation Body Text"
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
        }
    }
}
```

### Android Result ###

We can see, that a small Firebase Icon pops up in the Action Bar:

<a href="https://github.com/bytefish/FcmSharp/blob/master/FcmSharp/Examples/Images/Screenshot_20181118-100611.png">
    <img src="https://github.com/bytefish/FcmSharp/blob/master/FcmSharp/Examples/Images/Screenshot_20181118-100611.png" alt="Messaging App Started Screenshot" width="50%" />
</a>

And if we swipe down, we can see the Notification Title and Body, that we have sent with [FcmSharp]:

<a href="https://github.com/bytefish/FcmSharp/blob/master/FcmSharp/Examples/Images/Screenshot_20181118-100555.png">
    <img src="https://github.com/bytefish/FcmSharp/blob/master/FcmSharp/Examples/Images/Screenshot_20181118-100555.png" alt="Messaging App Started Screenshot" width="50%" />
</a>

## Advanced ##

###  ###

### Downloading the JSON Service Account Key ###

Go to the Firebase Console and choose your project. Then go to the ``Project Settings`` (by clicking on the Gear Icon next to ``Project Overview``) 
and select the Tab ``Service Accounts``. Then scroll down and select ``Generate Private Key`` to download the JSON file. Please see the complete 
example in this README for a step-by-step guide.

### How to use a Proxy Server ###

[FcmSharp] supports using Proxy Servers. The following test shows how to use a Proxy. It uses the ``ProxyHttpClientFactory`` of FcmSharp for creating a proxied ``FcmClient``.

```csharp
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
```

## How to ...? ##


## Send a Multicast message ##

The following example shows how to send a message to multiple tokens in a batch. You have to use the ``SendMulticastMessage`` 
to send a message to multiple devices as a Multicast message:


```csharp
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
```

### Synchronous API Calls ###

The ``FcmClient`` only provides an asynchronous API, and a synchronous API won't be added. I know that 
asynchronous programming can be very challenging for beginners, so here is how you can turn an async 
call into a synchronous one:

```csharp
var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();
```

## Thanks ##

[OVSoftware]: http://www.ovsoftware.de
[JetBrains]: https://jetbrains.com
[ReSharper]: https://www.jetbrains.com/resharper/

<p>
    <a href="https://ovsoftware.de">
        <img src="https://raw.githubusercontent.com/bytefish/FcmSharp/master/FcmSharp/Images/OV_Logo.png" height="94">
    </a>
    <a href="https://www.jetbrains.com/">
        <img src="https://raw.githubusercontent.com/bytefish/FcmSharp/master/FcmSharp/Images/jetbrains.png" height="94" width="94">
    </a>
</p>

I want to thank all contributors of this project for helping to create a great library, my employer [OVSoftware] for allowing me to work on my side-projects and [JetBrains] for donating me a [ReSharper] license for this project.

