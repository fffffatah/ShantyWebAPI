using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShantyWebAPI.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.User
{
    public class ListenerUpdateModel
    {
        [Required]
        [FromHeader]
        public string JwtToken { get; set; }
        [JsonIgnore]
        public string Id { get; set; }
        [ImageValidation]
        public IFormFile ProfileImage { get; set; }
        [JsonIgnore]
        public string ProfileImageUrl { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }
        [DobValidation]
        public string Dob { get; set; }
    }
}
