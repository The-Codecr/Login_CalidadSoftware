using System.Text.RegularExpressions;

namespace LoginBackEnd.Application.Auth;

public static class PasswordValidator
{
    private const int MIN_LENGTH = 5;
    private const int MAX_LENGTH = 10;

    public static (bool IsValid, string ErrorMessage) Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return (false, "La contraseña es requerida");
        }

        if (password.Length < MIN_LENGTH)
        {
            return (false, $"La contraseña debe tener al menos {MIN_LENGTH} caracteres");
        }

        if (password.Length > MAX_LENGTH)
        {
            return (false, $"La contraseña no puede tener más de {MAX_LENGTH} caracteres");
        }

        // Validar al menos una mayúscula
        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            return (false, "La contraseña debe contener al menos una letra mayúscula");
        }

        // Validar al menos un carácter especial
        if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>_\-+=\[\]\\\/`~;]"))
        {
            return (false, "La contraseña debe contener al menos un carácter especial (!@#$%^&*...)");
        }

        return (true, string.Empty);
    }

    public static string GetPasswordRequirements()
    {
        return $"La contraseña debe tener entre {MIN_LENGTH} y {MAX_LENGTH} caracteres, " +
               "al menos una mayúscula y un carácter especial (!@#$%^&*...)";
    }
}