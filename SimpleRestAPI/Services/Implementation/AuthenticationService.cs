using Microsoft.IdentityModel.Tokens;
using SimpleRestAPI.App_Data;
using SimpleRestAPI.Models.Authentication;
using SimpleRestAPI.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SimpleRestAPI.Services.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SimpleRestAPIDbContext _context;
        public AuthenticationService(SimpleRestAPIDbContext context)
        {
            _context = context;
        }

        public AuthenticationResponse Authentication(AuthenticationRequest model)
        {
            var user= _context.Users.SingleOrDefault(x=>x.Name==model.Username && x.Password==model.Password);
            if (user == null) return null;
            var token = GenerateJWTToken(user);
            return new AuthenticationResponse(user, token);
        }

        private string GenerateJWTToken(Models.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AUTHSECRET_AUTHSECRET"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("https://localhost:44306",
                "https://localhost:44306",
                null,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
