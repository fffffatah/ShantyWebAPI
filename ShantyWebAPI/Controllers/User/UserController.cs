using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.Models.User;

namespace ShantyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //REGSTRATION
        [HttpPost]
        [Route("register/listener")]
        public ActionResult<ListenerRegistrationModel> PutListener([FromForm]ListenerRegistrationModel listenerRegistrationModel)
        {
            ListenerGlobalModel listenerGlobalModel = new ListenerGlobalModel();
            listenerGlobalModel.Id = BCrypt.Net.BCrypt.HashPassword(listenerRegistrationModel.Username + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
            listenerGlobalModel.ProfileImage = listenerRegistrationModel.ProfileImage;
            listenerGlobalModel.ProfileImageUrl = ""; //TODO: upload to blob storage and return uri
            listenerGlobalModel.Username = listenerRegistrationModel.Username;
            listenerGlobalModel.FirstName = listenerRegistrationModel.FirstName;
            listenerGlobalModel.LastName = listenerRegistrationModel.LastName;
            listenerGlobalModel.Phone = listenerRegistrationModel.Phone;
            listenerGlobalModel.Email = listenerRegistrationModel.Email;
            listenerGlobalModel.Region = listenerRegistrationModel.Region;
            listenerGlobalModel.Dob = listenerRegistrationModel.Dob;
            listenerGlobalModel.Pass = BCrypt.Net.BCrypt.HashPassword(listenerRegistrationModel.Pass + Environment.GetEnvironmentVariable("PASSWORD_SALT"));
            listenerGlobalModel.IsEmailVerified = "false";
            listenerGlobalModel.IsSubscriber = "false";
            listenerGlobalModel.Type = "listener";
            return StatusCode(StatusCodes.Status200OK, "Listener Account Created");
        }
    }
}
