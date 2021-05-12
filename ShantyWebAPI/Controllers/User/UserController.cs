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
        //COMMON METHODS
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
        //REGSTRATION
        [HttpPost]
        [Route("register/listener")]
        public ActionResult<ListenerRegistrationModel> PutListener([FromForm]ListenerRegistrationModel listenerRegistrationModel)
        {
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
        [HttpPost]
        [Route("register/label")]
        public ActionResult<LabelRegistrationModel> PutLabel([FromForm] LabelRegistrationModel labelRegistrationModel)
        {
            LabelGlobalModel labelGlobalModel = new LabelGlobalModel();
            labelGlobalModel.Id = GenerateUserId(labelRegistrationModel.Username + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
            labelGlobalModel.LabelIcon = labelRegistrationModel.LabelIcon;
            labelGlobalModel.LabelIconUrl = new UserDataAccess().UploadLabelIcon(labelGlobalModel.LabelIcon, labelGlobalModel.Id);
            labelGlobalModel.LabelName = labelRegistrationModel.LabelName;
            labelGlobalModel.Pass = BCrypt.Net.BCrypt.HashPassword(labelRegistrationModel.Pass, BCrypt.Net.BCrypt.GenerateSalt());
            labelGlobalModel.Phone = labelRegistrationModel.Phone;
            labelGlobalModel.Email = labelRegistrationModel.Email;
            labelGlobalModel.Region = labelRegistrationModel.Region;
            labelGlobalModel.Username = labelRegistrationModel.Username;
            labelGlobalModel.EstDate = labelRegistrationModel.EstDate;
            labelGlobalModel.IsEmailVerified = "false";
            labelGlobalModel.IsVerified = "false";
            labelGlobalModel.Type = "label";
            if(new UserDataAccess().RegisterLabel(labelGlobalModel))
            {
                return StatusCode(StatusCodes.Status200OK, "Label Account Created");
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Label Account Creation Failed");
        }
    }
}
