using System.ComponentModel.DataAnnotations;

namespace Yourworktime.Web.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "E-mail is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool StaySignedIn { get; set; }
    }
}
