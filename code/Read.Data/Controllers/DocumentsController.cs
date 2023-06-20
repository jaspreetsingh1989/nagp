using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Read.Data.BLL;
using Read.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Read.Data.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse))]
    [Route("api")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentsBL _documentsBL;
        private readonly IConfiguration _configuration;

        public DocumentsController(IDocumentsBL documentsBL, IConfiguration configuration)
        {
            _documentsBL = documentsBL;
            _configuration = configuration;
        }

        #region Public Methods

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [HttpGet("read-data", Name = "Get Documents")]
        public async Task<ActionResult> GetDocuments()
        {
            ApiResponse response;
            try
            {
                //Process a valid request
                List<UsersDto> users = await _documentsBL.GetDocuments();
                response = new ApiResponse(users, true, users.Any() ? null : new ApiError("Error occurred", "Custom error code"));


                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                response = new ApiResponse(new ApiError(ex.Message, "Bad Request"));
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
