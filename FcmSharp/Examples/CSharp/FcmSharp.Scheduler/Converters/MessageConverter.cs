using System.Collections.Generic;
using System.Linq;
using FcmSharp.Requests;
using SourceType = FcmSharp.Scheduler.Database.Model.Message;
using TargetType = FcmSharp.Requests.FcmMessage;

namespace FcmSharp.Scheduler.Converters
{
    public static class MessageConverter
    {
        public static List<TargetType> Convert(List<SourceType> source)
        {
            if (source == null)
            {
                return null;
            }

            return source
                .Select(x => Convert(x))
                .ToList();
        }

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
