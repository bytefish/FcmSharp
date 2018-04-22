// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using FcmSharp.Exceptions;
using FcmSharp.Http.Builder;
using FcmSharp.Serializer;
using FcmSharp.Settings;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;

namespace FcmSharp.Http.Client
{
    public class FcmHttpClient : IFcmHttpClient
    {
        private readonly ConfigurableHttpClient client;
        private readonly IFcmClientSettings settings;
        private readonly IJsonSerializer serializer;

        public FcmHttpClient(IFcmClientSettings settings)
            : this(settings, new ConfigurableHttpClient(new ConfigurableMessageHandler(new HttpClientHandler())), JsonSerializer.Default)
        {
        }

        public FcmHttpClient(IFcmClientSettings settings, ConfigurableHttpClient client, IJsonSerializer serializer)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if(serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }

            this.settings = settings;
            this.client = client;
            this.serializer = serializer;
        }


        public Task<TResponseType> SendAsync<TResponseType>(HttpRequestMessageBuilder builder, CancellationToken cancellationToken)
        {
            return SendAsync<TResponseType>(builder, default(HttpCompletionOption), cancellationToken);
        }

        public async Task<TResponseType> SendAsync<TResponseType>(HttpRequestMessageBuilder builder, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            // Build the Request Message:
            var httpRequestMessage = builder.Build();

            // Add Authorization Header:
            var accessToken = await CreateAccessTokenAsync(cancellationToken);

            builder.AddHeader("Authorization", $"Bearer {accessToken}");

            // Invoke actions before the Request:
            OnBeforeRequest(httpRequestMessage);

            // Invoke the Request:
            HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage, completionOption, cancellationToken);

            // Invoke actions after the Request:
            OnAfterResponse(httpRequestMessage, httpResponseMessage);

            // Apply the Response Interceptors:
            EvaluateResponse(httpResponseMessage);

            // Now read the Response Content as String:
            string httpResponseContentAsString = await httpResponseMessage.Content.ReadAsStringAsync();

            // And finally return the Object:
            return serializer.DeserializeObject<TResponseType>(httpResponseContentAsString);
        }

        public Task SendAsync(HttpRequestMessageBuilder builder, CancellationToken cancellationToken)
        {
            return SendAsync(builder, default(HttpCompletionOption), cancellationToken);
        }

        public async Task SendAsync(HttpRequestMessageBuilder builder, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            // Build the Request Message:
            var httpRequestMessage = builder.Build();

            // Add Authorization Header:
            var accessToken = await CreateAccessTokenAsync(cancellationToken);

            builder.AddHeader("Authorization", $"Bearer {accessToken}");

            // Invoke actions before the Request:
            OnBeforeRequest(httpRequestMessage);

            // Invoke the Request:
            HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage, completionOption, cancellationToken);

            // Invoke actions after the Request:
            OnAfterResponse(httpRequestMessage, httpResponseMessage);

            // Apply the Response Interceptors:
            EvaluateResponse(httpResponseMessage);
        }

        protected virtual void OnBeforeRequest(HttpRequestMessage httpRequestMessage)
        {
        
        }

        protected virtual void OnAfterResponse(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage)
        {
        }

        private async Task<string> CreateAccessTokenAsync(CancellationToken cancellationToken)
        {
            
            var credential = GoogleCredential.FromJson(settings.Credentials)    
                // We need the Messaging Scope:
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
                // Cast to the ServiceAccountCredential:
                .UnderlyingCredential as ServiceAccountCredential;
            
            if (credential == null)
            {
                throw new Exception("Error creating Access Token for Authorizing Request");
            }

            // Initialize with the Configurable Client:
            credential.Initialize(client);

            // Execute the Request:
            var accessToken = await credential.GetAccessTokenForRequestAsync(cancellationToken: cancellationToken);

            if (accessToken == null)
            {
                throw new Exception("Empty Access Token for Authorizing Request");
            }

            return accessToken;
        }

        public void EvaluateResponse(HttpResponseMessage response)
        {
            if (response == null)
            {
                return;
            }

            HttpStatusCode httpStatusCode = response.StatusCode;

            if (httpStatusCode == HttpStatusCode.OK)
            {
                return;
            }

            if ((int) httpStatusCode >= 400)
            {
                throw new FcmHttpException(response);
            }
        }
    }
}