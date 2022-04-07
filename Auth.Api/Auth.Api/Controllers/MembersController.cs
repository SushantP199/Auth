using Auth.Api.Models;
using Auth.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    //[Authorize(Roles = "Admin")]

    public class MembersController : ControllerBase
    {
        private readonly MembersService _membersService;

        public MembersController(MembersService membersService)
        {
            _membersService = membersService;
        }

        [HttpGet]
        public IActionResult GetNewRegistrations()
        {
            try
            {
                var newRegistrations = _membersService.GetNewRegistrations();

                if (newRegistrations != null)
                    return Ok(newRegistrations);

                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return BadRequest();
            }
        }

        [HttpPost]
        [Route("{memberId}/{registrationStatus}")]
        public async Task<IActionResult> ApproveRegistration(string memberId, string registrationStatus)
        {
            try
            {
                var response = await _membersService.ApproveRegistration(memberId, registrationStatus);

                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetMembers()
        {
            try
            {
                var members = _membersService.GetMembers();

                if (members != null)
                    return Ok(members);

                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return BadRequest();
            }
        }

        [HttpGet("{memberId}")]
        public IActionResult GetMembers(string memberId)
        {
            try
            {
                var member = _membersService.GetMemberById(memberId);

                if (member != null)
                    return Ok(member);

                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return BadRequest();
            }
        }
    }
}
