using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Models
{
    public class CustomResponseModel
    {
        public string Code { get; set; }
        public string Phrase { get; set; }
        public string Message { get; set; }
    }
}
