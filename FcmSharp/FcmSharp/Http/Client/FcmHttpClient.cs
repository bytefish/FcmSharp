// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using FcmSharp.Exceptions;
using FcmSharp.Http.Retry;
using FcmSharp.Serializer;
using FcmSharp.Http.Constants;
using FcmSharp.Settings;
using FcmSharp.Http.Utils;

namespace FcmSharp.Http
{
    public class FcmHttpClient : IFcmHttpClient
    {
        private readonly HttpClient client;
        private readonly IJsonSerializer serializer;
        private readonly IFcmClientSettings settings;

        public FcmHttpClient(IFcmClientSettings settings)
            : this(settings, new HttpClient(), JsonSerializer.Default)
        {
        }

        public FcmHttpClient(IFcmClientSettings settings, HttpClient client, IJsonSerializer serializer)
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

        public async Task<TResponseType> PostAsync<TRequestType, TResponseType>(TRequestType request, CancellationToken cancellationToken)
        {
            return await PostAsync<TRequestType, TResponseType>(request, default(HttpCompletionOption), cancellationToken);
        }

        public async Task<TResponseType> PostAsync<TRequestType, TResponseType>(TRequestType request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            // Now build the HTTP Request Message:
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, settings.FcmUrl);
            
            // Build the Content of the Request:
            StringContent content = new StringContent(serializer.SerializeObject(request), Encoding.UTF8, MediaTypeNames.ApplicationJson);

            // Append it to the Request:
            httpRequestMessage.Content = content;

            // Add the Authorization Header with the API Key:
            AddAuthorizationHeader(httpRequestMessage);

            // Invoke actions before the Request:
            OnBeforeRequest(httpRequestMessage);

            // Invoke the Request:
            HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage, completionOption, cancellationToken);

            // Invoke actions after the Request:
            OnAfterResponse(httpRequestMessage, httpResponseMessage);

            // Evaluate the Response:
            EvaluateResponse(httpResponseMessage);

            // Now read the Response Content as String:
            string httpResponseContentAsString = await httpResponseMessage.Content.ReadAsStringAsync();

            // And finally return the Object:
            return serializer.DeserializeObject<TResponseType>(httpResponseContentAsString);
        }

        public async Task PostAsync<TRequestType>(TRequestType request, CancellationToken cancellationToken)
        {
            await PostAsync(request, default(HttpCompletionOption), cancellationToken);
        }

        public async Task PostAsync<TRequestType>(TRequestType request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            // Build the HTTP Request Message:
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, settings.FcmUrl);
            
            // Build the Content of the Request:
            httpRequestMessage.Content = new StringContent(serializer.SerializeObject(request), Encoding.UTF8, MediaTypeNames.ApplicationJson);

            // Add the Authorization Header with the API Key:
            AddAuthorizationHeader(httpRequestMessage);

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

        public void AddAuthorizationHeader(HttpRequestMessage httpRequestMessage)
        {
            string apiKey = settings.ApiKey;

            httpRequestMessage.Headers.TryAddWithoutValidation(HttpHeaderNames.Authorization, string.Format("key={0}", apiKey));
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

            String reasonPhrase = response.ReasonPhrase;

            if (httpStatusCode == HttpStatusCode.BadRequest)
            {
                throw new FcmBadRequestException(reasonPhrase);
            }

            if (httpStatusCode == HttpStatusCode.Unauthorized)
            {
                throw new FcmAuthenticationException(reasonPhrase);
            }

            int httpStatusCodeIntegerValue = Convert.ToInt32(httpStatusCode);

            if (httpStatusCodeIntegerValue >= 500 && httpStatusCodeIntegerValue < 600)
            {
                RetryConditionValue retryConditionValue;

                if (RetryUtils.TryDetermineRetryDelay(response, out retryConditionValue))
                {
                    throw new FcmRetryAfterException(reasonPhrase, retryConditionValue);
                }
            }

            throw new FcmGeneralException(reasonPhrase);
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
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
    }
}