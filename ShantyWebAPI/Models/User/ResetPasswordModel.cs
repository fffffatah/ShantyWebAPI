using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.User
{
    public class ResetPasswordModel
    {
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password Must be 8 in Length and Must Contain Uppercase, Number and a Special Character")]
        public string NewPass { get; set; }
        [Required]
        [Compare("NewPass", ErrorMessage = "New Password and Confirm Password must be same")]
        public string ConfirmPass { get; set; }
    }
}
