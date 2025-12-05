using FluentAssertions;
using LoginBackEnd.Domain.Users;

namespace LoginBackEnd.Tests.Domain.Users;

/// <summary>
/// Pruebas unitarias para la clase User del dominio.
/// Estas pruebas verifican el comportamiento de los métodos de la entidad User.
/// *** Convención de nombres: "NombreMetodo_Escenario_ResultadoEsperado" | "When_Then"
/// </summary>
public class UserTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_DebeCrearUsuarioConValoresCorrectos()
    {
        // Arrange
        var id = Guid.NewGuid();
        var email = "test@example.com";
        var passwordHash = "hashedPassword123";
        var loginAttempts = 0;
        var isBlocked = false;
        DateTime? blockedUntil = null;

        // Act
        var user = new User(id, email, passwordHash, loginAttempts, isBlocked, blockedUntil);

        // Assert
        user.Id.Should().Be(id);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
        user.LoginAttempts.Should().Be(loginAttempts);
        user.IsBlocked.Should().BeFalse();
        user.BlockedUntil.Should().BeNull();
    }

    [Fact]
    public void Constructor_DebeCrearUsuarioConValoresPorDefecto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var email = "test@example.com";
        var passwordHash = "hashedPassword123";

        // Act
        var user = new User(id, email, passwordHash);

        // Assert
        user.LoginAttempts.Should().Be(0);
        user.IsBlocked.Should().BeFalse();
        user.BlockedUntil.Should().BeNull();
    }

    #endregion

    #region IncrementLoginAttempts Tests

    [Fact]
    public void IncrementLoginAttempts_DebeIncrementarContadorDeIntentos()
    {
        // Arrange
        var user = CreateDefaultUser();
        var initialAttempts = user.LoginAttempts;

        // Act
        user.IncrementLoginAttempts();

        // Assert
        user.LoginAttempts.Should().Be(initialAttempts + 1);
    }

    [Fact]
    public void IncrementLoginAttempts_DebeIncrementarMultiplesVeces()
    {
        // Arrange
        var user = CreateDefaultUser();

        // Act
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();

        // Assert
        user.LoginAttempts.Should().Be(3);
    }

    #endregion

    #region ResetLoginAttempts Tests

    [Fact]
    public void ResetLoginAttempts_DebeEstablecerIntentosEnCero()
    {
        // Arrange
        var user = CreateDefaultUser();
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();

        // Act
        user.ResetLoginAttempts();

        // Assert
        user.LoginAttempts.Should().Be(0);
    }

    [Fact]
    public void ResetLoginAttempts_CuandoIntentosYaEsCero_DebeMantenersEnCero()
    {
        // Arrange
        var user = CreateDefaultUser();

        // Act
        user.ResetLoginAttempts();

        // Assert
        user.LoginAttempts.Should().Be(0);
    }

    #endregion

    #region Block Tests

    [Fact]
    public void Block_DebeBloquerUsuarioYEstablecerFechaDeBloqueo()
    {
        // Arrange
        var user = CreateDefaultUser();
        var blockMinutes = 30;
        var beforeBlock = DateTime.UtcNow;

        // Act
        user.Block(blockMinutes);
        var afterBlock = DateTime.UtcNow;

        // Assert
        user.IsBlocked.Should().BeTrue();
        user.BlockedUntil.Should().NotBeNull();
        user.BlockedUntil.Should().BeAfter(beforeBlock);
        user.BlockedUntil.Should().BeBefore(afterBlock.AddMinutes(blockMinutes + 1));
    }

    [Fact]
    public void Block_ConDiferentesMinutos_DebeEstablecerFechaCorrecta()
    {
        // Arrange
        var user = CreateDefaultUser();
        var blockMinutes = 60;

        // Act
        user.Block(blockMinutes);

        // Assert
        user.IsBlocked.Should().BeTrue();
        user.BlockedUntil.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(blockMinutes), TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(1440)] // 24 horas
    public void Block_ConDiferentesDuraciones_DebeFuncionarCorrectamente(int minutes)
    {
        // Arrange
        var user = CreateDefaultUser();

        // Act
        user.Block(minutes);

        // Assert
        user.IsBlocked.Should().BeTrue();
        user.BlockedUntil.Should().NotBeNull();
        user.BlockedUntil.Should().BeAfter(DateTime.UtcNow);
    }

    #endregion

    #region Unblock Tests

    [Fact]
    public void Unblock_DebeDesbloquearUsuarioYRestablecerPropiedades()
    {
        // Arrange
        var user = CreateDefaultUser();
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();
        user.Block(30);

        // Act
        user.Unblock();

        // Assert
        user.IsBlocked.Should().BeFalse();
        user.BlockedUntil.Should().BeNull();
        user.LoginAttempts.Should().Be(0);
    }

    [Fact]
    public void Unblock_CuandoUsuarioNoBloqueado_NoDebeGenerarError()
    {
        // Arrange
        var user = CreateDefaultUser();

        // Act
        user.Unblock();

        // Assert
        user.IsBlocked.Should().BeFalse();
        user.BlockedUntil.Should().BeNull();
        user.LoginAttempts.Should().Be(0);
    }

    #endregion

    #region IsStillBlocked Tests

    [Fact]
    public void IsStillBlocked_CuandoUsuarioNoBloqueado_DebeRetornarFalse()
    {
        // Arrange
        var user = CreateDefaultUser();

        // Act
        var result = user.IsStillBlocked();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsStillBlocked_CuandoBloqueadoYFechaNoExpirada_DebeRetornarTrue()
    {
        // Arrange
        var user = CreateDefaultUser();
        user.Block(30); // Bloquear por 30 minutos

        // Act
        var result = user.IsStillBlocked();

        // Assert
        result.Should().BeTrue();
        user.IsBlocked.Should().BeTrue();
    }

    [Fact]
    public void IsStillBlocked_CuandoBloqueadoYFechaExpirada_DebeDesbloquearYRetornarFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var email = "test@example.com";
        var passwordHash = "hashedPassword123";
        
        // Crear usuario con bloqueo expirado (fecha en el pasado)
        var expiredBlockDate = DateTime.UtcNow.AddMinutes(-10);
        var user = new User(id, email, passwordHash, 5, true, expiredBlockDate);

        // Act
        var result = user.IsStillBlocked();

        // Assert
        result.Should().BeFalse();
        user.IsBlocked.Should().BeFalse();
        user.BlockedUntil.Should().BeNull();
        user.LoginAttempts.Should().Be(0);
    }

    [Fact]
    public void IsStillBlocked_CuandoBloqueadoPeroFechaEsNull_DebeDesbloquearYRetornarFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var email = "test@example.com";
        var passwordHash = "hashedPassword123";
        
        // Crear usuario bloqueado pero sin fecha de bloqueo
        var user = new User(id, email, passwordHash, 5, true, null);

        // Act
        var result = user.IsStillBlocked();

        // Assert
        result.Should().BeFalse();
        user.IsBlocked.Should().BeFalse();
    }

    #endregion

    #region UpdatePassword Tests

    [Fact]
    public void UpdatePassword_DebeActualizarPasswordHash()
    {
        // Arrange
        var user = CreateDefaultUser();
        var oldPasswordHash = user.PasswordHash;
        var newPasswordHash = "newHashedPassword456";

        // Act
        user.UpdatePassword(newPasswordHash);

        // Assert
        user.PasswordHash.Should().Be(newPasswordHash);
        user.PasswordHash.Should().NotBe(oldPasswordHash);
    }

    [Fact]
    public void UpdatePassword_ConDiferentesPasswords_DebeActualizarCorrectamente()
    {
        // Arrange
        var user = CreateDefaultUser();

        // Act & Assert
        user.UpdatePassword("password1");
        user.PasswordHash.Should().Be("password1");

        user.UpdatePassword("password2");
        user.PasswordHash.Should().Be("password2");

        user.UpdatePassword("password3");
        user.PasswordHash.Should().Be("password3");
    }

    #endregion

    #region Integration Scenarios Tests

    [Fact]
    public void EscenarioCompleto_IntentosDeLoginFallidos_DebeBloquerUsuario()
    {
        // Arrange
        var user = CreateDefaultUser();
        var maxAttempts = 5;

        // Act - Simular 5 intentos fallidos
        for (int i = 0; i < maxAttempts; i++)
        {
            user.IncrementLoginAttempts();
        }

        if (user.LoginAttempts >= maxAttempts)
        {
            user.Block(1); // Bloquear por 1 minuto
        }

        // Assert
        user.LoginAttempts.Should().Be(maxAttempts);
        user.IsBlocked.Should().BeTrue();
        user.BlockedUntil.Should().NotBeNull();
    }

    [Fact]
    public void EscenarioCompleto_LoginExitosoDespuesDeFallos_DebeRestablecerIntentos()
    {
        // Arrange
        var user = CreateDefaultUser();
        
        // Act - Simular intentos fallidos
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();
        
        // Login exitoso
        user.ResetLoginAttempts();

        // Assert
        user.LoginAttempts.Should().Be(0);
        user.IsBlocked.Should().BeFalse();
    }

    [Fact]
    public void EscenarioCompleto_RecuperacionDePassword_DebeDesbloquearYActualizarPassword()
    {
        // Arrange
        var user = CreateDefaultUser();
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();
        user.IncrementLoginAttempts();
        user.Block(30);

        var newPasswordHash = "newSecurePasswordHash";

        // Act - Recuperación de contraseña
        user.UpdatePassword(newPasswordHash);
        user.Unblock();

        // Assert
        user.PasswordHash.Should().Be(newPasswordHash);
        user.IsBlocked.Should().BeFalse();
        user.BlockedUntil.Should().BeNull();
        user.LoginAttempts.Should().Be(0);
    }

    [Fact]
    public void EscenarioCompleto_BloqueoTemporalExpirado_DebePermitirNuevoLogin()
    {
        // Arrange
        var id = Guid.NewGuid();
        var email = "test@example.com";
        var passwordHash = "hashedPassword123";
        
        // Crear usuario con bloqueo expirado
        var expiredBlockDate = DateTime.UtcNow.AddMinutes(-5);
        var user = new User(id, email, passwordHash, 5, true, expiredBlockDate);

        // Act
        var isStillBlocked = user.IsStillBlocked();

        // Assert
        isStillBlocked.Should().BeFalse();
        user.IsBlocked.Should().BeFalse();
        user.LoginAttempts.Should().Be(0);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Crea un usuario con valores por defecto para las pruebas.
    /// </summary>
    private User CreateDefaultUser()
    {
        return new User(
            Guid.NewGuid(),
            "test@example.com",
            "hashedPassword123",
            0,
            false,
            null
        );
    }

    #endregion
}
