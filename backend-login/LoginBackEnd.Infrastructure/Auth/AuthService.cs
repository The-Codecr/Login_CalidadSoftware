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
        if(!request.Email.Contains("@") && !string.IsNullOrWhiteSpace(request.Email))
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Ingresa un correo electrónico válido (ejemplo: usuario@dominio.com)"
            };
        }

        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Email y contraseña son requeridos"
            };
        }

        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Credenciales inválidas"
            };
        }

        if (user.IsStillBlocked())
        {
            return new LoginResponse
            {
                Success = false,
                Message = $"Cuenta bloqueada hasta {user.BlockedUntil?.ToLocalTime()}"
            };
        }

        if (user.PasswordHash != request.Password)
        {
            user.IncrementLoginAttempts();

            if (user.LoginAttempts >= MAX_ATTEMPTS)
            {
                user.Block(BLOCK_MINUTES);
            }

            await _userRepository.UpdateAsync(user);

            return new LoginResponse
            {
                Success = false,
                Message = user.IsBlocked
                    ? $"Cuenta bloqueada por {BLOCK_MINUTES} minutos"
                    : "Credenciales inválidas"
            };
        }

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
            Message = "Inicio de sesión exitoso"
        };
    }

    public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        // Validar que los campos no estén vacíos
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Token) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return new ForgotPasswordResponse
            {
                Success = false,
                Message = "Email, token y contraseña son requeridos"
            };
        }

        // Validar formato de email
        if (!request.Email.Contains("@"))
        {
            return new ForgotPasswordResponse
            {
                Success = false,
                Message = "Email inválido"
            };
        }

        // VALIDAR CONTRASEÑA
        var (isValid, errorMessage) = PasswordValidator.Validate(request.Password);
        if (!isValid)
        {
            return new ForgotPasswordResponse
            {
                Success = false,
                Message = errorMessage
            };
        }

        // Buscar el usuario por email
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            return new ForgotPasswordResponse
            {
                Success = false,
                Message = "Usuario no encontrado"
            };
        }

        // En producción, validar que el token sea válido
        // TODO: Implementar validación de token (BD o cache)

        // Actualizar la contraseña
        // NOTA: En producción deberías hashear la contraseña
        user.UpdatePassword(request.Password);

        // Desbloquear usuario si estaba bloqueado
        if (user.IsBlocked)
        {
            user.Unblock();
        }

        // Guardar cambios en la base de datos
        await _userRepository.UpdateAsync(user);

        return new ForgotPasswordResponse
        {
            Success = true,
            Message = "Contraseña actualizada correctamente"
        };
    }
}