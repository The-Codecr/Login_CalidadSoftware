# Pruebas Unitarias - LoginBackEnd.Tests

Este proyecto contiene las pruebas unitarias para el backend de login, específicamente enfocadas en la clase `User` del dominio.

## 📋 Requisitos

- .NET 8.0 SDK
- Un IDE compatible (Visual Studio, VS Code, Rider)

## 🏗️ Estructura del Proyecto

```
LoginBackEnd.Tests/
├── Domain/
│   └── Users/
│       └── UserTests.cs       # Pruebas para la clase User
├── LoginBackEnd.Tests.csproj  # Archivo de proyecto
└── README.md                  # Este archivo
```

## 🧪 Frameworks y Librerías Utilizadas

- **xUnit**: Framework de pruebas unitarias
- **FluentAssertions**: Librería para assertions más legibles
- **Moq**: Framework de mocking (para futuras pruebas)
- **coverlet.collector**: Herramienta para cobertura de código

## 🚀 Cómo Ejecutar las Pruebas

### Opción 1: Desde la línea de comandos

```bash
# Navegar al directorio del proyecto de pruebas
cd LoginBackEnd.Tests

# Ejecutar todas las pruebas
dotnet test

# Ejecutar con información detallada
dotnet test --logger "console;verbosity=detailed"

# Ejecutar con cobertura de código
dotnet test --collect:"XPlat Code Coverage"
```

### Opción 2: Desde Visual Studio

1. Abrir la solución `LoginBackEnd.sln`
2. Ir al menú **Test** > **Run All Tests**
3. Ver los resultados en el **Test Explorer**

### Opción 3: Desde VS Code

1. Instalar la extensión ".NET Core Test Explorer"
2. Las pruebas aparecerán automáticamente en el panel lateral
3. Hacer clic en "Run All Tests"

## 📊 Cobertura de Pruebas

Las pruebas unitarias cubren los siguientes aspectos de la clase `User`:

### ✅ Constructor
- Creación de usuario con valores correctos
- Valores por defecto

### ✅ Gestión de Intentos de Login
- `IncrementLoginAttempts()`: Incremento de intentos
- `ResetLoginAttempts()`: Reinicio de intentos

### ✅ Bloqueo de Usuario
- `Block()`: Bloqueo con diferentes duraciones
- `Unblock()`: Desbloqueo y reinicio de propiedades
- `IsStillBlocked()`: Verificación de estado de bloqueo

### ✅ Actualización de Contraseña
- `UpdatePassword()`: Cambio de contraseña

### ✅ Escenarios de Integración
- Intentos fallidos de login hasta bloqueo
- Login exitoso después de fallos
- Recuperación de contraseña con desbloqueo
- Expiración de bloqueo temporal

## 📝 Ejemplos de Casos de Prueba

### Ejemplo 1: Verificar Bloqueo de Usuario

```csharp
[Fact]
public void Block_DebeBloquerUsuarioYEstablecerFechaDeBloqueo()
{
    // Arrange
    var user = CreateDefaultUser();
    var blockMinutes = 30;

    // Act
    user.Block(blockMinutes);

    // Assert
    user.IsBlocked.Should().BeTrue();
    user.BlockedUntil.Should().NotBeNull();
}
```

### Ejemplo 2: Verificar Desbloqueo Automático

```csharp
[Fact]
public void IsStillBlocked_CuandoBloqueadoYFechaExpirada_DebeDesbloquearYRetornarFalse()
{
    // Arrange
    var expiredBlockDate = DateTime.UtcNow.AddMinutes(-10);
    var user = new User(id, email, passwordHash, 5, true, expiredBlockDate);

    // Act
    var result = user.IsStillBlocked();

    // Assert
    result.Should().BeFalse();
    user.IsBlocked.Should().BeFalse();
}
```

## 🔧 Integración con el Proyecto Principal

Para agregar este proyecto a la solución principal:

```bash
# Desde la raíz del proyecto
dotnet sln LoginBackEnd.sln add LoginBackEnd.Tests/LoginBackEnd.Tests.csproj
```

## 📈 Estadísticas de Pruebas

| Categoría | Número de Pruebas |
|-----------|-------------------|
| Constructor | 2 |
| Incremento de Intentos | 2 |
| Reinicio de Intentos | 2 |
| Bloqueo | 4 |
| Desbloqueo | 2 |
| Verificación de Bloqueo | 4 |
| Actualización de Contraseña | 2 |
| Escenarios de Integración | 4 |
| **TOTAL** | **22 pruebas** |

## 🎯 Mejores Prácticas Implementadas

1. **Patrón AAA** (Arrange-Act-Assert): Todas las pruebas siguen este patrón
2. **Nombres Descriptivos**: Los nombres de las pruebas describen claramente qué se está probando
3. **Pruebas Aisladas**: Cada prueba es independiente
4. **Datos de Prueba**: Uso de datos parametrizados con `[Theory]` y `[InlineData]`
5. **Cobertura Completa**: Se prueban tanto casos exitosos como casos límite

## 🐛 Ejecutar Pruebas Específicas

```bash
# Ejecutar solo las pruebas de bloqueo
dotnet test --filter "FullyQualifiedName~Block"

# Ejecutar solo las pruebas de una clase específica
dotnet test --filter "FullyQualifiedName~UserTests"

# Ejecutar una prueba específica
dotnet test --filter "FullyQualifiedName~Block_DebeBloquerUsuarioYEstablecerFechaDeBloqueo"
```

## 📚 Recursos Adicionales

- [Documentación de xUnit](https://xunit.net/)
- [Documentación de FluentAssertions](https://fluentassertions.com/)
- [Mejores prácticas de pruebas unitarias en .NET](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

## 🤝 Contribución

Para agregar nuevas pruebas:

1. Crear un nuevo archivo de prueba en la carpeta correspondiente
2. Seguir el patrón de nomenclatura existente
3. Usar el patrón AAA (Arrange-Act-Assert)
4. Asegurar que todas las pruebas pasen antes de hacer commit

## 📄 Licencia

Este proyecto de pruebas sigue la misma licencia que el proyecto principal LoginBackEnd.
