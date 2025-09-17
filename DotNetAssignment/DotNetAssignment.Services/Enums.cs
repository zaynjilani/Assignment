using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssignment.Services
{
    public enum OtpType
    {
        Mobile = 1,
        Email
    }
    public enum messagesEnum
    {
        GENERAL_SUCCESS = 4001,
        GENERAL_ERROR,
        ACCOUNT_ALREADY_EXISTS,
        NOT_FOUND,
        UNMATCHED_PIN,
        INCORRECT_OTP,
    }
}
