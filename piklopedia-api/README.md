# Piklopedia API

API REST para la gestión de Pikmin y consolas de Nintendo.

* Los **Pikmin** se obtienen desde el servicio **SOAP** del parcial anterior.
* Las **Consolas de Nintendo** se gestionan mediante un cliente **gRPC** que consume el microservicio `nintendoapi`.
* La API está protegida con **OAuth2 / JWT** usando **Hydra**.
* Todo corre en **Docker Compose**.

---

## 1. Tecnologías principales

| Componente           | Tecnología                 |
| -------------------- | -------------------------- |
| Lenguaje / Framework | Java 17 + Spring Boot      |
| BD (Pikmin / SOAP)   | PostgreSQL                 |
| BD (Nintendo / gRPC) | MongoDB (en `nintendoapi`) |
| Cache                | Redis                      |
| Autenticación        | OAuth2 + JWT (Hydra)       |
| SOAP client          | Spring Web Services        |
| gRPC client          | grpc-java                  |
| Contenedores         | Docker + Docker Compose    |
| Docs                 | Swagger / OpenAPI          |

---

## 2. Puertos y servicios

| Servicio           | Puerto host |
| ------------------ | ----------- |
| PostgreSQL         | 5432        |
| PikminApi (SOAP)   | 8081        |
| Piklopedia API     | 8090        |
| Hydra Public       | 4444        |
| Hydra Admin        | 4445        |
| Redis              | 6379        |
| NintendoApi (gRPC) | 50051       |

---

## 3. Arranque con Docker

### 3.1 Clonar el proyecto

```bash
git clone -b feature/PikminApi --single-branch https://github.com/ZaidAIR231/Distribuidos.git
cd Distribuidos/piklopedia-api
```

### 3.2 Configurar ruta del servicio SOAP (PikminApi)

En `docker-compose.yml`, servicio `pikminapi`:

```yaml
build:
  context: C:/Users/<TU-USUARIO>/OneDrive/Documentos/repos/PikminApi
  dockerfile: Dockerfile
```

```yaml
  nintendoapi:
    build:
      context: C:/Users/greni/OneDrive/Documentos/repos/nintendoapi   # AJUSTA esta ruta 
```



### 3.3 Levantar todo

```bash
docker compose up -d --build
```

Verificar:

```bash
docker compose ps
```

---

## 4. Autenticación (obligatoria para Postman)

Todos los endpoints REST de esta API requieren **Bearer Token JWT** válido.

### 4.1 Crear cliente OAuth2 en Hydra (Postman o cualquier REST client)
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


En **Postman**:

* En cada request:

  * Tab **Authorization**
  * Type: `Bearer Token`
  * Token: pega `access_token`.

---

## 5. Swagger (opcional)

Puedes explorar y probar la API desde Swagger:

```
http://localhost:8090/swagger-ui/index.html
```

---

## 6. Uso en Postman – Pikmin (SOAP detrás)

Los Pikmin se obtienen desde el servicio SOAP a través del gateway REST.

### 6.1 Modelo básico de Pikmin

El modelo expuesto por la API (DTO) es algo como:

```json
{
  "id": "4fa0f20a-5bfe-4c20-8465-9c00338c54f6",
  "captainName": "Olimar",
  "color": "red",
  "onionCount": 20,
  "habitat": "Forest of Hope"
}
```

Para crear / actualizar, se usan DTOs sin `id`:

```json
{
  "captainName": "Olimar",
  "color": "red",
  "onionCount": 20,
  "habitat": "Forest of Hope"
}
```

### 6.2 Reglas y validaciones (Pikmin)

* `captainName`: obligatorio, no vacío.
* `color`: obligatorio, no vacío.
* `onionCount`: obligatorio, entero ≥ 0.
* `habitat`: opcional, pero si viene no debe ir vacío.
* `id` (path): UUID válido.
* No se permite duplicar ciertas combinaciones de negocio (por ejemplo, mismo `captainName + color`), se devuelve 409.

### 6.3 Endpoints Pikmin

| Método | Ruta         | Descripción             |
| ------ | ------------ | ----------------------- |
| GET    | /pikmin/{id} | Obtener Pikmin por UUID |
| GET    | /pikmin      | Listar Pikmin           |
| POST   | /pikmin      | Crear Pikmin            |
| PUT    | /pikmin/{id} | Reemplazar Pikmin       |
| PATCH  | /pikmin/{id} | Actualizar parcialmente |
| DELETE | /pikmin/{id} | Eliminar Pikmin         |

#### 6.3.1 POST /pikmin – Crear Pikmin

