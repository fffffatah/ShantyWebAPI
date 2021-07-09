using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ShantyWebAPI.Models.Album
{
    [BsonIgnoreExtraElements]
    public class AlbumGetModel
    {
        public string AlbumId { get; set; }
        public string CoverImageUrl { get; set; }
        public string AlbumName { get; set; }
        public string Year { get; set; }
        public string Genre { get; set; }
        public string ArtistId { get; set; }
        public string LabelId { get; set; }
    }
}
