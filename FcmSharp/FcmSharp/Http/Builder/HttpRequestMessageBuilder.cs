// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace FcmSharp.Http.Builder
{
    public class HttpRequestMessageBuilder
    {
        private class Header
        {
            public readonly string Name;
            public readonly string Value;

            public Header(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }

        private string url;
        private HttpMethod httpMethod;
        private IDictionary<string, string> parameters;
        private IList<Header> headers;
        private IList<UrlSegment> segments;
        private HttpContent content;

        public HttpRequestMessageBuilder(string url, HttpMethod httpMethod)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            this.url = url;
            this.httpMethod = httpMethod;
            this.headers = new List<Header>();
            this.segments = new List<UrlSegment>();
            this.content = null;
            this.parameters = new Dictionary<string, string>();
        }

        public HttpRequestMessageBuilder HttpMethod(HttpMethod httpMethod)
        {
            this.httpMethod = httpMethod;

            return this;
        }

        public HttpRequestMessageBuilder AddHeader(string name, string value)
        {
            this.headers.Add(new Header(name, value));

            return this;
        }

        public HttpRequestMessageBuilder SetHeader(string name, string value)
        {
            var header = this.headers.FirstOrDefault(x => x.Name == name);

            if (header != null)
            {
                this.headers.Remove(header);
            }

            AddHeader(name, value);

            return this;
        }

        public HttpRequestMessageBuilder SetStringContent(string content, Encoding encoding, string mediaType)
        {
            this.content = new StringContent(content, encoding, mediaType);

            return this;
        }

        public HttpRequestMessageBuilder SetHttpContent(HttpContent httpContent)
        {
            this.content = httpContent;

            return this;
        }

        public HttpRequestMessageBuilder AddUrlSegment(string name, string value)
        {
            this.segments.Add(new UrlSegment(name, value));

            return this;
        }

        public HttpRequestMessageBuilder AddQueryString(string key, string value)
        {
            this.parameters.Add(key, value);

            return this;
        }

        public HttpRequestMessage Build()
        {
            string resourceUrl = HttpRequestUtils.ReplaceSegments(url, segments);
            string queryString = HttpRequestUtils.BuildQueryString(resourceUrl, parameters);
            string resourceUrlWithQueryString = string.Format("{0}{1}", resourceUrl, queryString);

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, resourceUrlWithQueryString);

            foreach (var header in headers)
            {
                httpRequestMessage.Headers.TryAddWithoutValidation(header.Name, header.Value);
            }

            if (content != null)
            {
                httpRequestMessage.Content = content;
            }

            return httpRequestMessage;
        }
    }
}