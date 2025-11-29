using LoginBackEnd.Application.Auth;
using LoginBackEnd.Domain.Users;
using LoginBackEnd.Infrastructure.Auth.Jwt;

namespace LoginBackEnd.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtProvider _jwtProvider;

    private const int MAX_ATTEMPTS = 5;
    private const int BLOCK_MINUTES = 1;

    public AuthService(IUserRepository userRepository, JwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Email and password are required"
            };
        }

        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            // No exponer si el usuario existe o no
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid Login Credentials"
            };
        }

        // Verificar si el usuario está bloqueado
        if (user.IsStillBlocked())
        {
            return new LoginResponse
            {
                Success = false,
                Message = $"Account locked until {user.BlockedUntil?.ToLocalTime()}"
            };
        }

        // Contraseña incorrecta
        if (user.PasswordHash != request.Password)
        {
            user.IncrementLoginAttempts();

            // Si alcanza límite → bloquear
            if (user.LoginAttempts >= MAX_ATTEMPTS)
            {
                user.Block(BLOCK_MINUTES);
            }

            await _userRepository.UpdateAsync(user);

            return new LoginResponse
            {
                Success = false,
                Message = user.IsBlocked
                    ? $"Account locked for {BLOCK_MINUTES} minutes"
                    : "Invalid Login Credentials"
            };
        }

        // Contraseña correcta → reset de intentos y bloqueo
        if (user.LoginAttempts > 0 || user.IsBlocked)
        {
            user.Unblock();
            await _userRepository.UpdateAsync(user);
        }

        var token = _jwtProvider.GenerateToken(user);

        return new LoginResponse
        {
            Success = true,
            Token = token,
            Message = "Login Successful"
        };
    }
}
