using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.Song
{
    public class SongGlobalModel
    {
        public string Id { get; set; }
        public IFormFile CoverImage { get; set; }
        public string CoverImageUrl { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public IFormFile SongFile { get; set; }
        public string SongFileUrl { get; set; }
        public string Genre { get; set; }
        public int TimesStreamed { get; set; }
        public string AlbumId { get; set; }
    }
}
