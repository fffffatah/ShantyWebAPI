using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.User
{
    [BsonIgnoreExtraElements]
    public class LabelGetInfoModel
    {
        [BsonElement("LabelId")]
        public string LabelId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [BsonElement("LabelIconUrl")]
        public string LabelIconUrl { get; set; }
        [BsonElement("LabelName")]
        public string LabelName { get; set; }
        [BsonElement("EstDate")]
        public string EstDate { get; set; }
        [BsonElement("Region")]
        public string Region { get; set; }
        [BsonElement("IsVerified")]
        public string IsVerified { get; set; }
    }
}
