using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace ShantyWebAPI.Models.User
{
    [BsonIgnoreExtraElements]
    public class ArtistGetInfoModel
    {
        [JsonIgnore]
        public string Username { get; set; }
        [JsonIgnore]
        public string Email { get; set; }
        [JsonIgnore]
        public string Phone { get; set; }
        [BsonElement("ArtistId")]
        public string ArtistId { get; set; }
        [BsonElement("ProfileImageUrl")]
        public string ProfileImageUrl { get; set; }
        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        [BsonElement("LastName")]
        public string LastName { get; set; }
        [BsonElement("Dob")]
        public string Dob { get; set; }
        [BsonElement("Region")]
        public string Region { get; set; }
        [BsonElement("IsVerified")]
        public string IsVerified { get; set; }
        [BsonElement("LabelId")]
        public string LabelId { get; set; }
    }
}
