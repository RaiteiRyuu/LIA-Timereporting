using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using MongoServer.Core.Models;
using System.Threading.Tasks;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Yourworktime.Core.Services
{
    public class SigninService
    {
        private IConfiguration configuration;
        private UserService userService;

        public SigninService(UserService userService, IConfiguration configuration)
        {
            this.userService = userService;
            this.configuration = configuration;
        }

        public async Task<SigninResult> SignIn(string email, string password)
        {
            email = email.ToLower();

            UserModel user = await AuthenticateUser(email, password);

            if (user != null)
            {
                string token = GenerateJSONWebToken(user);
                return new SigninResult(true, new string[0], token);
            }

            return new SigninResult(false, new string[] { "Wrong e-mail or password" }, null);
        }

        private async Task<UserModel> AuthenticateUser(string email, string password)
        {
            List<UserModel> models = await userService.LoadUsersByField("Email", email);
            if (models.Count == 0)
                return null;

            UserModel user = models.First();

            string hashedPassword = Utils.ComputeSha256Hash(string.Concat(password, user.Salt));
            if(!user.Password.Equals(hashedPassword))
                return null;

            return user;
        }

        private string GenerateJSONWebToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(Convert.ToInt32(configuration["Jwt:ExpiryInDays"])),
                signingCredentials: credentials);
            
            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }
    }
}
