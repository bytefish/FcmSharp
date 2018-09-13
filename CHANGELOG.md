# CHANGELOG #

## 2.4.0 ##

* The ``ServiceAccountCredential`` is now cached, see [Issue #28](https://github.com/bytefish/FcmSharp/issues/28). JWT Tokens are now only refreshed, when they are close to expiration.
* The class ``StreamBasedFcmClientSettings`` has been added by [@janniksam](https://github.com/janniksam).
