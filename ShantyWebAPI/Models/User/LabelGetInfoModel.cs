using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models.User
{
    public class LabelGetInfoModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LabelIconUrl { get; set; }
        public string LabelName { get; set; }
        public string EstDate { get; set; }
        public string Region { get; set; }
        public string IsVerified { get; set; }
    }
}
