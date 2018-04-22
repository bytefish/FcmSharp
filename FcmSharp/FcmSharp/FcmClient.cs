// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        private readonly IFcmHttpClient httpClient;
        
        private static readonly Dictionary<HttpStatusCode, String> IID_ERROR_CODES = new Dictionary<HttpStatusCode, string>
        {
            {HttpStatusCode.BadRequest, "invalid-argument"},
            {HttpStatusCode.Unauthorized, "authentication-error"},
            {HttpStatusCode.Forbidden, "authentication-error"},
            {HttpStatusCode.InternalServerError, "internal-error"},
            {HttpStatusCode.ServiceUnavailable, "503"}
        };

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



        public void AddAuthorizationHeader(HttpRequestMessage httpRequestMessage)
        {
            string apiKey = settings.ApiKey;

            httpRequestMessage.Headers.TryAddWithoutValidation(HttpHeaderNames.Authorization, string.Format("key={0}", apiKey));
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