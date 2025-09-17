using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssignment.Entities
{
    public class GlobalSetting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
