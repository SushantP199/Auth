using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Models.Forgot
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Secret question is not selected")]
        public string SecretQuestion { get; set; }

        [Required(ErrorMessage = "Answer of secret question is required")]
        public string SecretAnswer { get; set; }
    }
}
