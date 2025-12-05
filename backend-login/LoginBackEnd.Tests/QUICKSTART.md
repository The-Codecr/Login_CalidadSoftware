# Guía de Instalación Rápida - LoginBackEnd.Tests

## Instalación en 5 Pasos

### Paso 1: Verificar Requisitos
```bash
# Verificar que tienes .NET 8.0 o superior
dotnet --version

# Debería mostrar algo como: 8.0.x o 9.0.x
```

### Paso 2: Copiar el Proyecto
Copia la carpeta `LoginBackEnd.Tests` en el mismo directorio donde están tus otros proyectos:

```
backend-login/
├── LoginBackEnd.Api/
├── LoginBackEnd.Application/
├── LoginBackEnd.Domain/
├── LoginBackEnd.Infrastructure/
├── LoginBackEnd.Tests/          ← Aquí
└── LoginBackEnd.sln
```

### Paso 3: Agregar a la Solución

**Opción A - Visual Studio:**
1. Abre `LoginBackEnd.sln`
2. Clic derecho en la solución
3. "Add" → "Existing Project"
4. Selecciona `LoginBackEnd.Tests/LoginBackEnd.Tests.csproj`

**Opción B - Línea de Comandos:**
```bash
cd backend-login
dotnet sln LoginBackEnd.sln add LoginBackEnd.Tests/LoginBackEnd.Tests.csproj
```

### Paso 4: Restaurar y Compilar
```bash
cd LoginBackEnd.Tests
dotnet restore
dotnet build
```

### Paso 5: Ejecutar Pruebas
```bash
dotnet test
```

## ¡Listo!

Deberías ver una salida como esta:

```
Passed!  - Failed:     0, Passed:    87, Skipped:     0, Total:    87
```

---

## Solución de Problemas

### Error: "Project not found"
```bash
# Verifica que las rutas en el .csproj sean correctas
cd LoginBackEnd.Tests
cat LoginBackEnd.Tests.csproj | grep ProjectReference
```

### Error: "Package not found"
```bash
# Limpia y restaura
dotnet clean
dotnet nuget locals all --clear
dotnet restore
```

### Error: "MongoDB.Driver not found"
```bash
# El proyecto Domain ya tiene esta referencia
cd ../LoginBackEnd.Domain
dotnet restore
cd ../LoginBackEnd.Tests
dotnet restore
```

---

## Checklist de Instalación

- [ ] .NET 8.0 SDK instalado
- [ ] Proyecto copiado en ubicación correcta
- [ ] Proyecto agregado a la solución (.sln)
- [ ] `dotnet restore` ejecutado sin errores
- [ ] `dotnet build` ejecutado sin errores
- [ ] `dotnet test` muestra 87 pruebas pasadas

---

## Próximos Pasos

Una vez instalado, puedes:

1. **Explorar las pruebas**
   - Abre `Domain/Users/UserTests.cs`
   - Abre `Application/Auth/PasswordValidatorTests.cs`

2. **Ejecutar pruebas específicas**
   ```bash
   dotnet test --filter "FullyQualifiedName~UserTests"
   ```

3. **Generar reporte de cobertura**
   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```

4. **Ver pruebas en tu IDE**
   - Visual Studio: Menú Test → Test Explorer
   - VS Code: Instala ".NET Core Test Explorer"
   - Rider: El explorador de pruebas aparece automáticamente

---

## 📚 Documentación Adicional

- **README.md**: Documentación completa del proyecto
- **INTEGRATION_GUIDE.md**: Guía detallada de integración
- **SUMMARY.md**: Resumen ejecutivo del proyecto

---

## ¿Necesitas Ayuda?

Si tienes problemas:

1. Revisa la sección "Solución de Problemas" arriba
2. Consulta `INTEGRATION_GUIDE.md` para más detalles
3. Verifica que todas las rutas sean correctas
4. Asegúrate de estar en el directorio correcto

---

**Tiempo estimado de instalación**: 5 minutos  
**Dificultad**: Fácil
