using BookApi.Model;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Shirley.Book.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.AuthServices
{
    public class AuthService : IAuthService, ILoginService, ILogoutService
    {
        public Task<AuthenticateResult> Authentication(UserInfo userInfo)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticateResult> Login(string userName, string password)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                issuer: Const.Domain,
                audience: Const.Domain,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Task.FromResult(new AuthenticateResult
            {
                LoginSuccess = true,
                Message = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }
    }
}
