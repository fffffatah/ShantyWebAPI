using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShantyWebAPI.Models;
using ShantyWebAPI.Models.Playlist;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShantyWebAPI.Controllers.Playlist
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
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

        //CREATE PLAYLIST
        [HttpPost]
        [Route("create/playlist")]
        public ActionResult<CustomResponseModel> CreatePlaylist([FromForm] PlaylistCreateModel playlistCreateModel)
        {
            playlistCreateModel.CreatorId = new PlaylistDataAccess().JwtTokenValidation(playlistCreateModel.JwtToken);
            if (playlistCreateModel.CreatorId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new PlaylistDataAccess().IsListenerOrArtist(playlistCreateModel.CreatorId))
            {
                PlaylistGlobalModel playlistGlobalModel = new PlaylistGlobalModel();
                playlistGlobalModel.PlaylistId = GenerateUserId();
                playlistGlobalModel.PlaylistName = playlistCreateModel.PlaylistName;
                playlistGlobalModel.PlaylistImage = playlistCreateModel.PlaylistImage;
                playlistGlobalModel.PlaylistImageUrl = new PlaylistDataAccess().UploadPlaylistCoverImage(playlistCreateModel.PlaylistImage, playlistGlobalModel.PlaylistId);
                playlistGlobalModel.CreatorId = playlistCreateModel.CreatorId;
                if (new PlaylistDataAccess().CreatePlaylist(playlistGlobalModel))
                {
                    return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Playlist Created" });
                }
            }
            else
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Playlist Creator Must be a Listener or Artist" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Playlist Creation Failed" });
        }
        //DELETE PLAYLIST
        [HttpGet]
        [Route("delete/playlist")]
        public ActionResult<CustomResponseModel> DeleteAlbum([FromHeader][Required] string jwtToken, [Required] string playlistId)
        {
            string userId = new PlaylistDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new PlaylistDataAccess().IsListenerOrArtist(userId))
            {
                if (new PlaylistDataAccess().DeletePlaylist(userId, playlistId))
                {
                    return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Playlist Deleted" });
                }
            }
            else
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "User Must be a Listener or Artist" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Could Not Delete Playlist" });
        }
        //ADD SONG TO PLAYLIST
        [HttpGet]
        [Route("add/playlist/song")]
        public ActionResult<CustomResponseModel> AddSongPlaylist([FromHeader][Required] string jwtToken, [Required] string playlistId, [Required] string songId)
        {
            string userId = new PlaylistDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new PlaylistDataAccess().IsListenerOrArtist(userId))
            {
                if(new PlaylistDataAccess().AddSongPlaylist(userId, playlistId, songId))
                {
                    return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Song Added to Playlist" });
                }
            }
            else
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "User Must be a Listener or Artist" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Could Not Add Song to Playlist" });
        }
        //REMOVE SONG FROM PLAYLIST
        [HttpGet]
        [Route("remove/playlist/song")]
        public ActionResult<CustomResponseModel> RemoveSongPlaylist([FromHeader][Required] string jwtToken, [Required] string playlistId, [Required] string songId)
        {
            string userId = new PlaylistDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            if (new PlaylistDataAccess().IsListenerOrArtist(userId))
            {
                if (new PlaylistDataAccess().RemoveSongPlaylist(userId, playlistId, songId))
                {
                    return Ok(new CustomResponseModel() { Code = "200", Phrase = "OK", Message = "Song Removed from Playlist" });
                }
            }
            else
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "User Must be a Listener or Artist" });
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Could Not Remove Song from Playlist" });
        }
        //GET PLAYLIST
        [HttpGet]
        [Route("get/playlist")]
        public ActionResult<PlaylistGetModel> GetPlaylist([FromHeader][Required] string jwtToken, [Required] string playlistId)
        {
            string userId = new PlaylistDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            PlaylistGetModel playlistGetModel = null;
            if (new PlaylistDataAccess().IsListenerOrArtist(userId))
            {
                playlistGetModel = new PlaylistDataAccess().GetPlaylist(playlistId);
            }
            else
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "User Must be a Listener or Artist" });
            }
            if (playlistGetModel != null)
            {
                return playlistGetModel;
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Could Not Get Playlist" });
        }
        //GET ALL PLAYLIST
        [HttpGet]
        [Route("get/playlist/all")]
        public ActionResult<List<PlaylistGetModel>> GetAllPlaylist([FromHeader][Required] string jwtToken)
        {
            string creatorId = new PlaylistDataAccess().JwtTokenValidation(jwtToken);
            if (creatorId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            List<PlaylistGetModel> playlistGetModels = null;
            if (new PlaylistDataAccess().IsListenerOrArtist(creatorId))
            {
                playlistGetModels = new PlaylistDataAccess().GetAllPlaylist(creatorId);
            }
            else
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "User Must be a Listener or Artist" });
            }
            if (playlistGetModels != null)
            {
                return playlistGetModels;
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Could Not Get All Playlist" });
        }
    }
}
