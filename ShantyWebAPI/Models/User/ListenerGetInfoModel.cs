using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.User
{
    [BsonIgnoreExtraElements]
    public class ListenerGetInfoModel
    {
        [BsonElement("ListenerId")]
        public string ListenerId { get; set; }
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
        [BsonElement("IsSubscriber")]
        public string IsSubscriber { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
