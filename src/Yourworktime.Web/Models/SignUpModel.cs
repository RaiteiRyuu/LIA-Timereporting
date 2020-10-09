using System.ComponentModel.DataAnnotations;

namespace Yourworktime.Web.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(26, ErrorMessage = "First name is too long")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(26, ErrorMessage = "Last name is too long")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-mail is required")]
        [MaxLength(254, ErrorMessage = "E-mail is too long")]
        [EmailAddress(ErrorMessage = "Not a valid e-mail address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(128, ErrorMessage = "Password is too long")]
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
