// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using FcmSharp.Responses;

namespace FcmSharp.Exceptions
{
    public class FcmTopicManagementException : Exception
    {
        public readonly TopicMessageResponseError Error;
        
        public FcmTopicManagementException(TopicMessageResponseError error, string message) 
            : base(message)
        {
            Error = error;
        }
    }
}
