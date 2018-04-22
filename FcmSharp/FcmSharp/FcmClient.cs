// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Exceptions;
using FcmSharp.Http;
using FcmSharp.Http.Builder;
using FcmSharp.Http.Client;
using FcmSharp.Http.Constants;
using FcmSharp.Requests;
using FcmSharp.Responses;
using FcmSharp.Serializer;
using FcmSharp.Settings;
using Google.Apis.Auth.OAuth2;

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

        public async Task<FcmMessageResponse> SendAsync(FcmMessage message, CancellationToken cancellationToken)
        {
            // Construct the HTTP Message:
            HttpRequestMessageBuilder httpRequestMessageBuilder = new HttpRequestMessageBuilder(settings.FcmUrl, HttpMethod.Post)
                .SetStringContent(serializer.SerializeObject(message), Encoding.UTF8, MediaTypeNames.ApplicationJson);

            try
            {
                return await httpClient.SendAsync<FcmMessageResponse>(httpRequestMessageBuilder, cancellationToken);
            }
            catch (FcmHttpException exception)
            {
                // Get the Original HTTP Response:
                var response = exception.HttpResponseMessage;

                // Read the Content:
                var content = await response.Content.ReadAsStringAsync();

                // Parse the Error:
                var error = serializer.DeserializeObject<FcmMessageErrorResponse>(content);

                // Throw the Exception:
                throw new FcmMessageException(error);
            }
        }

        public async Task<TopicManagementResponse> SendAsync(TopicManagementRequest request, CancellationToken cancellationToken)
        {

            // Construct the HTTP Message:
            HttpRequestMessageBuilder httpRequestMessageBuilder = new HttpRequestMessageBuilder(settings.IidHost, HttpMethod.Post)
                // Add the Serialized Request Message:
                .SetStringContent(serializer.SerializeObject(request), Encoding.UTF8, MediaTypeNames.ApplicationJson);

            try
            {
                return await httpClient.SendAsync<TopicManagementResponse>(httpRequestMessageBuilder, cancellationToken);
            }
            catch (FcmHttpException exception)
            {
                // Get the Original HTTP Response:
                var response = exception.HttpResponseMessage;

                // Read the Content:
                var content = await response.Content.ReadAsStringAsync();

                // Parse the Error:
                var error = serializer.DeserializeObject<FcmMessageErrorResponse>(content);

                // Throw the Exception:
                throw new FcmMessageException(error);
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