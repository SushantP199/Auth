using Auth.Api.Models.DTO;
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
using System.Linq;

namespace Auth.Api.Repository
{
    public class MembersService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthDbContext _authDbContext;

        public MembersService(UserManager<AppUser> userManager, AuthDbContext authDbContext)
        {
            _userManager = userManager;
            _authDbContext = authDbContext;
        }

        public List<MemberRegister> GetNewRegistrations()
        {
            List<MemberRegister> newRegistrations = new List<MemberRegister>();

            try
            {
                List<AppUser> newAppUsers = _authDbContext.Users.Where(member => member.RegistrationStatus == RegistrationStatus.INPROGRESS).ToList();

                List<Member> newMembers = _authDbContext.Members.Where(member => member.AppUser.RegistrationStatus == RegistrationStatus.INPROGRESS).ToList();

                foreach(var newMember in newMembers)
                {
                    newMember.AppUser = newAppUsers.Where(user => user.Id == newMember.AppUser.Id).First();

                    MemberRegister memberRegister = new MemberRegister();

                    memberRegister.MemberId = newMember.MemberId;
                    memberRegister.FirstName = newMember.AppUser.FirstName;
                    memberRegister.LastName = newMember.AppUser.LastName;
                    memberRegister.DateOfBirth = newMember.AppUser.DateOfBirth;
                    memberRegister.ContactNumber = newMember.AppUser.ContactNumber;
                    memberRegister.Gender = newMember.AppUser.Gender;
                    memberRegister.Password = newMember.AppUser.Password;
                    memberRegister.ConfirmPassword = newMember.AppUser.ConfirmPassword;
                    memberRegister.SecretQuestion = newMember.AppUser.SecretQuestion;
                    memberRegister.SecretAnswer = newMember.AppUser.SecretAnswer;
                    memberRegister.RegistrationStatus = newMember.AppUser.RegistrationStatus;

                    newRegistrations.Add(memberRegister);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            return newRegistrations;
        }

        public async Task<Response> ApproveRegistration(string memberId, string registrationStatus)
        {
            Response response = new Response();

            try
            {
                var approvedMember = _authDbContext.Members.FindAsync(memberId).Result;

                // Member in dbo.AspNetUsers table
                var member = _userManager.Users.Where(member => member.Id == approvedMember.AppUserId).First();

                if (member == null)
                {
                    response.Status = "ERROR";
                    response.Message = "User not found!";

                    return response;
                }

                member.RegistrationStatus = registrationStatus;

                await _authDbContext.SaveChangesAsync();

                response.Status = "SUCCESS";
                response.Message = "Registration approved successfully!";

                return response;
            }
            catch (Exception e)
            {
                response.Status = "ERROR";
                response.Message = e.Message;

                return response;
            }
        }

        public List<MemberRegister> GetMembers()
        {
            List<MemberRegister> members = new List<MemberRegister>();

            try
            {
                List<AppUser> appUsers = _authDbContext.Users.Where(member => member.RegistrationStatus == RegistrationStatus.ACCEPTED).ToList();

                List<Member> acceptedMembers = _authDbContext.Members.Where(member => member.AppUser.RegistrationStatus == RegistrationStatus.ACCEPTED).ToList();

                foreach (var acceptedMember in acceptedMembers)
                {
                    acceptedMember.AppUser = appUsers.Where(user => user.Id == acceptedMember.AppUser.Id).First();

                    MemberRegister memberRegister = new MemberRegister();

                    memberRegister.MemberId = acceptedMember.MemberId;
                    memberRegister.FirstName = acceptedMember.AppUser.FirstName;
                    memberRegister.LastName = acceptedMember.AppUser.LastName;
                    memberRegister.DateOfBirth = acceptedMember.AppUser.DateOfBirth;
                    memberRegister.ContactNumber = acceptedMember.AppUser.ContactNumber;
                    memberRegister.Gender = acceptedMember.AppUser.Gender;
                    memberRegister.Password = acceptedMember.AppUser.Password;
                    memberRegister.ConfirmPassword = acceptedMember.AppUser.ConfirmPassword;
                    memberRegister.SecretQuestion = acceptedMember.AppUser.SecretQuestion;
                    memberRegister.SecretAnswer = acceptedMember.AppUser.SecretAnswer;
                    memberRegister.RegistrationStatus = acceptedMember.AppUser.RegistrationStatus;

                    members.Add(memberRegister);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            return members;
        }

        public MemberRegister GetMemberById(string memberId)
        {
            MemberRegister member;

            try
            {
                var memberData = _authDbContext.Members.Where(member => member.MemberId == memberId).First();

                var userData = _authDbContext.Users.Where(member => member.Id == memberData.AppUserId).First();

                member = new MemberRegister();

                member.MemberId = memberData.MemberId;
                member.FirstName = memberData.AppUser.FirstName;
                member.LastName = memberData.AppUser.LastName;
                member.DateOfBirth = memberData.AppUser.DateOfBirth;
                member.ContactNumber = memberData.AppUser.ContactNumber;
                member.Gender = memberData.AppUser.Gender;
                member.Password = memberData.AppUser.Password;
                member.ConfirmPassword = memberData.AppUser.ConfirmPassword;
                member.SecretQuestion = memberData.AppUser.SecretQuestion;
                member.SecretAnswer = memberData.AppUser.SecretAnswer;
                member.RegistrationStatus = memberData.AppUser.RegistrationStatus;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            return member;
        }
    }
}
