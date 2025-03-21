using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TestWebApplication.Authentication;
using TestWebApplication.Exceptions;
using TestWebApplication.Extensions;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly ILogger<TokenController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtTokenManager"></param>
        /// <param name="logger"></param>
        public TokenController(IJwtTokenManager jwtTokenManager, ILogger<TokenController> logger)
        {
            _jwtTokenManager = jwtTokenManager;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, description: "Request succeeded")]
        [HttpPost("Authenticate")]        
        public IActionResult Authenticate([FromBody][Required] UserCredential credential)
        {
            try
            {
                if (credential == null)
                    return this.LogAndReturnBadRequest(_logger,
                        new ErrorModel(null, ErrorCode.Validation, "Not valid userData."));
                var token = _jwtTokenManager.Authenticate(credential.Username, credential.Password);
                return Ok(token);
            }
            catch (TokenException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCode.Validation:
                        return this.LogAndReturnBadRequest(_logger,
                            new ErrorModel(ex, ErrorCode.Validation, $"Bad Request:{ex.Message}"));                   
                    case ErrorCode.NotFound:
                        return this.LogAndReturnNotFound(_logger,
                            new ErrorModel(ex, ErrorCode.Conflict, $"Not found: {ex.Message}"));
                    default:
                        return this.LogAndReturnInternalServerError(_logger, ex,
                            new ErrorModel(ex, ErrorCode.InternalServerError, "Internal Server Error"));
                }
            }                       
        }
    }
}
