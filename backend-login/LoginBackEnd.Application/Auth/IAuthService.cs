using System.Threading.Tasks;

namespace LoginBackEnd.Application.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
}