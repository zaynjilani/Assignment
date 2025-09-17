using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssignment.Services.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? ICNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailAddress { get; set; }
    }
}
