// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Requests;
using FcmSharp.Requests.DeviceGroup;
using FcmSharp.Requests.Topics;
using FcmSharp.Responses;

namespace FcmSharp
{
    public interface IFcmClient : IDisposable
    {
        Task<FcmMessageResponse> SendAsync(FcmMulticastMessage message, CancellationToken cancellationToken);

        Task<FcmMessageResponse> SendAsync<TPayload>(FcmMulticastMessage<TPayload> message, CancellationToken cancellationToken);

        Task<FcmMessageResponse> SendAsync(FcmUnicastMessage message, CancellationToken cancellationToken);

        Task<FcmMessageResponse> SendAsync<TPayload>(FcmUnicastMessage<TPayload> message, CancellationToken cancellationToken);

        Task<CreateDeviceGroupMessageResponse> SendAsync(CreateDeviceGroupMessage message, CancellationToken cancellationToken);

        Task SendAsync(RemoveDeviceGroupMessage message, CancellationToken cancellationToken);

        Task SendAsync(AddDeviceGroupMessage message, CancellationToken cancellationToken);

        Task<TopicMessageResponse> SendAsync(TopicUnicastMessage message, CancellationToken cancellationToken);

        Task<TopicMessageResponse> SendAsync<TPayload>(TopicUnicastMessage<TPayload> message, CancellationToken cancellationToken);

        Task<TopicMessageResponse> SendAsync(TopicMulticastMessage message, CancellationToken cancellationToken);

        Task<TopicMessageResponse> SendAsync<TPayload>(TopicMulticastMessage<TPayload> message, CancellationToken cancellationToken);
    }
}