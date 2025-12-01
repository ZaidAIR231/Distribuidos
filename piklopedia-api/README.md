# 🧪 Piklopedia API

Sistema de gestión de Pikmins.
Expone una **API REST** que permite registrar, consultar, actualizar y eliminar Pikmins.
Se conecta con el servicio **SOAP del Parcial 1**, usa **OAuth2 con Hydra**, cache con **Redis**, y está completamente dockerizado.

---

## ⚙️ Tecnologías

| Componente         | Tecnología                     |
|--------------------|--------------------------------|
| Lenguaje / Framework | Java 17 + Spring Boot         |
| Base de Datos (REST / SOAP) | PostgreSQL               |
| Cache              | Redis                           |
| Autenticación      | OAuth2 + JWT (Hydra de Ory)    |
| Comunicación SOAP  | Spring Web Services             |
| Contenedores       | Docker + Docker Compose        |
| Documentación      | Swagger / OpenAPI              |

---

## ✅ Requisitos Previos

- Docker Desktop
- Docker Compose v2+
- Puertos libres en tu máquina:

| Servicio        | Puerto |
|-----------------|--------|
| PostgreSQL      | 5432   |
| SOAP (PikminApi)| 8081   |
| API REST        | 8090   |
| Hydra Public    | 4444   |
| Hydra Admin     | 4445   |
| Redis           | 6379   |

---

## 📦 Instalación y Arranque

### 1️⃣ Clonar el proyecto

```bash
git clone -b feature/Piklopedia-api --single-branch https://github.com/ZaidAIR231/Distribuidos.git
cd Distribuidos/piklopedia-api
```

### 2️⃣ Configurar ruta del servicio SOAP

Editar `docker-compose.yml` → servicio `pikminapi`:

```yaml
build:
  context: C:/Users/<TU-USUARIO>/OneDrive/Documentos/repos/PikminApi
  dockerfile: Dockerfile
```

### 3️⃣ Levantar todo

```bash
docker compose up -d --build
```

### 4️⃣ Verificar

```bash
docker compose ps
```

---

## 🔐 Autenticación con Hydra

### ✅ 1. Crear Cliente OAuth2 (Postman → Body raw)

- URL: `http://localhost:4445/admin/clients`
- Método: `POST`
- Headers: `Content-Type: application/json`
- Body (raw → JSON):

```json
{
  "client_id": "rest-client",
  "client_secret": "secret",
  "grant_types": ["client_credentials"],
  "token_endpoint_auth_method": "client_secret_post",
  "scope": "read write"
}
```

### ✅ 2. Obtener Token (curl obligatorio)

```powershell
curl.exe -X POST http://localhost:4444/oauth2/token `
  -H "Content-Type: application/x-www-form-urlencoded" `
  -d "grant_type=client_credentials&client_id=rest-client&client_secret=secret&scope=read%20write"
```

---

## 📚 Documentación de API

Swagger disponible en:

```
http://localhost:8090/swagger-ui/index.html
```

---

## ✅ Endpoints Principales

| Método | Ruta                  | Descripción            |
|--------|------------------------|-------------------------|
| GET    | /pikmin/{id}          | Obtener Pikmin por ID  |
| GET    | /pikmin               | Listado paginado       |
| POST   | /pikmin               | Crear Pikmin           |
| PUT    | /pikmin/{id}          | Reemplazar Pikmin      |
| PATCH  | /pikmin/{id}          | Actualizar parcialmente|
| DELETE | /pikmin/{id}          | Eliminar Pikmin        |

---

## 🛠 Ejemplo de llamada con Token

```bash
curl -X GET http://localhost:8090/pikmin/1   -H "Authorization: Bearer <AQUÍ-TU-TOKEN>"
```

---

## 📌 Modelo de Datos (Ejemplo)

```json
{
  "id": "e3c0c1f3-1b22-4a6e-a49f-0f24b121fa72",
  "name": "Red Pikmin",
  "color": "red",
  "strength": 10,
  "weight": 1
}
```

# 📌 **Endpoints de la API Piklopedia**

## ✅ **1. Obtener Pikmin por ID**

**GET /pikmin/{id}**

| Descripción   | Obtiene un Pikmin específico por su UUID |
| ------------- | ---------------------------------------- |
| Autenticación | ✔ Requiere Token Bearer (OAuth2 - Hydra) |

📍 **Ejemplo de solicitud**

```
GET http://localhost:8090/pikmin/4fa0f20a-5bfe-4c20-8465-9c00338c54f6
Authorization: Bearer <token>
```

📤 **Respuesta 200 OK**

