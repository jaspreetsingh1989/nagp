using Read.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiVersion = Read.Data.Models.ApiVersion;

namespace Read.Data.Controllers
{
    [ApiVersionNeutral]
    [Produces("application/json", "application/xml")]
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        /// <summary>
        /// Returns the Latest and all Versions of API
        /// </summary>
        /// <returns>All version and the Latest version</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ApiVersion>))]
        [HttpGet(Name = "API Version")]
        public ActionResult<ApiVersion> Version()
        {
            //returns the API versions
            ApiVersion version = new ApiVersion();
            version.Latest = "1.0";
            version.Versions = new string[] { "1.0" };
            ApiResponse response = new ApiResponse(version);
            return Ok(response);
        }
    }
}
