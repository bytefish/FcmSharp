// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Scheduler.Quartz.Database.Configuration;
using FcmSharp.Scheduler.Quartz.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace FcmSharp.Scheduler.Quartz.Database
{

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Messaging.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MessageTypeConfiguration());
            
            Seeding.SeedData(modelBuilder);
        }
    }
}
