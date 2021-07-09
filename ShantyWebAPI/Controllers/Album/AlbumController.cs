using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.Models.Album;
using ShantyWebAPI.Models;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ShantyWebAPI.Controllers.Album
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        //COMMON METHODS
        string GenerateUserId()
        {
            string input = Guid.NewGuid().ToString();
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

        //CREATE ALBUM
        [HttpPost]
        [Route("create/album")]
        public ActionResult<CustomResponseModel> CreateAlbum([FromForm]AlbumCreateModel albumCreateModel)
        {
            albumCreateModel.LabelId = new AlbumDataAccess().JwtTokenValidation(albumCreateModel.JwtToken);
            if (albumCreateModel.LabelId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if(new AlbumDataAccess().IsLabel(albumCreateModel.LabelId))
            {
                AlbumGlobalModel albumGlobalModel = new AlbumGlobalModel();
                albumGlobalModel.Id = GenerateUserId();
                albumGlobalModel.AlbumName = albumCreateModel.AlbumName;
                albumGlobalModel.CoverImage = albumCreateModel.CoverImage;
                albumGlobalModel.CoverImageUrl = new AlbumDataAccess().UploadAlbumCoverImage(albumCreateModel.CoverImage, albumGlobalModel.Id);
                albumGlobalModel.LabelId = albumCreateModel.LabelId;
                albumGlobalModel.ArtistId = albumCreateModel.ArtistId;
                albumGlobalModel.Year = albumCreateModel.Year;
                albumGlobalModel.Genre = albumCreateModel.Genre;
                if (new AlbumDataAccess().CreateAlbum(albumGlobalModel))
                {
                    return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Album Created" });
                }
            }
            else
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Album Creator Must be a Label" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Album Creation Failed" });
        }

        //UPDATE ALBUM
        [HttpPost]
        [Route("update/album")]
        public ActionResult<CustomResponseModel> UpdateAlbum([FromForm]AlbumUpdateModel albumUpdateModel, [Required] string albumId)
        {
            albumUpdateModel.LabelId = new AlbumDataAccess().JwtTokenValidation(albumUpdateModel.JwtToken);
            albumUpdateModel.Id = albumId;
            if (albumUpdateModel.LabelId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new AlbumDataAccess().IsLabel(albumUpdateModel.LabelId))
            {
                if (new AlbumDataAccess().UpdateAlbum(albumUpdateModel))
                {
                    return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Album Updated" });
                }
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Album Updation Failed" });
        }

        //GET ALBUM
        [HttpGet]
        [Route("get/album")]
        public ActionResult<AlbumGetModel> GetAlbum([FromHeader][Required] string jwtToken, [Required] string albumId)
        {
            string userId = new AlbumDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            AlbumGetModel albumGetModel = new AlbumDataAccess().GetAlbum(albumId);
            if (albumGetModel != null)
            {
                return albumGetModel;
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Could Not Get Album" });
        }

        //DELETE ALBUM
        [HttpGet]
        [Route("delete/album")]
        public ActionResult<AlbumGetModel> DeleteAlbum([FromHeader][Required] string jwtToken, [Required] string albumId)
        {
            string labelId = new AlbumDataAccess().JwtTokenValidation(jwtToken);
            if (labelId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new AlbumDataAccess().IsLabel(labelId))
            {
                if (new AlbumDataAccess().DeleteAlbum(labelId,albumId))
                {
                    return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Album Delete" });
                }
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Could Not Delete Album" });
        }
    }
}
