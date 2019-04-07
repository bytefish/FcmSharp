using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FcmSharp.Http.Builder;
using FcmSharp.Http.Constants;
using FcmSharp.Requests;
using FcmSharp.Serializer;
using Newtonsoft.Json;

namespace FcmSharp.Batch
{
    public class SubRequestBody
    {
        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("validate_only")]
        public bool? ValidateOnly { get; set; }
    }

    public class SubRequest
    {
        public string Url { get; set; }

        public SubRequestBody Body { get; set; }

        public IDictionary<string, string> Headers { get; set; }
    }

    /// <summary>
    /// This Builder for Batch Messages is the implementation of the FCM Node SDK:
    /// 
    ///     https://github.com/firebase/firebase-admin-node/blob/bf3dbd117ffc3c895a150eee2169eb7c02f4f4cc/src/messaging/batch-request.ts
    ///
    /// All Credit for the implementation goes to the original authors.
    /// </summary>
    public class BatchMessageBuilder
    {
        private const string PART_BOUNDARY = "__END_OF_PART__";

        private readonly IJsonSerializer serializer;

        public BatchMessageBuilder(IJsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public HttpRequestMessageBuilder Build(SubRequest[] requests)
        {
            byte[] multipartPayload = GetMultipartPayload(requests);

            var byteArrayContent = new ByteArrayContent(multipartPayload);

            byteArrayContent.Headers.Remove("Content-Type");
            byteArrayContent.Headers.Add("Content-Type", $"multipart/mixed; boundary={PART_BOUNDARY}");

            return new HttpRequestMessageBuilder("https://fcm.googleapis.com/batch", HttpMethod.Post)
                .AddHeader("access_token_auth", "true")
                .SetHttpContent(byteArrayContent);
        }
        
        private byte[] GetMultipartPayload(SubRequest[] requests)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int requestIdx = 0; requestIdx < requests.Length; requestIdx++)
            {
                var request = requests[requestIdx];
                var part = CreatePart(request, requestIdx);

                stringBuilder.Append(part);
            }
            
            stringBuilder.Append($"--{PART_BOUNDARY}--\r\n");

            string multiPartPayload = stringBuilder.ToString();

            return Encoding.UTF8.GetBytes(multiPartPayload);
        }

        private string CreatePart(SubRequest request, int index)
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

        public string SerializeSubRequest(SubRequest request)
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
