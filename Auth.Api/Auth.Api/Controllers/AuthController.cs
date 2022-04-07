using Auth.Api.Models.Login;
using Auth.Api.Models.Register;
using Auth.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
         private readonly IAuthService _authService;

         public AuthController(IAuthService authService)
         {
             _authService = authService;
         }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] MemberRegister memberRegister)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _authService.Register(memberRegister);
                    return Ok(response);
                }
                catch (System.Exception)
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _authService.Login(userLogin);
                    return Ok(response);
                }
                catch (System.Exception)
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }
    }
}