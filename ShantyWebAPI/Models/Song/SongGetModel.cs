using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.Song
{
    [BsonIgnoreExtraElements]
    public class SongGetModel
    {
        public string SongId { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public string SongFileUrl { get; set; }
        public string Genre { get; set; }
        public int TimesStreamed { get; set; }
        public string AlbumId { get; set; }
    }
}
