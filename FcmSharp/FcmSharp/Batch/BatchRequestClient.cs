using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FcmSharp.Http.Builder;
using FcmSharp.Http.Constants;
using FcmSharp.Serializer;

namespace FcmSharp.Batch
{
    public class SubRequest<TPayloadType>
    {
        public string Url { get; set; }

        public TPayloadType Body { get; set; }

        public IDictionary<string, string> Headers { get; set; }
    }

    public class BatchRequestClient
    {
        private const string PART_BOUNDARY = "__END_OF_PART__";

        private readonly HttpClient httpClient;
        private readonly string batchUrl;
        private readonly IDictionary<string, string> headers;
        private readonly IJsonSerializer serializer;

        public BatchRequestClient(HttpClient httpClient, IJsonSerializer serializer, string batchUrl, IDictionary<string, string> headers)
        {
            this.httpClient = httpClient;
            this.batchUrl = batchUrl;
            this.headers = headers;
            this.serializer = serializer;
        }

        public async Task SendBatchAsync<TPayloadType>(SubRequest<TPayloadType>[] requests)
        {
            byte[] multipartPayload = GetMultipartPayload(requests);

            var request = new HttpRequestMessageBuilder(batchUrl, HttpMethod.Post)
                .AddHeader(HttpHeaderNames.ContentType, MediaTypeNames.MultipartMixed)
                .SetHttpContent(new ByteArrayContent(multipartPayload))
                .Build();

            var httpResponseMessage = await httpClient.SendAsync(request);

            
        }

        public byte[] GetMultipartPayload<TPayload>(SubRequest<TPayload>[] requests)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int requestIdx = 0; requestIdx < requests.Length; requestIdx++)
            {
                var request = requests[requestIdx];
                var part = CreatePart(request, requestIdx);

                stringBuilder.Append(part);
            }
            
            stringBuilder.Append($"--${PART_BOUNDARY}--\r\n");

            string multiPartPayload = stringBuilder.ToString();

            return Encoding.UTF8.GetBytes(multiPartPayload):
        }

        public string CreatePart<TPayloadType>(SubRequest<TPayloadType> request, int index)
        {
            string serializedRequest = SerializeSubRequest(request);

            StringBuilder part = new StringBuilder()
                .Append($"--{PART_BOUNDARY}\r\n")
                .Append($"Content-Length: {serializedRequest.Length}\r\n")
                .Append("Content-Type: application/http\r\n")
                .Append($"content-id: {index + 1}\r\n")
                .Append("content-transfer-encoding: binary\r\n")
                .Append("\r\n")
                .Append($"{serializedRequest}\r\n");

            return part.ToString();
        }

        public string SerializeSubRequest<TPayloadType>(SubRequest<TPayloadType> request)
        {
            string requestBody = serializer.SerializeObject(request.Body);

            StringBuilder messagePayload = new StringBuilder()
                .Append($"POST {request.Url} HTTP/1.1\r\n")
                .Append($"Content-Length: {requestBody.Length}\r\n")
                .Append("Content-Type: application/json; charset=UTF-8\r\n");

            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    messagePayload.Append($"{header.Key}: {header.Value}\r\n");
                }
            }
            messagePayload.Append("\r\n");
            messagePayload.Append(requestBody);

            return messagePayload.ToString();
        }
    }
}
