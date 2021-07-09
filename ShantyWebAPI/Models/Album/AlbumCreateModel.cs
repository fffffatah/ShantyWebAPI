using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.CustomAttributes;
using Microsoft.AspNetCore.Mvc;

namespace ShantyWebAPI.Models.Album
{
    public class AlbumCreateModel
    {
        [Required]
        [FromHeader]
        public string JwtToken { get; set; }
        [Required]
        [ImageValidation]
        public IFormFile CoverImage { get; set; }
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
        public string LabelId { get; set; }
    }
}
