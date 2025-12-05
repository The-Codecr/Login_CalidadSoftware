namespace LoginBackEnd.Application.Auth;
public record ForgotPasswordRequest(string Email, string Token, string Password);