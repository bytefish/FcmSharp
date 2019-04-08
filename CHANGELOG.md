# CHANGELOG #

## 3.0.1 ##

* Added an extension method ``SendMulticastMessage`` to make sending Multicast messages easier:

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

## 3.0.0 ##

* Fixed [Issue #59](https://github.com/bytefish/FcmSharp/issues/59) by adding a method for sending messages using the FCM Batch endpoint. 

The new method in the``FcmClient`` is: 

* ``Task<FcmBatchResponse> SendBatchAsync(Message[] messages, bool dryRun = false, CancellationToken cancellationToken = default(CancellationToken))``

The method makes it possible to send up to 1000 messages (hard limit for FCM) in a single request:

```csharp
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
```

Thanks to [@ianmark89](https://github.com/ianmark89) for raising the Issue!

Additional Fixes:

* Fixed [Issue #47](https://github.com/bytefish/FcmSharp/issues/47) using ``IDictionary`` for Requests. Thanks to [@paulhickman-a365](https://github.com/paulhickman-a365) for raising the issue!

## 2.8.4 ##

* Merged [Pull Request #53](https://github.com/bytefish/FcmSharp/pull/53), which fixed the deserialization in the ``BoolToIntConverter``. Thanks to [@vivan000](https://github.com/vivan000) for the Pull Request!

## 2.8.3 ##

* The ``fcm_options`` property has been added to the ``WebpushConfig`` in [Pull Request #51](https://github.com/bytefish/FcmSharp/pull/51). Thanks to [@Dinokin](https://github.com/Dinokin) for the Pull Request!

## 2.8.2 ##

* Added the ``channel_id`` property to the ``AndroidNotification``. Thanks to [@kehollin](https://github.com/kehollin) for bringing up the issue!

## 2.8.1 ##

* Added a Method ``FcmClientSettings CreateFromFile(string credentialsFileName)`` to the ``FileBasedFcmClientSettings`` class. This method automatically reads the Project ID from the Service Account Key, making it unecessary to explicitly pass it in code.

## 2.8.0 ##

[Issue #44](https://github.com/bytefish/FcmSharp/issues/44) fixed the Proxy usage in FcmSharp. Thanks a lot to [@kdlslyv](https://github.com/kdlslyv) for bringing up the issue! The Google API previously did not use the Proxy Settings provided by the user for the Token Refreshs. I have added a ``ProxyHttpClientFactory`` to the library to simplify the creation of proxied ``HttpClient`` instances.

An Integration Test with Fiddler was used to verify the functionality:

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

## 2.7.1 ##

* Added the ``PERMISSION_DENIED`` Error Code. Thanks to [@popelenkow](https://github.com/popelenkow) for the Pull Request.

## 2.7.0 ##

* Starting to Strong-Naming assemblies to make it easier for projects to consume the library, where Strong-Naming is a requirement. Thanks to [@DanAvni](https://github.com/DanAvni) for bringing up this question. I decided to go strong-named now, but if this leads to many bugs in other projects, then please let me know. 
* Like suggested in https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/strong-name-signing.md I am commiting the Private Signing Key to the repository:

> Firstly, we would recommend that these open source projects check-in their private key (remember, strong-names are used for identity, and not for security). 

## 2.6.0 ##

* [Issue #36](https://github.com/bytefish/FcmSharp/issues/36) fixes a bug, where the APNS Payload without a badge always resets the badge to 0. Much thanks to [@sir-boformer](https://github.com/sir-boformer) for the Bugfix!

## 2.5.0 ##

This release added an Exponential BackOff to handle ``503`` HTTP Status Codes, which can occur on high message frequency as mentioned in [Issue #31](https://github.com/bytefish/FcmSharp/issues/31). Thanks to [@Kilowhisky](https://github.com/Kilowhisky) for the bug report and useful informations.

There has been one breaking change in the ``IFcmClientSettings`` interface: The ``IFcmClientSettings`` now contain an additional ``ExponentialBackOffSettings`` property, which can be used to configure the Exponential BackOff parameters:

```csharp
public interface IFcmClientSettings
{
    string Project { get; }
    
    string Credentials { get; }

    ExponentialBackOffSettings ExponentialBackOffSettings { get; }
}
```

I didn't find a sane way to make sure ``ExponentialBackOffSettings`` are passed into the ``FcmHttpClient``, without having an additional constructor parameter in the ``FcmHttpClient`` making it hard to configure it from the outside. If you are implementing the ``IFcmClientSettings`` in your system, please make sure to instantiate the class with the default ``ExponentialBackOffSettings`` (can be obtained from ``ExponentialBackOffSettings.Default``).

That means the ``FcmClientSettings`` class contains 2 constructors (with default ``ExponentialBackOffSettings`` and with explicitly passed ``ExponentialBackOffSettings``):

```csharp
public FcmClientSettings(string project, string credentials, ExponentialBackOffSettings exportExponentialBackOffSettings)
```

You can use an additional Factory method in the ``FileBasedFcmClientSettings`` to pass the Exponential BackOff parameters into the settings. The existing Factory methods create ``IFcmClientSettings`` with default BackOff parameters:

```csharp
public static FcmClientSettings CreateFromFile(string project, string credentialsFileName, ExponentialBackOffSettings exponentialBackOffSettings)
```

Please note, that you also need to adjust the ``CancellationToken`` passed into the ``SendAsync`` method, if you need very long retry timespans. A ``CancellationToken`` also has a default Time Out that needs to be adjusted. In order to have a request being retried for 5 Minutes for example, you need to create a ``CancellationToken`` like this:

```
CancellationToken longLivingCancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(5)).Token;
```

## 2.4.0 ##

* The ``ServiceAccountCredential`` is now cached, see [Issue #28](https://github.com/bytefish/FcmSharp/issues/28). JWT Tokens are now only refreshed, when they are close to expiration.
* The class ``StreamBasedFcmClientSettings`` has been added by [@janniksam](https://github.com/janniksam).
