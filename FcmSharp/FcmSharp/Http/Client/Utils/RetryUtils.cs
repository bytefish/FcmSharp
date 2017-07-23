// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Http.Retry;
using System.Net.Http;

namespace FcmSharp.Http.Utils
{
    public static class RetryUtils
    {
        public static bool TryDetermineRetryDelay(HttpResponseMessage httpResponseMessage, out RetryConditionValue result)
        {
            result = null;

            if (httpResponseMessage.Headers.RetryAfter == null)
            {
                return false;
            }

            var retryAfterConditionHeaderValue = httpResponseMessage.Headers.RetryAfter;

            if (retryAfterConditionHeaderValue.Date.HasValue)
            {
                result = new RetryConditionValue(retryAfterConditionHeaderValue.Date.Value);

                return true;
            }

            if (retryAfterConditionHeaderValue.Delta.HasValue)
            {
                result = new RetryConditionValue(retryAfterConditionHeaderValue.Delta.Value);

                return true;
            }

            return false;
        }
    }
}
