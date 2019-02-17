using System;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Scheduler.Database;
using FcmSharp.Scheduler.Services;
using FcmSharp.Settings;

namespace FcmSharp.Scheduler
{
    class Program
    {
        private static readonly TimeSpan PollingInterval = TimeSpan.FromMinutes(1);

        static async Task Main(string[] args)
        {
            var cancellationToken = CancellationToken.None;

            // Initializes the Database:
            await CreateDatabase(cancellationToken);

            // Starts the Scheduling Loop for Message Scheduling:
            await ProcessMessages(cancellationToken);
        }


        public static async Task ProcessMessages(CancellationToken cancellationToken)
        {
            var service = CreateSchedulerService();

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(PollingInterval, cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    DateTime scheduledTime = DateTime.UtcNow;

                    Console.WriteLine($"[${DateTime.Now}] [INFO] Sending Messages scheduled at {scheduledTime}");

                    await service.SendScheduledMessagesAsync(scheduledTime, cancellationToken);
                }
            }

            service.Dispose();
        }

        private static Task CreateDatabase(CancellationToken cancellationToken)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Database.EnsureCreatedAsync(cancellationToken);
            }
        }

        private static ISchedulerService CreateSchedulerService()
        {
            // Read the Credentials from a File, which is not under Version Control:
            var settings = FileBasedFcmClientSettings.CreateFromFile(@"D:\serviceAccountKey.json");

            // Construct the Client:
            var client = new FcmClient(settings);
            
            return new SchedulerService(client);
        }
    }
}
