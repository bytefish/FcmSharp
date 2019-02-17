// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Scheduler.Quartz.Database;
using FcmSharp.Scheduler.Quartz.Database.Model;
using FcmSharp.Scheduler.Quartz.Extensions;
using FcmSharp.Scheduler.Quartz.Services.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FcmSharp.Scheduler.Quartz.Services
{
    public interface IMessagingService : IDisposable
    {
        Task SendScheduledMessageAsync(int messageId, CancellationToken cancellationToken);
    }

    public class MessagingService : IMessagingService
    {
        private readonly ILogger<MessagingService> logger;
        private readonly IFcmClient client;

        public MessagingService(ILogger<MessagingService> logger, IFcmClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public async Task SendScheduledMessageAsync(int messageId, CancellationToken cancellationToken)
        {
            if (logger.IsDebugEnabled())
            {
                logger.LogDebug($"Sending scheduled Message ID {messageId}");
            }

            var message = await GetScheduledMessageAsync(messageId, cancellationToken);

            await SendMessageAsync(message, cancellationToken);
        }

        private async Task SendMessageAsync(Message message, CancellationToken cancellationToken)
        {
            var target = MessageConverter.Convert(message);

            try
            {
                await client.SendAsync(target, cancellationToken);

                if (logger.IsDebugEnabled())
                {
                    logger.LogDebug($"Finished sending Message ID {message.Id}");
                }

                await SetMessageStatusAsync(message, StatusEnum.Finished, cancellationToken);
            }
            catch (Exception exception)
            {
                if (logger.IsErrorEnabled())
                {
                    logger.LogError(exception, $"Error sending Message ID {message.Id}");
                }

                await SetMessageStatusAsync(message, StatusEnum.Failed, cancellationToken);
            }
        }

        private Task<Message> GetScheduledMessageAsync(int messageId, CancellationToken cancellationToken)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Messages
                    .Where(x => x.Status == StatusEnum.Scheduled)
                    .Where(x => x.Id == messageId)
                    .AsNoTracking()
                    .FirstAsync(cancellationToken);
            }
        }

        private async Task SetMessageStatusAsync(Message message, StatusEnum status, CancellationToken cancellationToken)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Attach(message);

                // Set the new Status Value:
                message.Status = status;

                // Mark the Status as modified, so it is the only updated value:
                context
                    .Entry(message)
                    .Property(x => x.Status).IsModified = true;

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
