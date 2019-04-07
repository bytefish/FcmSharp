// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Requests;
using FcmSharp.Responses;

namespace FcmSharp
{
    public interface IFcmClient : IDisposable
    {
        Task<FcmMessageResponse> SendAsync(FcmMessage message, CancellationToken cancellationToken = default(CancellationToken));

        Task<FcmBatchResponse> SendBatchAsync(Message[] messages, bool dryRun = false, CancellationToken cancellationToken = default(CancellationToken));

        Task<TopicManagementResponse> SubscribeToTopic(TopicManagementRequest request, CancellationToken cancellationToken = default(CancellationToken));

        Task<TopicManagementResponse> UnsubscribeFromTopic(TopicManagementRequest request, CancellationToken cancellationToken = default(CancellationToken));
    }
}