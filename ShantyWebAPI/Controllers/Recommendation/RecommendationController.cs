using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShantyWebAPI.Models;
using ShantyWebAPI.Models.Chart;
using ShantyWebAPI.Models.Recommended;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Controllers.Recommendation
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        [HttpGet]
        [Route("get/recommended")]
        public ActionResult<RecommendedModel> GetRecommended([Required][FromHeader] string jwtToken)
        {
            string userId = new RecommendationDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            RecommendedModel recommendedModel = new RecommendationDataAccess().GetRecommended(userId);
            if (recommendedModel != null)
            {
                return recommendedModel;
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Recommended Fetch Failed" });
        }

        [HttpGet]
        [Route("get/globaltop")]
        public ActionResult<GlobalTopModel> GetGlobalTop([Required][FromHeader] string jwtToken)
        {
            string userId = new RecommendationDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            GlobalTopModel globalTopModel = new RecommendationDataAccess().GetGlobalTop(userId);
            if (globalTopModel != null)
            {
                return globalTopModel;
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Charts Fetch Failed" });
        }
    }
}
