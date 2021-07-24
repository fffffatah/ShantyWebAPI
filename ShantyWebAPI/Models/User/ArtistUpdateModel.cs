using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShantyWebAPI.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.User
{
    public class ArtistUpdateModel
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
        public string Region { get; set; }
    }
}
