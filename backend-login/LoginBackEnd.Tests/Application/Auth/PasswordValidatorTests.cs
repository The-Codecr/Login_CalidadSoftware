using FluentAssertions;
using LoginBackEnd.Application.Auth;

namespace LoginBackEnd.Tests.Application.Auth;

/// <summary>
/// Pruebas unitarias para la clase PasswordValidator.
/// Verifica que las validaciones de contraseña funcionen correctamente.
/// *** Convención de nombres: NombreMetodo_Escenario_ResultadoEsperado
/// </summary>
public class PasswordValidatorTests
{
    #region Validaciones de Longitud

    [Fact]
    public void Validate_ContraseñaVacia_DebeRetornarError()
    {
        // Arrange
        var password = "";

        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        errorMessage.Should().Be("La contraseña es requerida");
    }

    [Fact]
    public void Validate_ContraseñaNull_DebeRetornarError()
    {
        // Arrange
        string password = null!;

        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        errorMessage.Should().Be("La contraseña es requerida");
    }

    [Fact]
    public void Validate_ContraseñaSoloEspacios_DebeRetornarError()
    {
        // Arrange
        var password = "   ";

        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        errorMessage.Should().Be("La contraseña es requerida");
    }

    [Theory]
    [InlineData("A*")]      // 2 caracteres
    [InlineData("Ab*")]     // 3 caracteres
    [InlineData("Abc*")]    // 4 caracteres
    public void Validate_ContraseñaMenorA5Caracteres_DebeRetornarError(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        errorMessage.Should().Be("La contraseña debe tener al menos 5 caracteres");
    }

    [Fact]
    public void Validate_ContraseñaCon11Caracteres_DebeRetornarError()
    {
        // Arrange
        var password = "Abcdefghi*1"; // 11 caracteres

        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        errorMessage.Should().Be("La contraseña no puede tener más de 10 caracteres");
    }

    [Theory]
    [InlineData("Abcdefghij*1")]    // 12 caracteres
    [InlineData("Abcdefghijk*12")]  // 13 caracteres
    [InlineData("Abcdefghijklm*123")] // 16 caracteres
    public void Validate_ContraseñaMayorA10Caracteres_DebeRetornarError(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        errorMessage.Should().Be("La contraseña no puede tener más de 10 caracteres");
    }

    #endregion

    #region Validaciones de Mayúsculas

    [Theory]
    [InlineData("abcd*")]       // Sin mayúscula
    [InlineData("abc123*")]     // Sin mayúscula
    [InlineData("test@123")]    // Sin mayúscula
    public void Validate_SinMayuscula_DebeRetornarError(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        errorMessage.Should().Be("La contraseña debe contener al menos una letra mayúscula");
    }

    [Theory]
    [InlineData("Abcd*")]       // Una mayúscula al inicio
    [InlineData("aBCd*")]       // Múltiples mayúsculas
    [InlineData("abcdE*")]      // Una mayúscula al final
    public void Validate_ConMayuscula_DebeValidarCorrectamente(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeTrue();
        errorMessage.Should().BeEmpty();
    }

    #endregion

    #region Validaciones de Caracteres Especiales

    [Theory]
    [InlineData("Abcde")]       // Sin carácter especial
    [InlineData("Abc123")]      // Sin carácter especial
    [InlineData("ABCDE123")]    // Sin carácter especial
    public void Validate_SinCaracterEspecial_DebeRetornarError(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        errorMessage.Should().Be("La contraseña debe contener al menos un carácter especial (!@#$%^&*...)");
    }

    [Theory]
    [InlineData("Abc!12")]      // Con !
    [InlineData("Abc@123")]     // Con @
    [InlineData("Test#123")]    // Con #
    [InlineData("Pass$word")]   // Con $
    [InlineData("Test%123")]    // Con %
    [InlineData("Pass^word")]   // Con ^
    [InlineData("Test&123")]    // Con &
    [InlineData("Pass*word")]   // Con *
    [InlineData("Test(123")]    // Con (
    [InlineData("Pass)word")]   // Con )
    [InlineData("Test,123")]    // Con ,
    [InlineData("Pass.word")]   // Con .
    [InlineData("Test?123")]    // Con ?
    [InlineData("Pass:word")]   // Con :
    [InlineData("Test;123")]    // Con ;
    [InlineData("Pass'word")]   // Con '
    [InlineData("Test\"123")]   // Con "
    [InlineData("Pass{word")]   // Con {
    [InlineData("Test}123")]    // Con }
    [InlineData("Pass|word")]   // Con |
    [InlineData("Test<123")]    // Con <
    [InlineData("Pass>word")]   // Con >
    [InlineData("Test_123")]    // Con _
    [InlineData("Pass-word")]   // Con -
    [InlineData("Test+123")]    // Con +
    [InlineData("Pass=word")]   // Con =
    [InlineData("Test[123")]    // Con [
    [InlineData("Pass]word")]   // Con ]
    [InlineData("Test\\123")]   // Con \
    [InlineData("Pass/word")]   // Con /
    [InlineData("Test`123")]    // Con `
    [InlineData("Pass~word")]   // Con ~
    public void Validate_ConCaracterEspecial_DebeSerValida(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeTrue();
        errorMessage.Should().BeEmpty();
    }