* URL: `http://localhost:8090/pikmin`
* Método: `POST`
* Headers:

  * `Authorization: Bearer <token>`
  * `Content-Type: application/json`
* Body (raw / JSON), ejemplo válido:

```json
{
  "captainName": "Olimar",
  "color": "red",
  "onionCount": 15,
  "habitat": "Forest of Hope"
}
```

Respuestas:

* 201 Created → devuelve el Pikmin con `id`.
* 400 VALIDATION_ERROR → campos obligatorios faltantes o inválidos.
* 409 RESOURCE_CONFLICT → si se viola una regla de negocio (duplicado).

#### 6.3.2 GET /pikmin/{id} – Obtener Pikmin por id

* URL: `http://localhost:8090/pikmin/{id}`
* Ejemplo:

```http
GET http://localhost:8090/pikmin/4fa0f20a-5bfe-4c20-8465-9c00338c54f6
Authorization: Bearer <token>
```

Respuestas:

* 200 OK → devuelve el Pikmin.
* 400 CONSTRAINT_VIOLATION → si `id` no es un UUID válido.
* 404 RESOURCE_NOT_FOUND → si no existe.

#### 6.3.3 GET /pikmin – Listar

* URL: `http://localhost:8090/pikmin`
* Params opcionales: `page`, `size`.
* Headers: `Authorization: Bearer <token>`

Ejemplo:

```http
GET http://localhost:8090/pikmin?page=0&size=10
Authorization: Bearer <token>
```

#### 6.3.4 PUT /pikmin/{id} – Reemplazar

Body debe contener todos los campos obligatorios:

```json
{
  "captainName": "Louie",
  "color": "blue",
  "onionCount": 10,
  "habitat": "Lake Area"
}
```

Respuestas típicas:

* 200 OK → Pikmin actualizado.
* 400 VALIDATION_ERROR / ILLEGAL_ARGUMENT.
* 404 RESOURCE_NOT_FOUND.

#### 6.3.5 PATCH /pikmin/{id} – Actualizar parcialmente

Body con uno o más campos:

```json
{
  "color": "white",
  "onionCount": 25
}
```

#### 6.3.6 DELETE /pikmin/{id}

```http
DELETE http://localhost:8090/pikmin/4fa0f20a-5bfe-4c20-8465-9c00338c54f6
Authorization: Bearer <token>
```

* 204 No Content → eliminado.
* 404 RESOURCE_NOT_FOUND → si no existía.

---

## 7. Uso en Postman – Nintendo Consoles (gRPC detrás)

Estos endpoints REST usan por dentro el cliente gRPC `NintendoGrpcGateway`, que invoca al servicio `NintendoService` en el microservicio `nintendoapi` (puerto 50051).
Desde Postman solo ves HTTP; la parte gRPC es transparente.

### 7.1 Endpoints Nintendo

| Método | Ruta                    | Descripción                      |
| ------ | ----------------------- | -------------------------------- |
| POST   | /nintendo/consoles      | Crear consola                    |
| GET    | /nintendo/consoles      | Listar consolas                  |
| GET    | /nintendo/consoles/{id} | Obtener consola por id (MongoId) |
| PUT    | /nintendo/consoles/{id} | Actualizar consola               |
| DELETE | /nintendo/consoles/{id} | Eliminar consola                 |

### 7.2 Modelo de consola (ConsoleDto)

Respuesta típica:

```json
{
  "id": "692cb9c7d82124576e6d1b3d",
  "name": "Nintendo Switch",
  "codeName": "NX",
  "generation": 8,
  "releaseYear": 2017,
  "description": "Consola híbrida de Nintendo",
  "portable": true,
  "supportEmail": "support@nintendo.com",
  "regions": ["JP", "NA", "EU"]
}
```

### 7.3 Reglas y validaciones (Nintendo Consoles)

En `CreateConsoleDto` / `UpdateConsoleDto` (con Bean Validation):

* `name`:

  * obligatorio, no vacío.
* `generation`:

  * obligatorio, entero > 0.
* `releaseYear`:

  * obligatorio, rango válido (por ejemplo, 1970–2100).
* `codeName`:

  * opcional o requerido según tu implementación; si viene, no debe estar vacío.
* `portable`:

  * opcional, booleano.
* `supportEmail`:

  * opcional, si viene no vacío debe ser un email válido.
* `regions`:

  * opcional, lista de strings no vacíos.

ID de consola (Mongo):

* `{id}` en path debe ser un **ObjectId** válido:

  * 24 caracteres hex (`[0-9a-fA-F]{24}`).
