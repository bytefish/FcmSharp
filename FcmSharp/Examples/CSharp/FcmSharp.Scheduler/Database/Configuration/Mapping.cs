using FcmSharp.Scheduler.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FcmSharp.Scheduler.Database.Configuration
{
    public class MessageTypeConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .ToTable("message")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("message_id")
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.Topic)
                .HasColumnName("topic")
                .IsRequired();

            builder
                .Property(x => x.Title)
                .HasColumnName("title")
                .IsRequired();

            builder
                .Property(x => x.Body)
                .HasColumnName("body")
                .IsRequired();

            builder
                .Property(x => x.ScheduledTime)
                .HasColumnName("scheduled_time")
                .IsRequired();

            builder
                .Property(x => x.Status)
                .HasConversion<int>()
                .HasColumnName("status_id");
        }
    }
}
