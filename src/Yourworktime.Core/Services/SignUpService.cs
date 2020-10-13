using Microsoft.Extensions.Configuration;
using MongoServer.Core.Models;
using System;
using System.Linq;
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
                return new SignUpResult(false, new string[] { "An account with this E-mail already exists" }, null);

            string salt = Utils.GetSalt(16);
            string hashedPassword = Utils.ComputeSha256Hash(string.Concat(model.Password, salt));
            
            DateTime dateNow = DateTime.UtcNow;

            Guid id = Guid.NewGuid();
            string profileImagePath = $"W_{dateNow:yyyyMMddhhmm}_{id}.png";
            UserModel newUser = new UserModel()
            {
                Id = id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = $"{model.FirstName} {model.LastName}",
                Email = model.Email,
                RegisteredDate = dateNow,
                Salt = salt,
                Password = hashedPassword,
                ProfileImagePath = profileImagePath,
                Role = "User"

            };
            await userService.InsertUser(newUser);

            Utils.CreateAndSaveProfileImage($"{newUser.FirstName.First()}W", 100, 100, $"../../data/profilePics/{profileImagePath}");
            return new SignUpResult(true, new string[0], newUser);
        }

        private void CleanUpUserModel(UserModel model)
        {
            model.FirstName = model.FirstName.UppercaseFirst().Trim();
            model.LastName = model.LastName.UppercaseFirst().Trim();
            model.Email = model.Email.ToLower().Trim();
        }
    }
}