* Si no cumple:

  * 400 CONSTRAINT_VIOLATION o ILLEGAL_ARGUMENT.
* Si cumple pero no existe en Mongo / gRPC:

  * 404 RESOURCE_NOT_FOUND.

Reglas de negocio o gRPC:

* Datos inválidos → gRPC `INVALID_ARGUMENT` → HTTP 400.
* Consola duplicada → gRPC `ALREADY_EXISTS` → HTTP 409.
* ID inexistente → gRPC `NOT_FOUND` → HTTP 404.

### 7.4 POST /nintendo/consoles – Crear consola

* URL: `http://localhost:8090/nintendo/consoles`
* Método: `POST`
* Headers:

  * `Authorization: Bearer <token>`
  * `Content-Type: application/json`

Ejemplo 1 (válido):

```json
{
  "name": "Nintendo Switch",
  "codeName": "NX",
  "generation": 8,
  "releaseYear": 2017,
  "description": "Consola híbrida de Nintendo",
  "portable": true,
  "supportEmail": "support@nintendo.com",
  "regions": ["JP", "NA", "EU"]
}
```

Ejemplo 2 (sin `supportEmail`, también válido):

```json
{
  "name": "Nintendo 3DS",
  "codeName": "CTR",
  "generation": 7,
  "releaseYear": 2011,
  "description": "Portátil con efecto 3D sin gafas",
  "portable": true,
  "supportEmail": "",
  "regions": ["JP", "NA", "EU", "LATAM"]
}
```


Respuestas típicas:

* 201 Created → consola creada, con `id` de Mongo.
* 400 VALIDATION_ERROR → falta `name`, `generation` o `releaseYear`, o formato inválido.
* 409 RESOURCE_CONFLICT → si el gRPC indica consola duplicada.

### 7.5 GET /nintendo/consoles – Listar consolas

* URL: `http://localhost:8090/nintendo/consoles`
* Método: `GET`
* Headers: `Authorization: Bearer <token>`

Respuesta: lista de `ConsoleDto`.

### 7.6 GET /nintendo/consoles/{id} – Obtener por id

* URL: `http://localhost:8090/nintendo/consoles/{id}`

Ejemplo:

```http
GET http://localhost:8090/nintendo/consoles/692cb9c7d82124576e6d1b3d
Authorization: Bearer <token>
```

Respuestas:

* 200 OK → consola encontrada.
* 400 CONSTRAINT_VIOLATION → id no es un ObjectId válido (longitud distinta de 24, caracteres no hex).
* 404 RESOURCE_NOT_FOUND → no existe consola con ese id.

### 7.7 PUT /nintendo/consoles/{id} – Actualizar consola

* Todos los campos obligatorios (como en Create) deben venir en el body.

Ejemplo:

```json
{
  "name": "Nintendo Switch OLED",
  "codeName": "NX",
  "generation": 8,
  "releaseYear": 2021,
  "description": "Versión con pantalla OLED",
  "portable": true,
  "supportEmail": "support@nintendo.com",
  "regions": ["JP", "NA", "EU"]
}
```

Respuestas:

* 200 OK → consola actualizada.
* 400 VALIDATION_ERROR / ILLEGAL_ARGUMENT.
* 404 RESOURCE_NOT_FOUND.
* 409 RESOURCE_CONFLICT → si se viola una regla de unicidad.

### 7.8 DELETE /nintendo/consoles/{id}

```http
DELETE http://localhost:8090/nintendo/consoles/692cb9c7d82124576e6d1b3d
Authorization: Bearer <token>
```

Comportamiento:

* Si existe y se elimina: 204 No Content.
* Si ya no existe o nunca existió: 404 RESOURCE_NOT_FOUND.

---

## 8. Formato de errores (GlobalExceptionHandler)

Cuando ocurre un error, la API responde con un objeto `ApiError` similar a:

```json
{
  "status": 400,
  "error": "Bad Request",
  "code": "VALIDATION_ERROR",
  "message": "Request validation failed",
  "path": "/nintendo/consoles",
  "fieldErrors": {
    "name": "must not be blank",
    "generation": "must be greater than 0"
  }
}
```

Códigos de `code` posibles:

* `VALIDATION_ERROR` – DTOs con `@Valid` fallan.
* `CONSTRAINT_VIOLATION` – parámetros de path/query inválidos.
* `DATA_INTEGRITY_VIOLATION` – error de integridad en BD.
* `ILLEGAL_ARGUMENT` – reglas de negocio internas.
* `RESOURCE_CONFLICT` – conflictos (duplicados, etc.).
* `RESOURCE_NOT_FOUND` – recurso no encontrado.

---
