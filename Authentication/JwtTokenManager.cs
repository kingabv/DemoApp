using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestWebApplication.Exceptions;
using TestWebApplication.Models;

namespace TestWebApplication.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public JwtTokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string? Authenticate(string username, string password)
        {            
            var usersFromJson = _configuration.GetValue<string>("UserData");
            if (usersFromJson == null)
                throw new TokenException(ErrorCode.NotFound, "User data not found in the appSettings.json file");

            IList<UserCredential>? users = JsonConvert.DeserializeObject<IList<UserCredential>>(usersFromJson);

            if (users == null || !users.Any(x => x.Username == username && x.Password == password))
                throw new TokenException(ErrorCode.NotFound, "The given User data is not authorized for token request.");

            var key = _configuration.GetValue<string>("JwtConfig:Key");
            var issuer = _configuration.GetValue<string>("JwtConfig:Issuer");
            var audience = _configuration.GetValue<string>("JwtConfig:Audience");
            var minutes = _configuration.GetValue<int>("Jwt:ExpireMinutes");

            var keyBytes = Encoding.ASCII.GetBytes(key);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
          
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, username),
                ]),
                Expires = minutes == 0 ? DateTime.UtcNow.AddMinutes(60) : DateTime.UtcNow.AddMinutes(minutes), 
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };
         
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
