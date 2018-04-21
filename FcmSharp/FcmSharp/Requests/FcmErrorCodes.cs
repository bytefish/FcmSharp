using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FcmSharp.Requests
{
    public enum FcmErrorCodes
    {
        RegistrationTokenNotRegistered = 0,

        MismatchedCredential = 1,

        MessageRateExceeded = 2,

        InvalidApnsCredentials = 3,

        InternalError = 4,

        InvalidArgument = 5,

        ServerUnavailable = 6
    }

    public enum IidErrorCodes
    {
        InvalidArgument = 0,

        AuthenticationError = 1,

        InternalError = 2,

        ServerUnavailable = 3
    }
}
