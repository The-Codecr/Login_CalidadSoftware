# Guía de Integración del Proyecto de Pruebas

## Pasos para Integrar LoginBackEnd.Tests en la Solución

### 1. Copiar el Proyecto de Pruebas

Copie la carpeta `LoginBackEnd.Tests` en el mismo directorio donde se encuentran los otros proyectos:

```
backend-login/
├── LoginBackEnd.Api/
├── LoginBackEnd.Application/
├── LoginBackEnd.Domain/
├── LoginBackEnd.Infrastructure/
├── LoginBackEnd.Tests/          ← Copiar aquí
└── LoginBackEnd.sln
```

### 2. Agregar el Proyecto a la Solución

Opción A - Usando Visual Studio:
1. Abrir la solución `LoginBackEnd.sln`
2. Clic derecho en la solución en el Solution Explorer
3. Seleccionar "Add" > "Existing Project..."
4. Navegar a `LoginBackEnd.Tests/LoginBackEnd.Tests.csproj` y seleccionar

Opción B - Usando línea de comandos:
```bash
cd backend-login
dotnet sln LoginBackEnd.sln add LoginBackEnd.Tests/LoginBackEnd.Tests.csproj
```

### 3. Restaurar Paquetes NuGet

```bash
cd LoginBackEnd.Tests
dotnet restore
```

### 4. Compilar el Proyecto de Pruebas

```bash
dotnet build
```

### 5. Ejecutar las Pruebas

```bash
dotnet test
```

## Verificación de la Integración

Si todo está correctamente configurado, deberías ver una salida similar a:

```
Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    22, Skipped:     0, Total:    22, Duration: < 1 s
```

## Actualización del Archivo .sln (Manual)

Si prefieres editar manualmente el archivo `.sln`, agrega estas líneas:

```
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "LoginBackEnd.Tests", "LoginBackEnd.Tests\LoginBackEnd.Tests.csproj", "{GUID-GENERADO-AUTOMATICAMENTE}"
EndProject
```

Y en la sección `GlobalSection(ProjectConfigurationPlatforms)`:

```
{GUID}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
{GUID}.Debug|Any CPU.Build.0 = Debug|Any CPU
{GUID}.Release|Any CPU.ActiveCfg = Release|Any CPU
{GUID}.Release|Any CPU.Build.0 = Release|Any CPU
```

## Configuración en VS Code

Si usas VS Code, instala las siguientes extensiones:

1. **C# Dev Kit** (Microsoft)
2. **.NET Core Test Explorer** (Jun Han)
3. **Coverage Gutters** (ryanluker) - Para visualizar cobertura de código

### settings.json recomendado para VS Code:

```json
{
    "dotnet-test-explorer.testProjectPath": "**/*Tests.csproj",
    "dotnet-test-explorer.enableTelemetry": false,
    "coverage-gutters.coverageFileNames": [
        "coverage.cobertura.xml"
    ]
}
```

## Ejecución en CI/CD

### GitHub Actions Ejemplo:

```yaml
name: Run Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"
    
    - name: Upload coverage
      uses: codecov/codecov-action@v3
```

## Solución de Problemas Comunes

### Error: "Project not found"
**Solución**: Verificar que las rutas relativas en el archivo `.csproj` son correctas:
```xml
<ProjectReference Include="..\LoginBackEnd.Domain\LoginBackEnd.Domain.csproj" />
```

### Error: "Package restore failed"
**Solución**: 
```bash
dotnet nuget locals all --clear
dotnet restore
```

### Error: "MongoDB.Driver not found"
**Solución**: El proyecto de Domain ya tiene la referencia, solo necesitas restaurar:
```bash
dotnet restore
```

## Generación de Reportes de Cobertura

### Con ReportGenerator:

```bash
# Instalar ReportGenerator globalmente
dotnet tool install -g dotnet-reportgenerator-globaltool

# Ejecutar pruebas con cobertura
dotnet test --collect:"XPlat Code Coverage"

# Generar reporte HTML
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

# Abrir el reporte
# Windows: start coveragereport/index.html
# macOS: open coveragereport/index.html
# Linux: xdg-open coveragereport/index.html
```

## Checklist de Integración

- [ ] Proyecto copiado en la ubicación correcta
- [ ] Proyecto agregado a la solución (.sln)
- [ ] Paquetes NuGet restaurados
- [ ] Proyecto compila sin errores
- [ ] Todas las pruebas pasan (22/22)
- [ ] IDE reconoce las pruebas (Test Explorer)
- [ ] Referencias a otros proyectos funcionan correctamente

## Soporte

Si encuentras problemas durante la integración:

1. Verificar que .NET 8.0 SDK está instalado: `dotnet --version`
2. Limpiar y reconstruir: `dotnet clean && dotnet build`
3. Revisar las referencias entre proyectos
4. Consultar los logs de error detallados: `dotnet build -v detailed`

---

**Nota**: Este proyecto de pruebas está diseñado para funcionar con .NET 8.0 y la estructura de proyectos existente. Si tu proyecto usa una versión diferente, ajusta el `TargetFramework` en el archivo `.csproj` correspondientemente.
