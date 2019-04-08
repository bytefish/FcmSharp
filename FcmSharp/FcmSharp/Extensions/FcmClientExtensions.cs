using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Requests;
using FcmSharp.Responses;

namespace FcmSharp
{
    public static class FcmClientExtensions
    {
        public static Task<FcmBatchResponse> SendMulticastMessage(this IFcmClient client, string[] tokens, Message message, bool dryRun = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var messages = tokens.Select(token => BuildMessage(token, message)).ToArray();

            return client.SendBatchAsync(messages, dryRun, cancellationToken);
        }

        private static Message BuildMessage(string token, Message message)
        {
            return new Message
            {
                Token = token,
                AndroidConfig = message.AndroidConfig,
                ApnsConfig = message.ApnsConfig,
                Condition = message.Condition,
                Data = message.Data,
                Notification = message.Notification,
                WebpushConfig = message.WebpushConfig,
                Topic = null
            };
        }
    }
}
