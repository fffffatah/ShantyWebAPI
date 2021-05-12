using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.Models.User;
using System.Text;

namespace ShantyWebAPI.Controllers.User
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
            string GenerateUserId(string input)
            {
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return sb.ToString().ToLower();
                }
            }
            ListenerGlobalModel listenerGlobalModel = new ListenerGlobalModel();
            listenerGlobalModel.Id = GenerateUserId(listenerRegistrationModel.Username + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
            listenerGlobalModel.ProfileImage = listenerRegistrationModel.ProfileImage;
            listenerGlobalModel.ProfileImageUrl = new UserDataAccess().UploadProfileImage(listenerGlobalModel.ProfileImage, listenerGlobalModel.Id);
            listenerGlobalModel.Username = listenerRegistrationModel.Username;
            listenerGlobalModel.FirstName = listenerRegistrationModel.FirstName;
            listenerGlobalModel.LastName = listenerRegistrationModel.LastName;
            listenerGlobalModel.Phone = listenerRegistrationModel.Phone;
            listenerGlobalModel.Email = listenerRegistrationModel.Email;
            listenerGlobalModel.Region = listenerRegistrationModel.Region;
            listenerGlobalModel.Dob = listenerRegistrationModel.Dob;
            listenerGlobalModel.Pass = BCrypt.Net.BCrypt.HashPassword(listenerRegistrationModel.Pass, BCrypt.Net.BCrypt.GenerateSalt());
            listenerGlobalModel.IsEmailVerified = "false";
            listenerGlobalModel.IsSubscriber = "false";
            listenerGlobalModel.Type = "listener";
            if(new UserDataAccess().RegisterListener(listenerGlobalModel))
            {
                return StatusCode(StatusCodes.Status200OK, "Listener Account Created");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Listener Account Creation Failed");
        }
    }
}
