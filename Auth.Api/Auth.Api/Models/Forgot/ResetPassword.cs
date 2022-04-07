using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Models.Forgot
{
    public class ResetPassword
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 10, ErrorMessage = "Password Should be atleast 10 characters long")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords does not match")]
        public string ConfirmNewPassword { get; set; }
    }
}
