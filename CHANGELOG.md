# CHANGELOG #

## 2.4.0 ##

* The ``ServiceAccountCredential`` is now cached, see [Issue #28](https://github.com/bytefish/FcmSharp/issues/28). JWT Tokens are now only refreshed, when they are close to expiration.
* The class ``StreamBasedFcmClientSettings`` has been added by [@janniksam](https://github.com/janniksam).

## 2.5.0 ##

This release added an Exponential BackOff to handle ``503`` HTTP Status Codes, which can occur on high message frequency as mentioned in [Issue #18](https://github.com/bytefish/FcmSharp/issues/31). Thanks to [@Kilowhisky](https://github.com/Kilowhisky) for the bug report and useful informations.

The ``IFcmClientSettings`` now contain an additional ``ExponentialBackOffSettings`` property, which can be used to configure the Exponential BackOff parameters:

```csharp
public FcmClientSettings(string project, string credentials, ExponentialBackOffSettings exportExponentialBackOffSettings)
```

You can use an additional Factory method in the ``FileBasedFcmClientSettings`` to pass the Exponential BackOff parameters into the settings:

```csharp
public static FcmClientSettings CreateFromFile(string project, string credentialsFileName, ExponentialBackOffSettings exponentialBackOffSettings)
```

Please note, that you also need to adjust the ``CancellationToken`` passed into the ``SendAsync`` method, which also has a default Cancellation TimeSpan, that needs to be adjusted. In order to have a request being retried for 5 Minutes for example, you need to create a ``CancellationToken`` like this:

```
CancellationToken longLivingCancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(5)).Token;
```