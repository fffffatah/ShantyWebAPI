using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.CustomAttributes;

namespace ShantyWebAPI.Models.User
{
    public class SendOtpModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Otp must be 6 characters")]
        public string Otp { get; set; }
    }
}
