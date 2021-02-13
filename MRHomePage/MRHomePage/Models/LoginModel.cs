using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MRHomePage.Models
{
    public class LoginModel
    {
        [Required]
        [MinLength(1)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
        public string RedirectUrl { get; set; }
        public bool? IsWrongCredentials { get; set; }
    }
}
