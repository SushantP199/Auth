using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Auth.Api.Models
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid contact number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Gender is not selected.")]
        public string Gender { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 10, ErrorMessage = "Password Should be atleast 10 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords does not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Secret question is not selected")]
        public string SecretQuestion { get; set; }

        [Required(ErrorMessage = "Answer of secret question is required")]
        public string SecretAnswer { get; set; }

        public string RegistrationStatus { get; set; }
    }
}
