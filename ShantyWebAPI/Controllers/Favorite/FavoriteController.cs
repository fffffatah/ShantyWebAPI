using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShantyWebAPI.Models;
using ShantyWebAPI.Models.Favorite;
using ShantyWebAPI.Models.Recommended;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Controllers.Favorite
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        [HttpGet]
        [Route("get/favorite")]
        public ActionResult<FavoriteModel> GetRecommended([Required][FromHeader] string jwtToken)
        {
            string userId = new FavoriteDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            FavoriteModel favorite = new FavoriteDataAccess().GetFavorite(userId);
            if (favorite != null || favorite.SongGetModels.Count != 0)
            {
                return favorite;
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Favorite Fetch Failed" });
        }
    }
}