```json
{
  "id": "4fa0f20a-5bfe-4c20-8465-9c00338c54f6",
  "name": "Red Pikmin",
  "color": "red",
  "onionCount": 20,
  "habitat": "Forest of Hope",
  "createdAt": "2025-10-20T18:20:10Z",
  "updatedAt": "2025-10-20T18:30:10Z"
}
```

📤 **Respuesta 404 Not Found**

```json
{
  "status": 404,
  "error": "Not Found",
  "message": "Pikmin with id 4fa0f20a... not found",
  "path": "/pikmin/4fa0f20a-5bfe-4c20-8465-9c00338c54f6"
}
```

---

## ✅ **2. Listar todos los Pikmin**

**GET /pikmin**

| Descripción   | Lista de Pikmin con paginación opcional |
| ------------- | --------------------------------------- |
| Query Params  | `page`, `pageSize`, `sort`, `filter`    |
| Autenticación | ✔ Sí                                    |

📍 **Ejemplo**

```
GET http://localhost:8090/pikmin?page=0&pageSize=5
Authorization: Bearer <token>
```

📤 **Respuesta 200 OK**

```json
[
  {
    "id": "4fa0f20a-5bfe-4c20-8465-9c00338c54f6",
    "name": "Red Pikmin",
    "color": "red",
    "onionCount": 20,
    "habitat": "Forest of Hope"
  },
  {
    "id": "6ac0b20a-1dfe-4f30-3423-9c00367h54d3",
    "name": "Blue Pikmin",
    "color": "blue",
    "onionCount": 10,
    "habitat": "Lake Area"
  }
]
```

---

## ✅ **3. Crear un Pikmin**

**POST /pikmin**

| Descripción   | Crea un nuevo Pikmin |
| ------------- | -------------------- |
| Body          | ✔ JSON requerido     |
| Autenticación | ✔ Sí                 |

📥 **Body de solicitud**

```json
{
  "name": "Yellow Pikmin",
  "color": "yellow",
  "onionCount": 5,
  "habitat": "Thunder Plateau"
}
```

📤 **Respuesta 201 Created**

```json
{
  "id": "b8e5e243-f25c-4d28-9b2d-26d4a9c916ce",
  "name": "Yellow Pikmin",
  "color": "yellow",
  "onionCount": 5,
  "habitat": "Thunder Plateau"
}
```

---

## ✅ **4. Reemplazar un Pikmin**

**PUT /pikmin/{id}**

📥 **Body**

```json
{
  "name": "Updated Pikmin",
  "color": "purple",
  "onionCount": 12,
  "habitat": "Cave of Trials"
}
```

📤 **Respuesta 200 OK**

```json
{
  "id": "4fa0f20a-5bfe-4c20-8465-9c00338c54f6",
  "name": "Updated Pikmin",
  "color": "purple",
  "onionCount": 12,
  "habitat": "Cave of Trials"
}
```

---

## ✅ **5. Modificar parcialmente un Pikmin**

**PATCH /pikmin/{id}**

📥 **Body**

```json
{
  "color": "white",
  "onionCount": 25
}
```

📤 **Respuesta 200 OK**

```json
{
  "id": "4fa0f20a-5bfe-4c20-8465-9c00338c54f6",
  "name": "Updated Pikmin",
  "color": "white",
  "onionCount": 25,
  "habitat": "Cave of Trials"
}
```

---

## ✅ **6. Eliminar un Pikmin**

**DELETE /pikmin/{id}**

📍 **Ejemplo**

```
DELETE http://localhost:8090/pikmin/4fa0f20a-5bfe-4c20-8465-9c00338c54f6
Authorization: Bearer <token>
```

📤 **Respuesta 204 No Content**

📤 **Respuesta 404 Not Found**

```json
{
  "status": 404,
  "error": "Not Found",
  "message": "Pikmin not found"
}
```

---

## ✅ **7. Errores comunes**

| Código | Descripción                |
| ------ | -------------------------- |
| 400    | Validación fallida         |
| 401    | Token ausente o inválido   |
| 404    | Pikmin no encontrado       |
| 500    | Error interno del servidor |

## ⚠️ Errores Comunes

| Error | Causa | Solución |
|-------|-------|----------|
| 401 Unauthorized | Sin token o token inválido | Agregar Bearer Token |
| 403 Forbidden | Token sin permisos | Usar scope `read` o `write` |
| 500 SOAP error | Servicio SOAP caído | Verificar PikminApi en puerto 8081 |
| unhealthy en SOAP | Ruta mal configurada | Revisar ruta build context en docker-compose.yml |

---

## ✅ Notas 

- Todos los endpoints requieren token JWT válido۔
- Hydra gestiona autenticación y autorización।
- Swagger permite probar los endpoints con token।