using DotNetAssignment.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssignment.Services.Helpers
{
    public class GeneralHelper
    {
        #region OTP
        public static int GenerateOtp()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }
        #endregion

        #region Common Response Model
        public static GeneralModel ResponseModel(object data, messagesEnum msgCode, string msgDescription)
        {
            return new GeneralModel
            {
                data = data,
                msgCode = Convert.ToString(msgCode),
                msgDescription = msgDescription,
            };
        }
        #endregion
    }
}
