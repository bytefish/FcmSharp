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
using Google.Apis.Util;

namespace FcmSharp.Http.Client
{
    public class FcmHttpClient : IFcmHttpClient
    {
        private readonly ConfigurableHttpClient client;
        private readonly IFcmClientSettings settings;
        private readonly IJsonSerializer serializer;
        private readonly ServiceAccountCredential credential;

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
            this.credential = CreateServiceAccountCredential(client, settings);

            InitializeExponentialBackOff(client);
        }
        
        public Task<TResponseType> SendAsync<TResponseType>(HttpRequestMessageBuilder builder, CancellationToken cancellationToken)
        {
            return SendAsync<TResponseType>(builder, default(HttpCompletionOption), cancellationToken);
        }

        public async Task<TResponseType> SendAsync<TResponseType>(HttpRequestMessageBuilder builder, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            // Add Authorization Header:
            var accessToken = await CreateAccessTokenAsync(cancellationToken).ConfigureAwait(false);

            builder.AddHeader("Authorization", $"Bearer {accessToken}");

            // Build the Request Message:
            var httpRequestMessage = builder.Build();
            
            // Invoke actions before the Request:
            OnBeforeRequest(httpRequestMessage);

            // Invoke the Request:
            HttpResponseMessage httpResponseMessage = await client
                .SendAsync(httpRequestMessage, completionOption, cancellationToken)
                .ConfigureAwait(false);

            // Invoke actions after the Request:
            OnAfterResponse(httpRequestMessage, httpResponseMessage);

            // Apply the Response Interceptors:
            EvaluateResponse(httpResponseMessage);

            // Now read the Response Content as String:
            string httpResponseContentAsString = await httpResponseMessage.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            // And finally return the Object:
            return serializer.DeserializeObject<TResponseType>(httpResponseContentAsString);
        }

        public Task SendAsync(HttpRequestMessageBuilder builder, CancellationToken cancellationToken)
        {
            return SendAsync(builder, default(HttpCompletionOption), cancellationToken);
        }

        public async Task SendAsync(HttpRequestMessageBuilder builder, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            // Add Authorization Header:
            var accessToken = await CreateAccessTokenAsync(cancellationToken)
                .ConfigureAwait(false);

            builder.AddHeader("Authorization", $"Bearer {accessToken}");

            // Build the Request Message:
            var httpRequestMessage = builder.Build();
            
            // Invoke actions before the Request:
            OnBeforeRequest(httpRequestMessage);

            // Invoke the Request:
            HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage, completionOption, cancellationToken).ConfigureAwait(false);

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

        private ServiceAccountCredential CreateServiceAccountCredential(ConfigurableHttpClient client, IFcmClientSettings settings)
        {
            var serviceAccountCredential = GoogleCredential.FromJson(settings.Credentials)
                // We need the Messaging Scope:
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
                // Cast to the ServiceAccountCredential:
                .UnderlyingCredential as ServiceAccountCredential;

            if (serviceAccountCredential == null)
            {
                throw new Exception($"Error creating ServiceAccountCredential from JSON File {settings.Credentials}");
            }

            serviceAccountCredential.Initialize(client);
            
            return serviceAccountCredential;
        }

        private void InitializeExponentialBackOff(ConfigurableHttpClient client)
        {
            // The Maximum Number of Retries is limited to 3 per default for a ConfigurableHttpClient. This is 
            // somewhat weird, because the ExponentialBackOff Algorithm is initialized with 10 Retries per default.
            // 
            // Somehow the NumTries seems to be the limiting factor here, so it basically overrides anything you 
            // are going to write in the Exponential Backoff Handler.
            client.MessageHandler.NumTries = 20;

            // Create the Default BackOff Algorithm:
            var backoff = new ExponentialBackOff();

            // Create the Initializer. Make sure to set the Maximum Timespan between two Requests. It 
            // is 16 Seconds per Default:
            var backoffInitializer = new BackOffHandler.Initializer(backoff)
            {
                MaxTimeSpan = TimeSpan.FromDays(1),
            };

            // Now create the Handler:
            var initializer = new ExponentialBackOffInitializer(ExponentialBackOffPolicy.UnsuccessfulResponse503, () => new BackOffHandler(backoffInitializer));

            // And finally append the BackOff Handler, which reacts to 503 Requests:
            initializer.Initialize(client);
        }

        private async Task<string> CreateAccessTokenAsync(CancellationToken cancellationToken)
        {
            // Execute the Request:
            var accessToken = await credential
                .GetAccessTokenForRequestAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (accessToken == null)
            {
                throw new Exception("Failed to obtain Access Token for Request");
            }

            return accessToken;
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}