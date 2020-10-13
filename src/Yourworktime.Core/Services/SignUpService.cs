using Microsoft.Extensions.Configuration;
using Yourworktime.Core.Models;
using System;
using System.Threading.Tasks;

namespace Yourworktime.Core.Services
{
    public class SignUpService
    {
        private readonly IConfiguration configuration;
        private readonly UserService userService;

        public SignUpService(UserService userService, IConfiguration configuration)
        {
            this.userService = userService;
            this.configuration = configuration;
        }

        public async Task<SignUpResult> SignUp(UserModel model)
        {
            CleanUpUserModel(model);

            if (await userService.CountUsersByField("Email", model.Email) > 0)
                return new SignUpResult(false, new string[] { "An account with this E-mail already exists" });

            string salt = Utils.GetSalt(16);
            string hashedPassword = Utils.ComputeSha256Hash(string.Concat(model.Password, salt));

            DateTime dateNow = DateTime.UtcNow;

            UserModel newUser = new UserModel()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = $"{model.FirstName} {model.LastName}",
                Email = model.Email,
                RegisteredDate = dateNow,
                Salt = salt,
                Password = hashedPassword,
                Role = "User"

            };
            await userService.InsertUser(newUser);

            return new SignUpResult(true, new string[0]);
        }

        private void CleanUpUserModel(UserModel model)
        {
            model.FirstName = model.FirstName.UppercaseFirst().Trim();
            model.LastName = model.LastName.UppercaseFirst().Trim();
            model.Email = model.Email.ToLower().Trim();
        }
    }
}