    #endregion

    #region Contraseñas Válidas

    [Theory]
    [InlineData("Abc12*")]      // Contraseña mínima válida (6 caracteres)
    [InlineData("Test@123")]    // Contraseña válida común
    [InlineData("Pass#word")]   // Contraseña válida con palabra
    [InlineData("MyP@ss1")]     // Contraseña válida corta
    [InlineData("Secure!23")]   // Contraseña válida segura
    [InlineData("A1b2C3d4*")]   // Contraseña válida con números
    [InlineData("Valid$987")]   // Contraseña válida con números al final
    public void Validate_ContraseñasValidas_DebenSerAceptadas(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeTrue();
        errorMessage.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Ab*12")]       // Exactamente 5 caracteres (mínimo)
    [InlineData("Abcde*1234")]  // Exactamente 10 caracteres (máximo)
    public void Validate_ContraseñaEnLimitesExactos_DebeSerValida(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeTrue();
        errorMessage.Should().BeEmpty();
    }

    #endregion

    #region Contraseñas Inválidas Comunes

    [Theory]
    [InlineData("password")]        // Muy común, sin mayúscula ni especial
    [InlineData("Password")]        // Sin carácter especial
    [InlineData("PASSWORD")]        // Sin minúscula ni especial
    [InlineData("12345678")]        // Solo números
    [InlineData("abcdefgh")]        // Solo minúsculas
    [InlineData("ABCDEFGH")]        // Solo mayúsculas
    public void Validate_ContraseñasComunesInseguras_DebenSerRechazadas(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        errorMessage.Should().NotBeEmpty();
    }

    #endregion

    #region GetPasswordRequirements Tests

    [Fact]
    public void GetPasswordRequirements_DebeRetornarMensajeDescriptivo()
    {
        // Act
        var requirements = PasswordValidator.GetPasswordRequirements();

        // Assert
        requirements.Should().NotBeNullOrEmpty();
        requirements.Should().Contain("5");
        requirements.Should().Contain("10");
        requirements.Should().Contain("mayúscula");
        requirements.Should().Contain("carácter especial");
    }

    [Fact]
    public void GetPasswordRequirements_DebeContenerInformacionCompleta()
    {
        // Act
        var requirements = PasswordValidator.GetPasswordRequirements();

        // Assert
        requirements.Should().Be(
            "La contraseña debe tener entre 5 y 10 caracteres, " +
            "al menos una mayúscula y un carácter especial (!@#$%^&*...)"
        );
    }

    #endregion

    #region Escenarios de Edge Cases

    [Theory]
    [InlineData("AAAAA*")]      // Todas mayúsculas excepto especial
    [InlineData("A!!!!")]       // Mínimo con repetición de especiales
    [InlineData("A*a*a*a*a*")]  // Alternando mayúscula, especial, minúscula
    public void Validate_CasosLimite_DebeFuncionarCorrectamente(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeTrue();
        errorMessage.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Añ*bc")]       // Con letra acentuada
    [InlineData("Tést@123")]    // Con acento
    [InlineData("Contrañ*")]    // Con ñ
    public void Validate_ConCaracteresEspecialesLatinos_DebeFuncionarCorrectamente(string password)
    {
        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        // Debería ser válida si cumple todos los requisitos
        isValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_ConEspaciosEnMedio_DebeSerValida()
    {
        // Arrange
        var password = "My P@ss";

        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeTrue();
        errorMessage.Should().BeEmpty();
    }

    #endregion

    #region Múltiples Errores

    [Fact]
    public void Validate_ConVariosErrores_DebeRetornarPrimerError()
    {
        // Arrange
        var password = "a"; // Sin mayúscula, sin especial, muy corta

        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        // Debe retornar el primer error encontrado (longitud)
        errorMessage.Should().Be("La contraseña debe tener al menos 5 caracteres");
    }

    [Fact]
    public void Validate_DemasiadoLargaSinMayuscula_DebeRetornarErrorDeLongitud()
    {
        // Arrange
        var password = "abcdefghijk*"; // Muy larga y sin mayúscula

        // Act
        var (isValid, errorMessage) = PasswordValidator.Validate(password);

        // Assert
        isValid.Should().BeFalse();
        // El error de longitud tiene prioridad
        errorMessage.Should().Be("La contraseña no puede tener más de 10 caracteres");
    }

    #endregion
}
