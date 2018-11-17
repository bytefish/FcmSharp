// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Exceptions;
using FcmSharp.Http.Builder;
using FcmSharp.Http.Client;
using FcmSharp.Http.Constants;
using FcmSharp.Requests;
using FcmSharp.Responses;
using FcmSharp.Serializer;
using FcmSharp.Settings;

namespace FcmSharp
{
    public class FcmClient : IFcmClient
    {
        private readonly IFcmClientSettings settings;

        private readonly IJsonSerializer serializer;

        private readonly IFcmHttpClient httpClient;
        
        public FcmClient(IFcmClientSettings settings)
            : this(settings, JsonSerializer.Default)
        {
        }

        public FcmClient(IFcmClientSettings settings, IFcmHttpClient fcmHttpClient)
            : this(settings, JsonSerializer.Default, fcmHttpClient)
        {
        }


        public FcmClient(IFcmClientSettings settings, IJsonSerializer serializer)
            : this(settings, serializer, new FcmHttpClient(settings))
        {
        }


        public FcmClient(IFcmClientSettings settings, IJsonSerializer serializer, IFcmHttpClient httpClient)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (httpClient == null)
            {
                throw new ArgumentNullException("httpClient");
            }

            this.serializer = serializer;
            this.settings = settings;
            this.httpClient = httpClient;
        }

        public async Task<FcmMessageResponse> SendAsync(FcmMessage message, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            string url = $"https://fcm.googleapis.com/v1/projects/{settings.Project}/messages:send";

            // Construct the HTTP Message:
            HttpRequestMessageBuilder httpRequestMessageBuilder = new HttpRequestMessageBuilder(url, HttpMethod.Post)
                .SetStringContent(serializer.SerializeObject(message), Encoding.UTF8, MediaTypeNames.ApplicationJson);

            try
            {
                return await httpClient
                    .SendAsync<FcmMessageResponse>(httpRequestMessageBuilder, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (FcmHttpException exception)
            {
                // Get the Original HTTP Response:
                var response = exception.HttpResponseMessage;

                // Read the Content:
                var content = await response.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

                // Parse the Error:
                var error = serializer.DeserializeObject<FcmMessageErrorResponse>(content);

                // Throw the Exception:
                throw new FcmMessageException(error, content);
            }
        }

        public Task<TopicManagementResponse> SubscribeToTopic(TopicManagementRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            string iidSubscribePath = "iid/v1:batchAdd";

            return SendAsync(iidSubscribePath, request, cancellationToken);
        }

        public Task<TopicManagementResponse> UnsubscribeFromTopic(TopicManagementRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            string iidUnsubscribePath = "iid/v1:batchRemove";

            return SendAsync(iidUnsubscribePath, request, cancellationToken);
        }
        
        private async Task<TopicManagementResponse> SendAsync(string path, TopicManagementRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // Build the URL:
            string url = $"https://iid.googleapis.com/{path}";

            // Construct the HTTP Message:
            HttpRequestMessageBuilder httpRequestMessageBuilder = new HttpRequestMessageBuilder(url, HttpMethod.Post)
                // Add Option to use the Access Token Auth Header:
                .AddHeader("access_token_auth", "true")
                // Add the Serialized Request Message:
                .SetStringContent(serializer.SerializeObject(request), Encoding.UTF8, MediaTypeNames.ApplicationJson);

            try
            {
                return await httpClient
                    .SendAsync<TopicManagementResponse>(httpRequestMessageBuilder, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (FcmHttpException exception)
            {
                // Get the Original HTTP Response:
                var response = exception.HttpResponseMessage;

                // Read the Content:
                var content = await response.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

                // Parse the Error:
                var error = serializer.DeserializeObject<TopicMessageResponseError>(content);

                // Throw the Exception:
                throw new FcmTopicManagementException(error, content);
            }
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
                httpClient?.Dispose();
            }
        }
    }
}