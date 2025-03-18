using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebApplication.Authentication;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenManager _jwtTokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jwtTokenManager"></param>
        public TokenController(IJwtTokenManager jwtTokenManager)
        {
            _jwtTokenManager = jwtTokenManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Authenticate")]        
        public IActionResult Authenticate([FromBody] UserCredential credential)
        {
            var token = _jwtTokenManager.Authenticate(credential.Username, credential.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}
