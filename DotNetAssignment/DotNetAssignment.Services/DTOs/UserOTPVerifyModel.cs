using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssignment.Services.DTOs
{
    public class UserOTPVerifyModel
    {
        [Required(ErrorMessage = "User Id is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "OTP Type is required.")]
        public OtpType OtpType { get; set; }

        [Required(ErrorMessage = "OTP Type Value is required.")]
        public string OtpTypeValue { get; set; } = string.Empty;

        [Required(ErrorMessage = "OTP is required.")]
        public int Otp { get; set; }
    }
}
