# CHANGELOG #

## 2.7.1 ##

* Add the ``PERMISSION_DENIED`` Error Code. Thanks to [@popelenkow](https://github.com/popelenkow) for the Pull Request.

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
