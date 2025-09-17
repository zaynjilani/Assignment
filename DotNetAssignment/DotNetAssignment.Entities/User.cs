using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssignment.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? ICNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailAddress { get; set; }
        public bool IsPrivacyPolicyAccepted { get; set; }
        public bool IsBiometricLoginEnabled { get; set; }
        public int? MobileOTP { get; set; }
        public int? EmailOTP { get; set; }
        public int PIN { get; set; }
        public DateTime? OTPExpiry { get; set; }
        public bool IsMobileVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
