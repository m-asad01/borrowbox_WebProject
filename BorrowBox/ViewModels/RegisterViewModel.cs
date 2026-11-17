using System.ComponentModel.DataAnnotations;

namespace BorrowBox.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Please enter your full name (at least 3 letters).")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number.")]
        [RegularExpression(@"^[0-9]{4}-?[0-9]{7}$", ErrorMessage = "Please enter a valid phone number, e.g. 0300-1234567.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please create a password.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please re-enter your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public bool AcceptTerms { get; set; }
    }
}
