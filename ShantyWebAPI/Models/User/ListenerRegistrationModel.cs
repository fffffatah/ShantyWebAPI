﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.CustomAttributes;

namespace ShantyWebAPI.Models.User
{
    public class ListenerRegistrationModel
    {
        [ImageValidation]
        public IFormFile ProfileImage { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }
        [DobValidation]
        public string Dob { get; set; }
        [Required]
        public string Region { get; set; }
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]{5,11}$", ErrorMessage = "Username Must be between 6-12 Characters and Must Not Contain Special Character")]
        [UsernameValidation]
        public string Username { get; set; }
        [EmailAddress]
        [EmailValidation]
        public string Email { get; set; }
        [PhoneValidation]
        public string Phone { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password Must be 8 in Length and Must Contain Uppercase, Number and a Special Character")]
        public string Pass { get; set; }
    }
}
