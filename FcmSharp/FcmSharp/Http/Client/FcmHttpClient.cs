// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;
using System.Text;
using FcmSharp.Batch;
using FcmSharp.Exceptions;
using FcmSharp.Http.Builder;
using FcmSharp.Http.Constants;
using FcmSharp.Responses;
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
            : this(settings, JsonSerializer.Default, new Google.Apis.Http.HttpClientFactory(), CreateDefaultHttpClientArgs(settings))
        {
        }

        public FcmHttpClient(IFcmClientSettings settings, Google.Apis.Http.IHttpClientFactory httpClientFactory)
            : this(settings, JsonSerializer.Default, httpClientFactory, CreateDefaultHttpClientArgs(settings))
        {
        }

        public FcmHttpClient(IFcmClientSettings settings, Google.Apis.Http.IHttpClientFactory httpClientFactory, CreateHttpClientArgs httpClientArgs)
            : this(settings, JsonSerializer.Default, httpClientFactory, httpClientArgs)
        {
        }

        public FcmHttpClient(IFcmClientSettings settings, IJsonSerializer serializer, Google.Apis.Http.IHttpClientFactory httpClientFactory, CreateHttpClientArgs httpClientArgs)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            this.settings = settings;
            this.client = httpClientFactory.CreateHttpClient(httpClientArgs);
            this.serializer = serializer;
            this.credential = CreateServiceAccountCredential(httpClientFactory, settings);
            
            InitializeExponentialBackOff(client, settings);
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

        public async Task<TResponseType[]> SendBatchAsync<TResponseType>(HttpRequestMessageBuilder builder, CancellationToken cancellationToken)
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
                .SendAsync(httpRequestMessage, cancellationToken)
                .ConfigureAwait(false);

            // Invoke actions after the Request:
            OnAfterResponse(httpRequestMessage, httpResponseMessage);

            // Apply the Response Interceptors:
            EvaluateResponse(httpResponseMessage);

            var multipart = await httpResponseMessage.Content.ReadAsMultipartAsync(cancellationToken);

            List<TResponseType> result = new List<TResponseType>();

            foreach (var content in multipart.Contents)
            {
                string part = await content.ReadAsStringAsync();

                // This is quite a hack approach, which might or might not work for all scenarios.
                // I am splitting the multipart response into lines, which in turn is skipped until
                // we hit a line with a single "{", which indicates we have found some JSON:
                IEnumerable<string> jsonLines = part.Split('\n').SkipWhile(x => !string.Equals(x.Trim(), "{"));

                // Then we turn the lines into a String again:
                var jsonString = string.Join("\n", jsonLines);

                // So Newtonsoft.JSON can deserialize it again:
                var response = serializer.DeserializeObject<TResponseType>(jsonString);

                // And add it to the result:
                result.Add(response);
            }
            
            // And finally return the Object:
            return result.ToArray();
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

        private ServiceAccountCredential CreateServiceAccountCredential(IHttpClientFactory httpClientFactory, IFcmClientSettings settings)
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

            var initializer = new ServiceAccountCredential.Initializer(serviceAccountCredential.Id, serviceAccountCredential.TokenServerUrl)
            {
                User = serviceAccountCredential.User,
                AccessMethod = serviceAccountCredential.AccessMethod,
                Clock = serviceAccountCredential.Clock,
                Key = serviceAccountCredential.Key,
                Scopes = serviceAccountCredential.Scopes,
                HttpClientFactory = httpClientFactory
            };

            return new ServiceAccountCredential(initializer);
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

        private static CreateHttpClientArgs CreateDefaultHttpClientArgs(IFcmClientSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings", "Settings are needed to create the Default HttpClientArgs");
            }

            var args = new CreateHttpClientArgs();

            // Create the Default BackOff Algorithm:
            var backoff = new ExponentialBackOff(settings.ExponentialBackOffSettings.DeltaBackOff, settings.ExponentialBackOffSettings.MaxNumberOfRetries);

            // Create the Initializer. Make sure to set the Maximum Timespan between two Requests. It is 16 Seconds per Default:
            var backoffInitializer = new BackOffHandler.Initializer(backoff)
            {
                MaxTimeSpan = settings.ExponentialBackOffSettings.MaxTimeSpan
            };

            args.Initializers.Add(new ExponentialBackOffInitializer(ExponentialBackOffPolicy.UnsuccessfulResponse503, () => new BackOffHandler(backoffInitializer)));

            return args;
        }

        private void InitializeExponentialBackOff(ConfigurableHttpClient client, IFcmClientSettings settings)
        {
            // The Maximum Number of Retries is limited to 3 per default for a ConfigurableHttpClient. This is 
            // somewhat weird, because the ExponentialBackOff Algorithm is initialized with 10 Retries per default.
            // 
            // Somehow the NumTries seems to be the limiting factor here, so it basically overrides anything you 
            // are going to write in the Exponential Backoff Handler.
            client.MessageHandler.NumTries = settings.ExponentialBackOffSettings.MaxNumberOfRetries;
        }


        public void Dispose()
        {
            client?.Dispose();
        }
    }
}