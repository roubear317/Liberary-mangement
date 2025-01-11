using libraryManagment.Core.Model;
using libraryManagment.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace libraryManagment.EF.Repos
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(IConfiguration config , UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        }
        public async Task<string> GetTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim> {

          // new Claim(JwtRegisteredClaimNames.GivenName,user.FullName),
           new Claim(JwtRegisteredClaimNames.Email,user.Email)


           };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); 
            }
           

            var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha256);


            var discriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _config["Jwt:Issuer"],
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
            };

            var tokenhandler = new JwtSecurityTokenHandler();

            var token = tokenhandler.CreateToken(discriptor);

            return  tokenhandler.WriteToken(token);   




        }
    }
}
