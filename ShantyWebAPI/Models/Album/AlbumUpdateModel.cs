using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSwag.Annotations;
using ShantyWebAPI.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.Album
{
    public class AlbumUpdateModel
    {
        [Required]
        [FromHeader]
        public string JwtToken { get; set; }
        [JsonIgnore]
        public string Id { get; set; }
        [Required]
        [ImageValidation]
        public IFormFile CoverImage { get; set; }
        [JsonIgnore]
        public string CoverImageUrl { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string AlbumName { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string ArtistId { get; set; }
        [JsonIgnore]
        public string LabelId { get; set; }
    }
}
