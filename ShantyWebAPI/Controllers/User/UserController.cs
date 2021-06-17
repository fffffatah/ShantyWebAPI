using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.Models.User;
using ShantyWebAPI.Models;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

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
                new UserDataAccess().SendVerificationEmail(listenerGlobalModel.FirstName + " " + listenerGlobalModel.LastName, listenerGlobalModel.Email, listenerGlobalModel.Id);
                return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Listener Account Created" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Listener Account Creation Failed" });
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
                new UserDataAccess().SendVerificationEmail(labelGlobalModel.LabelName, labelGlobalModel.Email, labelGlobalModel.Id);
                return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Label Account Created" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Label Account Creation Failed" });
        }
        [HttpPost]
        [Route("register/artist")]
        public ActionResult<ArtistRegistrationModel> PutArtist([FromForm] ArtistRegistrationModel artistRegistrationModel)
        {
            ArtistGlobalModel artistGlobalModel = new ArtistGlobalModel();
            artistGlobalModel.Id = GenerateUserId(artistRegistrationModel.Username + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
            artistGlobalModel.ProfileImage = artistRegistrationModel.ProfileImage;
            artistGlobalModel.ProfileImageUrl = new UserDataAccess().UploadProfileImage(artistGlobalModel.ProfileImage, artistGlobalModel.Id);
            artistGlobalModel.Username = artistRegistrationModel.Username;
            artistGlobalModel.FirstName = artistRegistrationModel.FirstName;
            artistGlobalModel.LastName = artistRegistrationModel.LastName;
            artistGlobalModel.Phone = artistRegistrationModel.Phone;
            artistGlobalModel.Email = artistRegistrationModel.Email;
            artistGlobalModel.Region = artistRegistrationModel.Region;
            artistGlobalModel.Dob = artistRegistrationModel.Dob;
            artistGlobalModel.Pass = BCrypt.Net.BCrypt.HashPassword(artistRegistrationModel.Pass, BCrypt.Net.BCrypt.GenerateSalt());
            artistGlobalModel.IsEmailVerified = "false";
            artistGlobalModel.IsVerified = "false";
            artistGlobalModel.Type = "artist";
            artistGlobalModel.LabelId = artistRegistrationModel.LabelId;
            if(new UserDataAccess().RegisterArtist(artistGlobalModel))
            {
                new UserDataAccess().SendVerificationEmail(artistGlobalModel.FirstName + " " + artistGlobalModel.LastName, artistGlobalModel.Email, artistGlobalModel.Id);
                return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Artist Account Created" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Artist Account Creation Failed" });
        }

        //VERIFY EMAIL
        [HttpGet]
        [Route("email/verify")]
        public ActionResult VerifyEmail(string id)
        {
            if(new UserDataAccess().VerifyEmail(id))
            {
                return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Email Verified" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Email Verfication Failed" });
        }

        //RESET OR CHANGE PASSWORD
        [HttpPost]
        [Route("change/password")]
        public ActionResult<ChangePasswordModel> ChangePassword([FromForm] ChangePasswordModel changePasswordModel)
        {
            changePasswordModel.NewPass = BCrypt.Net.BCrypt.HashPassword(changePasswordModel.NewPass, BCrypt.Net.BCrypt.GenerateSalt());
            string id = new UserDataAccess().JwtTokenValidation(changePasswordModel.JwtToken);
            if (id == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new UserDataAccess().ResetChangePassword(id, changePasswordModel.NewPass))
            {
                return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Password Changed Successfully" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Couldn't Change Password" });
        }
        [HttpPost]
        [Route("send/otp")]
        public ActionResult<SendOtpModel> SendOtp([FromForm] SendOtpModel sendOtpModel)
        {
            string jwtToken = new UserDataAccess().SendOtpForPassReset(sendOtpModel.Otp, sendOtpModel.Email);
            if (jwtToken == "Email Not Verified")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = jwtToken });
            }
            else if (jwtToken == "Email Not Found")
            {
                return NotFound(new CustomResponseModel() { Code = "404", Phrase = "NotFound", Message = jwtToken });
            }
            else if(jwtToken != "")
            {
                return Ok(new { message = "OTP Sent", token = jwtToken });
            }
            return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Unauthorized" });
        }
        [HttpPost]
        [Route("reset/password")]
        public ActionResult<ResetPasswordModel> ResetPassword([FromForm] ResetPasswordModel resetPasswordModel)
        {
            resetPasswordModel.NewPass = BCrypt.Net.BCrypt.HashPassword(resetPasswordModel.NewPass, BCrypt.Net.BCrypt.GenerateSalt());
            string id = new UserDataAccess().JwtTokenValidation(resetPasswordModel.JwtToken);
            if (id == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if(new UserDataAccess().ResetChangePassword(id, resetPasswordModel.NewPass))
            {
                return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Password Reset Successful" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Password Reset Failed"});
        }

        //UPDATE USER DATA
        [HttpPost]
        [Route("update/artist")]
        public ActionResult<ArtistUpdateModel> UpdateArtist([FromForm] ArtistUpdateModel artistUpdateModel)
        {
            artistUpdateModel.Id = new UserDataAccess().JwtTokenValidation(artistUpdateModel.JwtToken);
            if (artistUpdateModel.Id == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if(new UserDataAccess().UpdateArtist(artistUpdateModel))
            {
                return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Artist Update Successful" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Artist Update Failed" });
        }
        [HttpPost]
        [Route("update/listener")]
        public ActionResult<ArtistUpdateModel> UpdateListener([FromForm] ListenerUpdateModel listenerUpdateModel)
        {
            listenerUpdateModel.Id = new UserDataAccess().JwtTokenValidation(listenerUpdateModel.JwtToken);
            if (listenerUpdateModel.Id == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new UserDataAccess().UpdateListener(listenerUpdateModel))
            {
                return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Listener Update Successful" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Listener Update Failed" });
        }
        [HttpPost]
        [Route("update/label")]
        public ActionResult<ArtistUpdateModel> UpdateLabel([FromForm] LabelUpdateModel labelUpdateModel)
        {
            labelUpdateModel.Id = new UserDataAccess().JwtTokenValidation(labelUpdateModel.JwtToken);
            if (labelUpdateModel.Id == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new UserDataAccess().UpdateLabel(labelUpdateModel))
            {
                return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Label Update Successful" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Label Update Failed" });
        }

        //GET USERS
        [HttpGet]
        [Route("get/artist")]
        public ActionResult<ArtistGetInfoModel> GetArtistInfo([FromHeader][Required] string jwtToken)
        {
            string id = new UserDataAccess().JwtTokenValidation(jwtToken);
            if (id == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            ArtistGetInfoModel artistGetInfoModel = new UserDataAccess().GetArtistInfo(id);
            //todo
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Failed To Get Artist" });
        }
        [HttpGet]
        [Route("get/listener")]
        public ActionResult<ListenerGetInfoModel> GetListenerInfo([FromHeader][Required] string jwtToken)
        {
            string id = new UserDataAccess().JwtTokenValidation(jwtToken);
            if (id == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            ListenerGetInfoModel listenerGetInfoModel = new UserDataAccess().GetListenerInfo(id);
            //todo
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Failed To Get Listener" });
        }
        [HttpGet]
        [Route("get/label")]
        public ActionResult<LabelGetInfoModel> GetLabelInfo([FromHeader][Required] string jwtToken)
        {
            string id = new UserDataAccess().JwtTokenValidation(jwtToken);
            if (id == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            LabelGetInfoModel labelGetInfoModel = new UserDataAccess().GetLabelInfo(id);
            //todo
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Failed To Get Label" });
        }

        //LOGIN USERS
        [HttpPost]
        [Route("login")]
        public ActionResult<UserLoginModel> Login([FromForm] UserLoginModel userLoginModel)
        {
            string jwtToken = new UserDataAccess().LoginUser(userLoginModel.Email, userLoginModel.Pass);
            if(jwtToken == "Email Not Verified")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = jwtToken });
            }
            else if (jwtToken != "")
            {
                return Ok(new { message = "Logged in Successfully", token = jwtToken });
            }
            return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Email/Password" });
        }
    }
}
