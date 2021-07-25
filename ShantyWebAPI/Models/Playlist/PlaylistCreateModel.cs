using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.Playlist
{
    public class PlaylistCreateModel
    {
        [FromHeader]
        [Required]
        public string JwtToken { get; set; }
        [JsonIgnore]
        public string PlaylistId { get; set; }
        [Required]
        public string PlaylistName { get; set; }
        public IFormFile PlaylistImage { get; set; }
        [JsonIgnore]
        public string PlaylistImageUrl { get; set; }
        [JsonIgnore]
        public string CreatorId { get; set; }
    }
}
