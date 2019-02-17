// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace FcmSharp.Scheduler.Quartz.Web.Converters
{
    public static class MessageConverter
    {
        public static Database.Model.Message Convert(Contracts.Message source)
        {
            if (source == null)
            {
                return null;
            }

            return new Database.Model.Message
            {
                Id = source.Id,
                Topic = source.Topic,
                Title = source.Title,
                Body = source.Body,
                ScheduledTime = source.ScheduledTime,
                Status = Convert(source.Status)
            };
        }

        public static Database.Model.StatusEnum Convert(Contracts.StatusEnum source)
        {
            switch (source)
            {
                case Contracts.StatusEnum.Scheduled:
                    return Database.Model.StatusEnum.Scheduled;
                case Contracts.StatusEnum.Finished:
                    return Database.Model.StatusEnum.Finished;
                case Contracts.StatusEnum.Failed:
                    return Database.Model.StatusEnum.Failed;
                default:
                    throw new ArgumentException($"Unknown Source StatusEnum {source}");
            }
        }

        public static Contracts.Message Convert(Database.Model.Message source)
        {
            if (source == null)
            {
                return null;
            }

            return new Contracts.Message
            {
                Id = source.Id,
                Topic = source.Topic,
                Title = source.Title,
                Body = source.Body,
                ScheduledTime = source.ScheduledTime,
                Status = Convert(source.Status)
            };
        }

        public static Contracts.StatusEnum Convert(Database.Model.StatusEnum source)
        {
            switch (source)
            {
                case Database.Model.StatusEnum.Scheduled:
                    return Contracts.StatusEnum.Scheduled;
                case Database.Model.StatusEnum.Finished:
                    return Contracts.StatusEnum.Finished;
                case Database.Model.StatusEnum.Failed:
                    return Contracts.StatusEnum.Failed;
                default:
                    throw new ArgumentException($"Unknown Source StatusEnum {source}");
            }
        }
    }
}
