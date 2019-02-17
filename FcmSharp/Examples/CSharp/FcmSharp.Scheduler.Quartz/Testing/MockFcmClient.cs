// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Requests;
using FcmSharp.Responses;
using FcmSharp.Scheduler.Quartz.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FcmSharp.Scheduler.Quartz.Testing
{
    public class MockFcmClient : IFcmClient
    {
        private readonly ILogger<MockFcmClient> logger;

        public MockFcmClient(ILogger<MockFcmClient> logger)
        {
            this.logger = logger;
        }

        public Task<FcmMessageResponse> SendAsync(FcmMessage message, CancellationToken cancellationToken = new CancellationToken())
        {
            if (logger.IsDebugEnabled())
            {
                var messageContent = JsonConvert.SerializeObject(message, Formatting.Indented);

                logger.LogDebug($"Sending Message with Content = {messageContent}");
            }

            return Task.FromResult(new FcmMessageResponse());
        }

        public Task<TopicManagementResponse> SubscribeToTopic(TopicManagementRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(new TopicManagementResponse());
        }

        public Task<TopicManagementResponse> UnsubscribeFromTopic(TopicManagementRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(new TopicManagementResponse());
        }

        public void Dispose()
        {
        }
    }
}
