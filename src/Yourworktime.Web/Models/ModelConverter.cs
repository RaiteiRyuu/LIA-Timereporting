using Yourworktime.Core.Models;

namespace Yourworktime.Web.Models
{
    public static class ModelConverter
    {
        public static UserModel ToUserModel(SignUpModel signUpModel)
        {
            UserModel user = new UserModel()
            {
                FirstName = signUpModel.FirstName,
                LastName = signUpModel.LastName,
                Email = signUpModel.Email,
                Password = signUpModel.Password
            };

            return user;
        }
    }
}
