using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.User
{
    public class ArtistGlobalModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pass { get; set; }
        public string Type { get; set; }
        public string IsEmailVerified { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string ProfileImageUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string Region { get; set; }
        public string IsVerified { get; set; }
        public string LabelId { get; set; }
    }
}
