using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.User
{
    public class ListenerGetInfoModel
    {
        public string ProfileImageUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string Region { get; set; }
        public string IsSubscriber { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
