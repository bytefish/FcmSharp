using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FcmSharp.Batch
{
    public static class BatchUtils
    {
        public static HttpResponseMessage ParseAsHttpResponse(string content)
        {
            var response = new HttpResponseMessage();

            using (var reader = new StringReader(content))
            {
                string line = reader.ReadLine();

                // Extract empty lines.
                while (string.IsNullOrEmpty(line))
                    line = reader.ReadLine();

                // Extract the outer header.
                while (!string.IsNullOrEmpty(line))
                    line = reader.ReadLine();

                // Extract the status code.
                line = reader.ReadLine();
                while (string.IsNullOrEmpty(line))
                {
                    line = reader.ReadLine();
                }

                int code = int.Parse(line.Split(' ')[1]);
                response.StatusCode = (HttpStatusCode)code;

                // Extract the headers.
                IDictionary<string, string> headersDic = new Dictionary<string, string>();

                while (!string.IsNullOrEmpty((line = reader.ReadLine())))
                {
                    var separatorIndex = line.IndexOf(':');
                    var key = line.Substring(0, separatorIndex).Trim();
                    var value = line.Substring(separatorIndex + 1).Trim();
                    // Check if the header already exists, and if so append its value 
                    // to the existing value. Fixes issue #548.
                    if (headersDic.ContainsKey(key))
                    {
                        headersDic[key] = headersDic[key] + ", " + value;
                    }
                    else
                    {
                        headersDic.Add(key, value);
                    }
                }

                // Set the content.
                string mediaType = null;
                if (headersDic.ContainsKey("Content-Type"))
                {
                    mediaType = headersDic["Content-Type"].Split(';', ' ')[0];
                    headersDic.Remove("Content-Type");
                }

                var strContent = reader.ReadToEnd();

                response.Content = new StringContent(strContent, Encoding.UTF8, mediaType);

                // Add the headers to the response.
                foreach (var keyValue in headersDic)
                {
                    System.Net.Http.Headers.HttpHeaders headers = response.Headers;

                    // Check if we need to add the current header to the content headers.
                    if (typeof(System.Net.Http.Headers.HttpContentHeaders).GetProperty(keyValue.Key.Replace("-", "")) != null)
                    {
                        headers = response.Content.Headers;
                    }

                    if (!headers.TryAddWithoutValidation(keyValue.Key, keyValue.Value))
                    {
                        throw new FormatException($"Could not parse header {keyValue.Key} from batch reply");
                    }
                }
            }

            return response;
        }
    }
}
