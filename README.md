# Censudex Auth Service

Servicio de Autenticación — JWT + Redis + gRPC

El **Auth Service** es el componente responsable de gestionar la autenticación dentro de la plataforma Censudex. Es un servicio **HTTP** (no gRPC), especializado en:

* Validar credenciales de usuario
* Generar tokens JWT firmados con HS256
* Validar tokens para la API Gateway
* Manejar cierre de sesión mediante **blocklist con Redis**
* Comunicarse vía **gRPC** con el ClientService para validar usuarios

Este servicio se integra con:

* **ClientService (gRPC)** → para obtener datos del usuario y validar credenciales
* **API Gateway (HTTP)** → para validación de token en cada request protegida

---

## 📌 Características principales

### ✔ Inicio de sesión con JWT (HS256)

Genera un token con:

* `sub` → UserId
* `id` → UserId
* `role` → rol del usuario
* ClaimTypes.Role → necesario para autorizar en API Gateway
* `jti` → identificador único para revocación

### ✔ Validación de tokens

Usado exclusivamente por la API Gateway para verificar integridad del token.

### ✔ Cierre de sesión con Redis Blocklist

Cada token inválido se almacena temporalmente en Redis utilizando su `jti`.

### ✔ Roles soportados

* `CLIENT`
* `ADMIN`

---

# 🚀 Prerequisites

* **.NET 8** o superior
* **Docker + Docker Compose**

---

# 📂 Estructura del proyecto

```
censudex-auth-service/
│
├── Src/
│   ├── Controllers/
│   ├── Services/
│   ├── Interfaces/
│   ├── DTOs/
│   ├── Helpers/
│   └── ...
│
├── docker-compose.yml
├── .env.example
├── Program.cs
└── README.md
```

---

# ⚙️ Instalación y ejecución

## 1. Clonar el repositorio

```bash
git clone <repo-url>
cd censudex-auth-service
```

---

## 2. Configurar variables de entorno (archivo .env)

Se debe copiar los elementos dentro del archivo .env.example en el archivo .env para el correcto funcionamiento de la API.
**Contenido del `.env.example`**

```env
# JWT Configuration
JWT_KEY=your-super-secret-key-min-32-characters-long-for-HS256
JWT_ISSUER=censudex-auth-service
JWT_AUDIENCE=censudex-clients
JWT_EXPIRES=60

# Redis Configuration
REDIS_CONNECTION=localhost:6379

# gRPC Clients Service
GRPC_CLIENTS_URL=https://localhost:7181
```

---

## 3. Iniciar Redis con Docker Compose

```bash
docker-compose up -d
```

Verificar estado:

```bash
docker-compose ps
```

Debe aparecer:

```
censudex-redis   Up
```

---

## 4. Ejecutar el servicio

```bash
dotnet restore
dotnet run
```

La API estará disponible en:

```
https://localhost:5144
```

---

## 5. Detener Redis

```bash
docker-compose down
```

---

# 🔌 Integración con ClientService (gRPC)

Auth Service se conecta automáticamente usando la variable:

```
GRPC_CLIENTS_URL=https://localhost:7181
```

Debe coincidir con el puerto HTTPS expuesto por ClientService.

Importante:
AuthService usa el método:

```
rpc GetClientByIdentifier (GetClientByIdentifierRequest) returns (AuthClientResponse);
```

Es responsabilidad del ClientService devolver:

* id
* username
* email
* isActive
* passwordHash
* role

---

# 🔑 Endpoints

## 1. Login

`POST /api/auth/login`

### Request

```json
{
  "usernameOrEmail": "admin@mail.com",
  "password": "admin123"
}
```

### Response

```json
{
  "token": "xxxxx.yyyyy.zzzzz",
  "userId": "GUID",
  "role": "ADMIN"
}
```

---

## 2. Validate Token

`GET /api/auth/validate-token`

Headers:

```
Authorization: Bearer <token>
```

### Response OK

```json
{
  "isValid": true,
  "userId": "GUID",
  "role": "CLIENT"
}
```

---

## 3. Logout

`POST /api/auth/logout`

Headers:

```
Authorization: Bearer <token>
```

---

