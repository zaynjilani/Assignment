using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssignment.Services.DTOs
{
    public class UserRegistrationModel
    {

        [Required(ErrorMessage = "Customer Name is required.")]
        [MaxLength(100, ErrorMessage = "Customer Name cannot be longer than 100 characters.")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "IC Number is required.")]
        [MaxLength(20, ErrorMessage = "IC Number cannot be longer than 20 characters.")]
        public string? ICNumber { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [Phone(ErrorMessage = "Invalid Mobile Number format.")]
        public string? MobileNumber { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        public string? EmailAddress { get; set; }
    }
}
