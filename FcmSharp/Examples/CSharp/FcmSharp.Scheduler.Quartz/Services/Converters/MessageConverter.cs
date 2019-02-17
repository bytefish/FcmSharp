// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using FcmSharp.Requests;
using SourceType = FcmSharp.Scheduler.Quartz.Database.Model.Message;
using TargetType = FcmSharp.Requests.FcmMessage;

namespace FcmSharp.Scheduler.Quartz.Services.Converters
{
    public static class MessageConverter
    {
        public static TargetType Convert(SourceType source)
        {
            if (source == null)
            {
                return null;
            }

            return new TargetType
            {
                ValidateOnly = false,
                Message = new Message
                {
                    Topic = source.Topic,
                    Notification = new Notification
                    {
                        Title = source.Title,
                        Body = source.Body
                    }
                }
            };
        }
    }
}
