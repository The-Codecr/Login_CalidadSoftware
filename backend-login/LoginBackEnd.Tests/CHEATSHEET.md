# Hoja de Referencia Rápida - LoginBackEnd.Tests

## Comandos Esenciales

### Ejecutar Todas las Pruebas
```bash
dotnet test
```

### Ejecutar con Salida Detallada
```bash
dotnet test --verbosity detailed
```

### Ejecutar con Cobertura de Código
```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

# ********************************************************************************************


## Filtros de Pruebas

### Solo Pruebas de User
```bash
dotnet test --filter "FullyQualifiedName~UserTests"
```

### Solo Pruebas de PasswordValidator
```bash
dotnet test --filter "FullyQualifiedName~PasswordValidatorTests"
```

### Prueba Específica
```bash
dotnet test --filter "FullyQualifiedName~Block_DebeBloquerUsuario"
```

### Por Categoría (Domain)
```bash
dotnet test --filter "FullyQualifiedName~Domain"
```

### Por Categoría (Application)
```bash
dotnet test --filter "FullyQualifiedName~Application"
```

---

# ********************************************************************************************


## Comandos de Build

### Limpiar
```bash
dotnet clean
```

### Restaurar Paquetes
```bash
dotnet restore
```

### Compilar
```bash
dotnet build
```

### Limpiar + Restaurar + Compilar
```bash
dotnet clean && dotnet restore && dotnet build
```

---


# ********************************************************************************************


## Reportes de Cobertura

### Generar Cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Instalar ReportGenerator
```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

### Generar Reporte HTML
```bash
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

### Windows - Abrir Reporte
```powershell
start coveragereport/index.html
```

### Mac - Abrir Reporte
```bash
open coveragereport/index.html
```

### Linux - Abrir Reporte
```bash
xdg-open coveragereport/index.html
```

---

## Troubleshooting

### Limpiar Cache de NuGet
```bash
dotnet nuget locals all --clear
```

### Restaurar con Fuerza
```bash
dotnet restore --force
```

### Ver Versión de .NET
```bash
dotnet --version
```

### Listar SDKs Instalados
```bash
dotnet --list-sdks
```

### Ver Info del Proyecto
```bash
dotnet list package
```

---

## Comandos de Visual Studio

### Ejecutar Tests
```
Test → Run All Tests
Ctrl+R, A
```

### Ver Test Explorer
```
Test → Test Explorer
Ctrl+E, T
```

### Debug Tests
```
Clic derecho en test → Debug Test
```

---

## Comandos de VS Code

### Ejecutar Tests (con extensión)
```
Ctrl+; Ctrl+R (Windows/Linux)
Cmd+; Cmd+R (Mac)
```

### Ver Test Explorer
```
Testing icon en sidebar
```

---


# ********************************************************************************************


## Integración con Solución

### Agregar Proyecto a Solución
```bash
dotnet sln LoginBackEnd.sln add LoginBackEnd.Tests/LoginBackEnd.Tests.csproj
```

### Listar Proyectos en Solución
```bash
dotnet sln LoginBackEnd.sln list
```

### Remover Proyecto (si es necesario)
```bash
dotnet sln LoginBackEnd.sln remove LoginBackEnd.Tests/LoginBackEnd.Tests.csproj
```

---

## Comandos de Inspección

### Ver Referencias del Proyecto
```bash
dotnet list reference
```

### Ver Paquetes NuGet
```bash
dotnet list package
```

### Ver Paquetes Desactualizados
```bash
dotnet list package --outdated
```

---

# ********************************************************************************************

## Scripts Personalizados

### Windows
```powershell
# Básico
.\run-tests.ps1

# Con cobertura
.\run-tests.ps1 -Coverage

# Todo incluido
.\run-tests.ps1 -Coverage -DetailedOutput -OpenReport
```

### Linux/Mac
```bash
# Básico
./run-tests.sh

# Con cobertura
./run-tests.sh --coverage

# Todo incluido
./run-tests.sh --coverage --detailed --open-report
```

---

## Atajos de Teclado

### Visual Studio
| Acción | Windows/Linux | Mac |
|--------|---------------|-----|
| Run All Tests | Ctrl+R, A | Cmd+R, A |
| Debug Test | Ctrl+R, Ctrl+T | Cmd+R, Cmd+T |
| Test Explorer | Ctrl+E, T | Cmd+E, T |
| Repeat Last Run | Ctrl+R, L | Cmd+R, L |

### VS Code (con .NET Test Explorer)
| Acción | Windows/Linux | Mac |
|--------|---------------|-----|
| Run Tests | Ctrl+; Ctrl+R | Cmd+; Cmd+R |
| Debug Tests | Ctrl+; Ctrl+D | Cmd+; Cmd+D |

---

## Mejores Prácticas

### Antes de Commit
```bash
# 1. Limpiar
dotnet clean

# 2. Restaurar
dotnet restore

# 3. Compilar
dotnet build

# 4. Ejecutar tests
dotnet test

# 5. Verificar cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### Workflow Completo
```bash
# Script completo de verificación
dotnet clean && \
dotnet restore && \
dotnet build && \
dotnet test --collect:"XPlat Code Coverage" && \
echo " Todo OK - Listo para commit"
```

---

## Documentación

| Archivo | Descripción |
|---------|-------------|
| **QUICKSTART.md** | Inicio rápido (5 min) |
| **README.md** | Documentación completa |
| **INTEGRATION_GUIDE.md** | Guía de integración detallada |
| **SUMMARY.md** | Resumen ejecutivo |
| **PROJECT_STRUCTURE.txt** | Estructura visual del proyecto |

---

## Ayuda Rápida

### Error: "Project not found"
```bash
cd LoginBackEnd.Tests
dotnet restore
```

### Error: "Package not found"
```bash
dotnet nuget locals all --clear
dotnet restore
```

### Error: "Build failed"
```bash
dotnet clean
dotnet build --verbosity detailed
```

### Tests no aparecen en IDE
```bash
# Cerrar IDE
dotnet clean
dotnet restore
dotnet build
# Abrir IDE
```

---

## Quick Tips

1. **Siempre ejecuta `dotnet restore` después de clonar**
2. **Usa `dotnet clean` si algo se comporta raro**
3. **Los scripts automatizan tareas comunes**
4. **La cobertura te muestra qué falta probar**
5. **Los filtros ayudan a enfocarte en pruebas específicas**

---

## Recursos

- [Documentación xUnit](https://xunit.net/)
- [FluentAssertions Docs](https://fluentassertions.com/)
- [.NET Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

