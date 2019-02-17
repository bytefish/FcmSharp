// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Scheduler.Quartz.Database;
using Microsoft.AspNetCore.Builder;

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
