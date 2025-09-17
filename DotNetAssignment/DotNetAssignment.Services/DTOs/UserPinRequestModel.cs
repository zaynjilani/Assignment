using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssignment.Services.DTOs
{
    public class UserPinRequestModel
    {
        [Required(ErrorMessage = "User Id is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Pin is required.")]
        public int Pin { get; set; } = 0;
    }
}
