using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShantyWebAPI.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.Song
{
    public class SongUploadModel
    {
        [Required]
        [FromHeader]
        public string JwtToken { get; set; }
        [JsonIgnore]
        public string SongId { get; set; }
        [Required]
        public string SongName { get; set; }
        [Required]
        public string ArtistName { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        [AudioFileValidation]
        public IFormFile SongFile { get; set; }
        [JsonIgnore]
        public string SongFileUrl { get; set; }
        [Required]
        public string AlbumId { get; set; }
        [Required]
        [ImageValidation]
        public IFormFile CoverImage { get; set; }
        [JsonIgnore]
        public string CoverImageUrl { get; set; }
    }
}
