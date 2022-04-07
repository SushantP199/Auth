using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Models.Forgot
{
    public class ForgotUsername
    {
        [Required(ErrorMessage = "Secret question is not selected")]
        public string SecretQuestion { get; set; }

        [Required(ErrorMessage = "Answer of secret question is required")]
        public string SecretAnswer { get; set; }
    }
}
