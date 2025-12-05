# Script para ejecutar pruebas unitarias del proyecto LoginBackEnd
# Uso: .\run-tests.ps1 [-Coverage] [-DetailedOutput] [-OpenReport]

param(
    [switch]$Coverage,
    [switch]$DetailedOutput,
    [switch]$OpenReport
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  LoginBackEnd - Ejecución de Pruebas  " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Limpiar salidas anteriores
Write-Host "Limpiando build anterior..." -ForegroundColor Yellow
dotnet clean --verbosity quiet

# Restaurar paquetes
Write-Host "Restaurando paquetes NuGet..." -ForegroundColor Yellow
dotnet restore --verbosity quiet

# Compilar
Write-Host "Compilando proyecto..." -ForegroundColor Yellow
$buildResult = dotnet build --no-restore --verbosity quiet
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error en la compilación. Abortando..." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  Ejecutando Pruebas Unitarias" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# Configurar parámetros de prueba
$testParams = @("test", "--no-build")

if ($DetailedOutput) {
    $testParams += "--verbosity"
    $testParams += "detailed"
} else {
    $testParams += "--verbosity"
    $testParams += "normal"
}

if ($Coverage) {
    Write-Host "Generando reporte de cobertura de código..." -ForegroundColor Yellow
    $testParams += "--collect:XPlat Code Coverage"
}

# Ejecutar pruebas
& dotnet $testParams

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "❌ Algunas pruebas fallaron" -ForegroundColor Red
    exit 1
} else {
    Write-Host ""
    Write-Host "✅ Todas las pruebas pasaron exitosamente" -ForegroundColor Green
}

# Generar reporte HTML de cobertura si se solicitó
if ($Coverage -and $OpenReport) {
    Write-Host ""
    Write-Host "Generando reporte HTML de cobertura..." -ForegroundColor Yellow
    
    # Verificar si reportgenerator está instalado
    $reportGenerator = Get-Command reportgenerator -ErrorAction SilentlyContinue
    
    if (-not $reportGenerator) {
        Write-Host "Instalando ReportGenerator..." -ForegroundColor Yellow
        dotnet tool install -g dotnet-reportgenerator-globaltool
    }
    
    # Buscar archivo de cobertura
    $coverageFile = Get-ChildItem -Path . -Recurse -Filter "coverage.cobertura.xml" | Select-Object -First 1
    
    if ($coverageFile) {
        reportgenerator "-reports:$($coverageFile.FullName)" "-targetdir:coveragereport" "-reporttypes:Html"
        
        Write-Host "Abriendo reporte de cobertura..." -ForegroundColor Green
        Start-Process "coveragereport\index.html"
    } else {
        Write-Host "No se encontró archivo de cobertura" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Ejecución Completada" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
