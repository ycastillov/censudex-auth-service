# Prerequisites

- .NET 8 o superior
- Docker y Docker Compose

## Instalación y ejecución

### 1. Clonar el repositorio
\`\`\`bash
git clone <repo-url>
cd censudex-auth-service
\`\`\`

### 2. Configurar variables de entorno
\`\`\`bash
cp .env.example .env
# Editar .env con tus valores (desarrollo usa valores por defecto)
\`\`\`

### 3. Iniciar Redis con Docker Compose
\`\`\`bash
docker-compose up -d
\`\`\`

Verifica que Redis está corriendo:
\`\`\`bash
docker-compose ps
# Output: censudex-redis should be "Up"
\`\`\`

### 4. Instalar dependencias y ejecutar
\`\`\`bash
dotnet restore
dotnet run
\`\`\`

La API estará disponible en: \`https://localhost:5001\`

### 5. Detener Redis
\`\`\`bash
docker-compose down
\`\`\`

## Troubleshooting

**Redis no conecta:**
\`\`\`bash
# Verificar logs
docker-compose logs redis

# Reiniciar
docker-compose restart redis
\`\`\`

**Puerto 6379 en uso:**
Edita \`docker-compose.yml\` y cambia \`"6379:6379"\` a \`"6380:6379"\` (o puerto disponible).
Luego actualiza \`REDIS_CONNECTION\` en \`.env\`.