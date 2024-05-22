using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthAPI.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private JwtOptions _jwtOptions;
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(ApplicationUser user)
        {
            try
            {
                var tokenHanler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

                var clamList = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName)
            };

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Audience = _jwtOptions.Audience,
                    Issuer = _jwtOptions.Issuer,
                    Subject = new ClaimsIdentity(clamList),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHanler.CreateToken(tokenDescriptor);

                return tokenHanler.WriteToken(token);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
