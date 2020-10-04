using System.ComponentModel.DataAnnotations;

namespace Yourworktime.Web.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-mail is required")]
        [EmailAddress(ErrorMessage = "Not a valid E-mail address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$" , ErrorMessage = "Password must be at least 8 characters, and include at least one lowercase letter, one uppercase letter, and a number")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please re-type your password")]
        [Compare(nameof(Password), ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Please indicate that you have read and agree to the Terms of Use")]
        public bool AgreeToTerms { get; set; }
    }
}
