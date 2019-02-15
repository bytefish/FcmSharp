using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Requests;
using FcmSharp.Responses;
using Newtonsoft.Json;

namespace FcmSharp.Scheduler.Testing
{
    public class MockFcmClient : IFcmClient
    {
        public Task<FcmMessageResponse> SendAsync(FcmMessage message, CancellationToken cancellationToken = new CancellationToken())
        {
            var content = JsonConvert.SerializeObject(message, Formatting.Indented);

            Console.WriteLine($"[{DateTime.Now}] [SendAsync] {content} ...");

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
