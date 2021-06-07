using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.CustomAttributes;

namespace ShantyWebAPI.Models.Album
{
    public class AlbumCreateModel
    {
        [Required]
        [ImageValidation]
        public IFormFile CoverImage { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string AlbumName { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string ArtistId { get; set; }
        [Required]
        public string LabelId { get; set; }
    }
}
