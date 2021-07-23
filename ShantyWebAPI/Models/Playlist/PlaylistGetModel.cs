using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;
using ShantyWebAPI.Models.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.Playlist
{
    [BsonIgnoreExtraElements]
    public class PlaylistGetModel
    {
        public string PlaylistId { get; set; }
        public string PlaylistName { get; set; }
        public string PlaylistImageUrl { get; set; }
        public List<SongGetModel> SongGetModels { get; set; }
        public string CreatorId { get; set; }
    }
}