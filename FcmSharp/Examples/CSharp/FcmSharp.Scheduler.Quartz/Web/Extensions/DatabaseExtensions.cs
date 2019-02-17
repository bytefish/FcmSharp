using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FcmSharp.Scheduler.Quartz.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FcmSharp.Scheduler.Quartz.Web.Extensions
{
    public static class DatabaseExtensions
    {
        public static IApplicationBuilder EnsureDatabaseCreated(this IApplicationBuilder app)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Database.EnsureCreated();
            }

            return app;
        }
    }
}
