using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.Album
{
    public class AlbumGlobalModel
    {
        public string Id { get; set; }
        public IFormFile CoverImage { get; set; }
        public string CoverImageUrl { get; set; }
        public string AlbumName { get; set; }
        public string Year { get; set; }
        public string ArtistId { get; set; }
        public string LabelId { get; set; }
    }
}
