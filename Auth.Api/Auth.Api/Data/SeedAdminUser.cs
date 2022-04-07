using Auth.Api.Models;
using Auth.Api.Models.DTO;
using System;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Auth.Api.Models.Register;

namespace Auth.Api.Data
{
    public class SeedAdminUser
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedAdminUser(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task SeedAdminAsync()
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync("9167405114");

                if (userExists == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                    AppUser appUser = new AppUser
                    {
                        UserName = "9167405114",
                        FirstName = "Sushant",
                        LastName = "Pagam",
                        DateOfBirth = DateTime.ParseExact("2000-06-04", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        ContactNumber = "9167405114",
                        Gender = "Male",
                        Password = "Sushant@199",
                        ConfirmPassword = "Sushant@199",
                        SecretQuestion = "Your Pet Name ?",
                        SecretAnswer = "5quad",
                        RegistrationStatus = RegistrationStatus.ACCEPTED,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    await _userManager.CreateAsync(appUser, "Sushant@199");
                    await _userManager.AddToRoleAsync(appUser, UserRoles.Admin);
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
