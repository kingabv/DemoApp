using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        public string Authenticate(string username, string password)
        {
            if (!UserData.Users.Any(x => x.Key == username && x.Value == password))
                return null;

            var key = _configuration.GetValue<string>("JwtConfig:Key");
            var issuer = _configuration.GetValue<string>("JwtConfig:Issuer");
            var audience = _configuration.GetValue<string>("JwtConfig:Audience");

            var keyBytes = Encoding.ASCII.GetBytes(key);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                }),
                Expires = DateTime.UtcNow.AddMinutes(60), //configuration.GetValue<int>("Jwt:ExpireMinutes")),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };
         
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
