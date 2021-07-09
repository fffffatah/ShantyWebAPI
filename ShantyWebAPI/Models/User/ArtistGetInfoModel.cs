using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace ShantyWebAPI.Models.User
{
    [BsonIgnoreExtraElements]
    public class ArtistGetInfoModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
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
