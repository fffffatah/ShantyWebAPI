using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShantyWebAPI.Models;
using ShantyWebAPI.Models.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShantyWebAPI.Controllers.Search
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        [Route("search/all")]
        public ActionResult<SearchResultModel> SearchResult([Required][FromHeader] string jwtToken, [Required] string query)
        {
            string userId = new SearchDataAccess().JwtTokenValidation(jwtToken);
            if (userId == "")
            {
                return Unauthorized(new CustomResponseModel() { Code = "401", Phrase = "Unauthorized", Message = "Invalid Jwt Token" });
            }
            SearchResultModel searchResultModels = new SearchDataAccess().SearchResult(query);
            if (searchResultModels != null)
            {
                return searchResultModels;
            }
            return BadRequest(new CustomResponseModel() { Code = "400", Phrase = "BadRequest", Message = "Search Failed" });
        }
    }
}
