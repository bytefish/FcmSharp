# FcmSharp #

[FcmSharp]: https://github.com/bytefish/FcmSharp
[Firebase Cloud Messaging HTTP Protocol]: https://firebase.google.com/docs/cloud-messaging/http-server-ref

[FcmSharp] is a .NET library for the Firebase Cloud Messaging (FCM) API. 

It implements the Firebase Cloud Messaging HTTP v1 API:

* https://firebase.google.com/docs/reference/fcm/rest/v1/projects.messages

[FcmSharp] supports .NET Core as of Version 1.0.0.

## Installing FcmSharp ##

You can use [NuGet](https://www.nuget.org) to install [FcmSharp]. Run the following command 
in the [Package Manager Console](http://docs.nuget.org/consume/package-manager-console).

```
PM> Install-Package FcmSharp
```

## Downloading the JSON Service Account Key ##

Go to the Firebase Console and choose your project. Then go to the ``Project Settings`` (by clicking on the Gear Icon next to ``Project Overview``) 
and select the Tab ``Service Accounts``. Then scroll down and select ``Generate Private Key`` to download the JSON file. Please see the complete 
example in this README for a step-by-step guide.

## Quickstart ##

The Quickstart shows you how to work with [FcmSharp] in C#.

```csharp
// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using FcmSharp.Requests;
using FcmSharp.Settings;

namespace FcmSharp.Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Read the Service Account Key from a File, which is not under Version Control:
            var settings = FileBasedFcmClientSettings.CreateFromFile("your_app", @"D:\serviceAccountKey.json");

            // Construct the Client:
            using (var client = new FcmClient(settings))
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

                // Print the Result to the Console:
                System.Console.WriteLine("Message ID = {0}", result.Name);

                System.Console.ReadLine();
            }
        }
    }
}
```


## Getting Started: Complete Example ##

In this tutorial I assume you have followed the official documentation to create a Firebase project:

* [https://firebase.google.com/docs/](https://firebase.google.com/docs/)

## Android-side  ##

[quickstart-android]: https://github.com/firebase/quickstart-android
[messaging]: https://github.com/firebase/quickstart-android/tree/master/messaging/

The Firebase repositories on GitHub provide great quickstart examples for almost all use cases. For this tutorial 
we are going to use their Firebase Cloud Messaging Quickstart example, which is located at:

* [https://github.com/firebase/quickstart-android/tree/master/messaging/](https://github.com/firebase/quickstart-android/tree/master/messaging/)

I simply cloned the entire [quickstart-android] repository and opened the [messaging] project.

### Adding google-services.json to the Project ###

[Firebase Console]: https://console.firebase.google.com

The only step, that's left to be done on the Android-side is to download the ``google-services.json`` file and add it to the project.

You start by opening the [Firebase Console] and going to the **Project Settings** of your project:

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_project_settings.jpg">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_project_settings.jpg" alt="Firebase Project Settings" class="mediacenter" />
</a>

Then select the **General** Tab and click on the **google-services.json** download link:

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_download_google_services_json.jpg">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_download_google_services_json.jpg" alt="Projects google-services.json" class="mediacenter" />
</a>

And put it in the ``app`` folder of your project. My ``messaging`` project is located at ``D:\github\quickstart-android\messaging``, 
so the final link will look like this: ``D:\github\quickstart-android\messaging\app\google-services.json``. 

### Additional Android Resources ###

If you still have problems adding Firebase to your Android application, then please consult the official Firebase documentation at:

* [https://firebase.google.com/docs/android/setup](https://firebase.google.com/docs/android/setup)

## FcmSharp-side ##

### Downloading the Service Account Key ###

All messages to the Firebase Cloud Messaging API need to be signed. This requires you to first download the Service Account Key for 
you project. 

Open the **Project Settings** and then go to the **Service Accounts** Tab. On this page click on **Generate New Private Key** for generating 
the required credentials.

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_private_key.jpg">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_private_key.jpg" alt="Private Key for Message Signing" class="mediacenter" />
</a>

A warning will be shown, which reminds you to store the key securely. This is imporant, so be sure to never leak the Private Key into the public.

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_download_key.JPG">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/firebase_console_download_key.JPG" alt="Download the Private Key" class="mediacenter" />
</a>

I have stored the Private Key to ``D:\serviceAccountKey.json``.

### Preparing the FcmSharp.Example project ###

I have added an example project for [FcmSharp] to its GitHub repository at:

* [https://github.com/bytefish/FcmSharp/tree/master/FcmSharp/Examples](https://github.com/bytefish/FcmSharp/tree/master/FcmSharp/Examples)

In the example you need to set your Projects ID, when creating the FcmSharp Settings. To find out your Project ID, in the [Firebase Console] 
select the **Project Settings** and copy the Project ID in the **General** Tab. Then in the sample replace ``your_project_id`` with your 
Firebase Project ID.

```csharp
// Read the Credentials from a File, which is not under Version Control:
var settings = FileBasedFcmClientSettings.CreateFromFile(@"your_project_id", @"D:\serviceAccountKey.json");
```

We are done with the FcmSharp-side!

## Sending the Message ##

### Getting the Device InstanceID Token ###

In the ``MainActivity.java`` of the Android project I set a breakpoint, where the Instance ID Token is obtained. You can also do it 
without a breakpoint and search the Logs for the InstanceID message:

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/android_instance_id_token.jpg">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/android_instance_id_token.jpg" alt="Getting the Instance ID Token" class="mediacenter" />
</a>

Once you click on the **LOG TOKEN** Button in the sample application, the breakpoint will be hit and you can easily 
copy the ``token`` from the Variables pane.

### Using FcmSharp to send a message to the Device ###

Now start the ``FcmSharp.Example`` project. A Console will open and prompt you to enter the Device Token. It is the 
Device Token we have just obtained from the Android application:

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/fcmsharp_example_console.jpg">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/fcmsharp_example_console.jpg" alt="Sending the message with FcmSharp" class="mediacenter" />
</a>

Before hitting Enter, make sure to set a Breakpoint in the ``onMessageReceived`` handler of the ``MyFirebaseMessagingService.java`` class.

### Receiving the Message in the Android Application ###

So after you have set the Breakpoint in the ``MyFirebaseMessagingService.java``, hit enter in the ``FcmSharp.Example`` console  and you should 
receive the message in your Android application:

<a href="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/android_remote_message_received.jpg">
    <img src="https://github.com/bytefish/bytefish.de/raw/master/images/blog/fcmsharp_getting_started/android_remote_message_received.jpg" alt="Receiving the Message in Android" class="mediacenter" />
</a>

## How to do Synchronous API Calls ##

The ``FcmClient`` only provides an asynchronous API, and a synchronous API won't be added. I know that 
asynchronous programming can be very challenging for beginners, so here is how you can turn an async 
call into a synchronous one:

```csharp
var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();
```
