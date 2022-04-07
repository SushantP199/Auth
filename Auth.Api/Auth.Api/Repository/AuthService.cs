using Auth.Api.Models;
using Auth.Api.Models.Register;
using Auth.Api.Models.Login;
using Auth.Api.Models.Forgot;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Auth.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Api.Models.DTO;
using System.Linq;

namespace Auth.Api.Repository
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AuthDbContext _authDbContext;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AuthDbContext authDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _authDbContext = authDbContext;
        }

        public async Task<Response> Register(MemberRegister memberRegister)
        {
            Response response = new Response();

            try
            {
                var userExists = await _userManager.FindByNameAsync(memberRegister.ContactNumber);


                if (userExists != null)
                {

                    if (userExists.RegistrationStatus == RegistrationStatus.DENIED)
                    {
                        response.Status = RegistrationStatus.DENIED;
                        response.Message = "You cannot register again as your previous registration request with this credentials is denied by admin";

                        return response;
                    }

                    response.Status = "ERROR";
                    response.Message = "User already exists";

                    return response;
                }
                else
                {

                    AppUser appUser = new AppUser
                    {
                        UserName = memberRegister.ContactNumber,
                        FirstName = memberRegister.FirstName,
                        LastName = memberRegister.LastName,
                        DateOfBirth = memberRegister.DateOfBirth,
                        ContactNumber = memberRegister.ContactNumber,
                        Gender = memberRegister.Gender,
                        Password = memberRegister.Password,
                        ConfirmPassword = memberRegister.ConfirmPassword,
                        SecretQuestion = memberRegister.SecretQuestion,
                        SecretAnswer = memberRegister.SecretAnswer,
                        RegistrationStatus = RegistrationStatus.INPROGRESS,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    var register = await _userManager.CreateAsync(appUser, memberRegister.Password);

                    if (register.Succeeded)
                    {
                        if (!await _roleManager.RoleExistsAsync(UserRoles.Member))
                            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Member));

                        if (await _roleManager.RoleExistsAsync(UserRoles.Member))
                            await _userManager.AddToRoleAsync(appUser, UserRoles.Member);

                        Member member = new Member();

                        member.MemberId = "MEMID" + new Random().Next(100000, 900000).ToString();
                        member.AppUserId = appUser.Id;
                        member.AppUser = appUser;

                        await _authDbContext.Members.AddAsync(member);
                        await _authDbContext.SaveChangesAsync();

                        response.Status = "SUCCESS";
                        response.Message = "Your registration is completed!";

                        return response;
                    }
                    else
                    {
                        response.Status = "ERROR";
                        response.Message = "Your registration is failed!";

                        return response;
                    }
                }
            }
            catch (System.Exception e)
            {
                response.Status = "ERROR";
                response.Message = e.Message;

                return response;
            }
        }

        public async Task<Response> Login(UserLogin userLogin)
        {
            Response response = new Response();

            try
            {
                var user = await _userManager.FindByNameAsync(userLogin.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, userLogin.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    if (userRoles.First() == UserRoles.Member)
                    {
                        if (user.RegistrationStatus == RegistrationStatus.DENIED)
                        {
                            response.Status = RegistrationStatus.DENIED;
                            response.Message = "You cannot login with your credentials as your registration request is denied by admin";

                            return response;
                        }

                        if (user.RegistrationStatus == RegistrationStatus.INPROGRESS)
                        {
                            response.Status = RegistrationStatus.INPROGRESS;
                            response.Message = "You can login with your credentials if and only if your registration request is accepted by admin";

                            return response;
                        }
                    }

                    var authClaims = new List<Claim> {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    foreach (var userRole in userRoles)
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                    var authSigingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddDays(1.0),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigingKey, SecurityAlgorithms.HmacSha256)
                    );

                    response.Status = "SUCCESS";
                    response.Message = "You have logged in sucessfully!";

                    AuthData authData = new AuthData();
                    authData.JWT = new JwtSecurityTokenHandler().WriteToken(token);
                    authData.UserRole = userRoles.First();

                    if (authData.UserRole == UserRoles.Member)
                    {
                        var member = _authDbContext.Members.Where(m => m.AppUserId == user.Id).First();

                        authData.MemberId = member.MemberId;
                    }

                    authData.UserFullName = String.Concat(user.FirstName, " ", user.LastName);
                    authData.Expiration = token.ValidTo;

                    response.AuthData = authData;

                    return response;
                }

                response.Status = "ERROR";
                response.Message = "You are unauthorized user";

                return response;
            }
            catch (Exception e)
            {
                response.Status = "ERROR";
                response.Message = e.Message;

                return response;
            }
        }

        public Task ForgotPassword(ForgotPassword forgotPassword)
        {
            throw new System.NotImplementedException();
        }

        public Task ForgotUsername(ForgotUsername forgotUsername)
        {
            throw new System.NotImplementedException();
        }





        public Task ResetPassword(ResetPassword resetPassword)
        {
            throw new System.NotImplementedException();
        }

        
    }
}
