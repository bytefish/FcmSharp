// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Http;
using FcmSharp.Requests;
using FcmSharp.Requests.DeviceGroup;
using FcmSharp.Requests.Topics;
using FcmSharp.Responses;
using FcmSharp.Settings;

namespace FcmSharp
{
    public class FcmClient : IFcmClient
    {
        private readonly IFcmClientSettings settings;
        private readonly IFcmHttpClient httpClient;

        public FcmClient(IFcmClientSettings settings)
            : this(settings, new FcmHttpClient(settings))
        {
        }

        public FcmClient(IFcmClientSettings settings, IFcmHttpClient httpClient)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (httpClient == null)
            {
                throw new ArgumentNullException("httpClient");
            }

            this.settings = settings;
            this.httpClient = httpClient;
        }

        public Task<FcmMessageResponse> SendAsync(FcmMulticastMessage message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync<FcmMulticastMessage, FcmMessageResponse>(message, cancellationToken);
        }

        public Task<FcmMessageResponse> SendAsync<TPayload>(FcmMulticastMessage<TPayload> message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync<FcmMulticastMessage, FcmMessageResponse>(message, cancellationToken);
        }

        public Task<FcmMessageResponse> SendAsync(FcmUnicastMessage message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync<FcmUnicastMessage, FcmMessageResponse>(message, cancellationToken);
        }

        public Task<FcmMessageResponse> SendAsync<TPayload>(FcmUnicastMessage<TPayload> message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync<FcmUnicastMessage<TPayload>, FcmMessageResponse>(message, cancellationToken);
        }

        public Task<CreateDeviceGroupMessageResponse> SendAsync(CreateDeviceGroupMessage message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync<CreateDeviceGroupMessage, CreateDeviceGroupMessageResponse>(message, cancellationToken);
        }

        public Task SendAsync(RemoveDeviceGroupMessage message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync(message, cancellationToken);
        }

        public Task SendAsync(AddDeviceGroupMessage message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync(message, cancellationToken);
        }

        public Task<TopicMessageResponse> SendAsync(TopicUnicastMessage message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync<TopicUnicastMessage, TopicMessageResponse>(message, cancellationToken);
        }

        public Task<TopicMessageResponse> SendAsync<TPayload>(TopicUnicastMessage<TPayload> message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync<TopicUnicastMessage<TPayload>, TopicMessageResponse>(message, cancellationToken);
        }

        public Task<TopicMessageResponse> SendAsync(TopicMulticastMessage message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync<TopicMulticastMessage, TopicMessageResponse>(message, cancellationToken);
        }

        public Task<TopicMessageResponse> SendAsync<TPayload>(TopicMulticastMessage<TPayload> message, CancellationToken cancellationToken)
        {
            return httpClient.PostAsync<TopicMulticastMessage<TPayload>, TopicMessageResponse>(message, cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Make sure we Dispose the HttpClient, when we finish:
            if (disposing)
            {
                if (httpClient != null)
                {
                    httpClient.Dispose();
                }
            }
        }
    }
}