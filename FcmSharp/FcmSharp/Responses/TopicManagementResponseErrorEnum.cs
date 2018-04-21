using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FcmSharp.Responses
{
    public enum TopicManagementResponseErrorEnum
    {
        None = 0,

        InvalidArgument = 1,

        NotFound = 2,

        Internal = 3,

        TooManyTopics = 4,

        Unknown = 5
    }
}
