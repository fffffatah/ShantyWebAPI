using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShantyWebAPI.Models.Album;
using ShantyWebAPI.Models;
using System.Text;

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
        public ActionResult<AlbumCreateModel> CreateAlbum([FromForm]AlbumCreateModel albumCreateModel)
        {
            AlbumGlobalModel albumGlobalModel = new AlbumGlobalModel();
            albumGlobalModel.Id = GenerateUserId();
            albumGlobalModel.AlbumName = albumCreateModel.AlbumName;
            albumGlobalModel.CoverImage = albumCreateModel.CoverImage;
            albumGlobalModel.CoverImageUrl = new AlbumDataAccess().UploadAlbumCoverImage(albumCreateModel.CoverImage, albumGlobalModel.Id);
            albumGlobalModel.LabelId = albumCreateModel.LabelId;
            albumGlobalModel.ArtistId = albumCreateModel.ArtistId;
            albumGlobalModel.Year = albumCreateModel.Year;
            if(new AlbumDataAccess().CreateAlbum(albumGlobalModel))
            {
                return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Album Created" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Album Creation Failed" });
        }
    }
}
