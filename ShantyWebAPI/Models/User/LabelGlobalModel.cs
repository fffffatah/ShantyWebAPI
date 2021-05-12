using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.User
{
    public class LabelGlobalModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pass { get; set; }
        public string Type { get; set; }
        public string IsEmailVerified { get; set; }
        public IFormFile LabelIcon { get; set; }
        public string LabelIconUrl { get; set; }
        public string LabelName { get; set; }
        public string EstDate { get; set; }
        public string Region { get; set; }
        public string IsVerified { get; set; }
    }
}
