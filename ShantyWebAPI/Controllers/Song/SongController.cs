using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShantyWebAPI.Models;
using ShantyWebAPI.Models.Song;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShantyWebAPI.Controllers.Song
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
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

        //UPLOAD SONG
        [HttpPost]
        [Route("upload/song")]
        public ActionResult<CustomResponseModel> UploadSong([FromForm] SongUploadModel songUploadModel)
        {
            string labelId = new SongDataAccess().JwtTokenValidation(songUploadModel.JwtToken);
            if (labelId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new SongDataAccess().IsLabel(labelId))
            {
                SongGlobalModel songGlobalModel = new SongGlobalModel();
                songGlobalModel.Id = GenerateUserId();
                songGlobalModel.SongName = songUploadModel.SongName;
                songGlobalModel.SongFile = songUploadModel.SongFile;
                songGlobalModel.SongFileUrl = new SongDataAccess().UploadAudioFile(songUploadModel.SongFile, songGlobalModel.Id);
                songGlobalModel.ArtistName = songUploadModel.ArtistName;
                songGlobalModel.Genre = songUploadModel.Genre;
                songGlobalModel.AlbumId = songUploadModel.AlbumId;
                songGlobalModel.TimesStreamed = 0;
                if (new SongDataAccess().UploadSong(songGlobalModel))
                {
                    return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Song Uploaded" });
                }
            }
            else
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Song Uploader Must be a Label" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Song Upload Failed" });
        }

        //UPDATE SONG
        [HttpPost]
        [Route("update/song")]
        public ActionResult<CustomResponseModel> UpdateSong([FromForm] SongUpdateModel songUpdateModel, [Required]string songId)
        {
            string labelId = new SongDataAccess().JwtTokenValidation(songUpdateModel.JwtToken);
            if (labelId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new SongDataAccess().IsLabel(labelId))
            {
                SongGlobalModel songGlobalModel = new SongGlobalModel();
                songGlobalModel.Id = songId;
                songGlobalModel.SongName = songUpdateModel.SongName;
                songGlobalModel.SongFile = songUpdateModel.SongFile;
                songGlobalModel.SongFileUrl = new SongDataAccess().UploadAudioFile(songUpdateModel.SongFile, songGlobalModel.Id);
                songGlobalModel.ArtistName = songUpdateModel.ArtistName;
                songGlobalModel.Genre = songUpdateModel.Genre;
                songGlobalModel.AlbumId = songUpdateModel.AlbumId;
                if (new SongDataAccess().UpdateSong(songGlobalModel))
                {
                    return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Song Updated" });
                }
            }
            else
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Song Updater Must be a Label" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Song Update Failed" });
        }

        //GET SONG BY ID
        [HttpGet]
        [Route("get/song")]
        public ActionResult<SongGetModel> GetSong([FromHeader][Required] string jwtToken, [Required] string songId)
        {
            string userId = new SongDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            SongGetModel songGetModel = new SongDataAccess().GetSong(songId);
            if (songGetModel != null)
            {
                return songGetModel;
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Could Not Get Song" });
        }

        //GET SONG BY ALBUM (LIST OF SONGS)
        [HttpGet]
        [Route("get/song/album")]
        public ActionResult<List<SongGetModel>> GetSongsAlbum([FromHeader][Required] string jwtToken, [Required] string albumId)
        {
            string userId = new SongDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            List<SongGetModel> songGetModels = new SongDataAccess().GetSongsAlbum(albumId);
            if (songGetModels != null)
            {
                return songGetModels;
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Could Not Get Song" });
        }

        //DELETE SONG
        [HttpGet]
        [Route("delete/song")]
        public ActionResult<CustomResponseModel> DeleteAlbum([FromHeader][Required] string jwtToken, [Required] string songId)
        {
            string labelId = new SongDataAccess().JwtTokenValidation(jwtToken);
            if (labelId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new SongDataAccess().IsLabel(labelId))
            {
                if (new SongDataAccess().DeleteSong(songId))
                {
                    return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Song Deleted" });
                }
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Could Not Delete Song" });
        }
    }
}
