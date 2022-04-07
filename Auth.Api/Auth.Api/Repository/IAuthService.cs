using Auth.Api.Models;
using Auth.Api.Models.Register;
using Auth.Api.Models.Login;
using Auth.Api.Models.Forgot;
using System.Threading.Tasks;

namespace Auth.Api.Repository
{
    public interface IAuthService
    {
        Task<Response> Register(MemberRegister memberRegister);

        Task<Response> Login(UserLogin userLogin);

        Task ForgotUsername(ForgotUsername forgotUsername);

        Task ForgotPassword(ForgotPassword forgotPassword);

        Task ResetPassword(ResetPassword resetPassword);
    }
}
