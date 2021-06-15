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
    public class LabelUpdateModel
    {
        [Required]
        [FromHeader]
        public string JwtToken { get; set; }
        [JsonIgnore]
        public string Id { get; set; }
        [ImageValidation]
        public IFormFile LabelIcon { get; set; }
        [JsonIgnore]
        public string LabelIconUrl { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string LabelName { get; set; }
        public string EstDate { get; set; }
    }
}
