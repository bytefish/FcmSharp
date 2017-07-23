// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FcmSharp.Http.Retry;

namespace FcmSharp.Exceptions
{
    public class FcmRetryAfterException : FcmException
    {
        public readonly RetryConditionValue RetryConditionValue;

        public FcmRetryAfterException(RetryConditionValue retryConditionValue)
        {
            RetryConditionValue = retryConditionValue;
        }

        public FcmRetryAfterException(string message, RetryConditionValue retryConditionValue) : base(message)
        {
            RetryConditionValue = retryConditionValue;
        }

        public FcmRetryAfterException(string message, Exception innerException, RetryConditionValue retryConditionValue) : base(message, innerException)
        {
            RetryConditionValue = retryConditionValue;
        }
    }
}
