#!/bin/bash
# Script para ejecutar pruebas unitarias del proyecto LoginBackEnd
# Uso: ./run-tests.sh [--coverage] [--detailed] [--open-report]

# Colores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Variables
COVERAGE=false
DETAILED=false
OPEN_REPORT=false

# Procesar argumentos
for arg in "$@"
do
    case $arg in
        --coverage)
            COVERAGE=true
            shift
            ;;
        --detailed)
            DETAILED=true
            shift
            ;;
        --open-report)
            OPEN_REPORT=true
            shift
            ;;
        *)
            echo -e "${RED}Argumento desconocido: $arg${NC}"
            echo "Uso: ./run-tests.sh [--coverage] [--detailed] [--open-report]"
            exit 1
            ;;
    esac
done

echo -e "${CYAN}========================================"
echo "  LoginBackEnd - Ejecución de Pruebas  "
echo -e "========================================${NC}"
echo ""

# Limpiar salidas anteriores
echo -e "${YELLOW}Limpiando build anterior...${NC}"
dotnet clean --verbosity quiet

# Restaurar paquetes
echo -e "${YELLOW}Restaurando paquetes NuGet...${NC}"
dotnet restore --verbosity quiet

# Compilar
echo -e "${YELLOW}Compilando proyecto...${NC}"
if ! dotnet build --no-restore --verbosity quiet; then
    echo -e "${RED}Error en la compilación. Abortando...${NC}"
    exit 1
fi

echo ""
echo -e "${GREEN}========================================"
echo "  Ejecutando Pruebas Unitarias"
echo -e "========================================${NC}"
echo ""

# Configurar parámetros de prueba
TEST_PARAMS="test --no-build"

if [ "$DETAILED" = true ]; then
    TEST_PARAMS="$TEST_PARAMS --verbosity detailed"
else
    TEST_PARAMS="$TEST_PARAMS --verbosity normal"
fi

if [ "$COVERAGE" = true ]; then
    echo -e "${YELLOW}Generando reporte de cobertura de código...${NC}"
    TEST_PARAMS="$TEST_PARAMS --collect:\"XPlat Code Coverage\""
fi

# Ejecutar pruebas
if eval "dotnet $TEST_PARAMS"; then
    echo ""
    echo -e "${GREEN}✅ Todas las pruebas pasaron exitosamente${NC}"
    TEST_RESULT=0
else
    echo ""
    echo -e "${RED}❌ Algunas pruebas fallaron${NC}"
    TEST_RESULT=1
fi

# Generar reporte HTML de cobertura si se solicitó
if [ "$COVERAGE" = true ] && [ "$OPEN_REPORT" = true ] && [ $TEST_RESULT -eq 0 ]; then
    echo ""
    echo -e "${YELLOW}Generando reporte HTML de cobertura...${NC}"
    
    # Verificar si reportgenerator está instalado
    if ! command -v reportgenerator &> /dev/null; then
        echo -e "${YELLOW}Instalando ReportGenerator...${NC}"
        dotnet tool install -g dotnet-reportgenerator-globaltool
        export PATH="$PATH:$HOME/.dotnet/tools"
    fi
    
    # Buscar archivo de cobertura
    COVERAGE_FILE=$(find . -name "coverage.cobertura.xml" | head -n 1)
    
    if [ -n "$COVERAGE_FILE" ]; then
        reportgenerator "-reports:$COVERAGE_FILE" "-targetdir:coveragereport" "-reporttypes:Html"
        
        echo -e "${GREEN}Abriendo reporte de cobertura...${NC}"
        
        # Detectar el sistema operativo y abrir el reporte
        case "$(uname -s)" in
            Darwin*)
                open coveragereport/index.html
                ;;
            Linux*)
                xdg-open coveragereport/index.html 2>/dev/null || \
                sensible-browser coveragereport/index.html 2>/dev/null || \
                echo -e "${YELLOW}Por favor, abre manualmente: coveragereport/index.html${NC}"
                ;;
            *)
                echo -e "${YELLOW}Por favor, abre manualmente: coveragereport/index.html${NC}"
                ;;
        esac
    else
        echo -e "${RED}No se encontró archivo de cobertura${NC}"
    fi
fi

echo ""
echo -e "${CYAN}========================================"
echo "  Ejecución Completada"
echo -e "========================================${NC}"

exit $TEST_RESULT
