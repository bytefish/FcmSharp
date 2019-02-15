using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Scheduler.Converters;
using FcmSharp.Scheduler.Database;
using FcmSharp.Scheduler.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace FcmSharp.Scheduler.Services
{
    public interface ISchedulerService :IDisposable
    {
        Task SendScheduledMessagesAsync(DateTime scheduledTime, CancellationToken cancellationToken);
    }

    public class SchedulerService : ISchedulerService
    {
        private readonly IFcmClient client;

        public SchedulerService(IFcmClient client)
        {
            this.client = client;
        }

        public async Task SendScheduledMessagesAsync(DateTime scheduledTime, CancellationToken cancellationToken)
        {
            var messages = await GetScheduledMessagesAsync(scheduledTime, cancellationToken);

            await SendMessagesAsync(messages, cancellationToken);
        }

        private async Task SendMessagesAsync(List<Message> messages, CancellationToken cancellationToken)
        {
            foreach (var message in messages)
            {
                var target = MessageConverter.Convert(message);

                try
                {
                    await client.SendAsync(target, cancellationToken);
                    await SetMessageStatusAsync(message, StatusEnum.Finished, cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[Error] {e.StackTrace}");

                    await SetMessageStatusAsync(message, StatusEnum.Failed, cancellationToken);
                }
            }
        }

        private Task<List<Message>> GetScheduledMessagesAsync(DateTime scheduledTime, CancellationToken cancellationToken)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Messages
                    .Where(x => x.Status == StatusEnum.Scheduled)
                    .Where(x => x.ScheduledTime <= scheduledTime)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
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
