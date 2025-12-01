# Nintendo Consoles gRPC Service

Servicio gRPC en Python para la gestión de consolas de Nintendo.
Este servicio es consumido por la API REST `piklopedia-api`.

Nota: Este servicio se levanta automáticamente mediante `docker-compose` desde el proyecto principal. No requiere ejecución independiente.

---

## Descripción

Implementación de un servicio gRPC en Python que administra consolas de Nintendo.
El servicio utiliza MongoDB como base de datos y expone operaciones CRUD mediante RPC unarios.

RPC implementados:

* Unary RPC:

  * `CreateConsole`
  * `GetConsoleById`
  * `UpdateConsole`
  * `DeleteConsole`
  * `ListConsoles`

Este servicio funciona exclusivamente como servidor gRPC; no expone endpoints HTTP.

---

## Stack Tecnológico

* Python 3.12
* gRPC (`grpcio`, `grpcio-tools`)
* MongoDB 6.0
* Motor (cliente async de MongoDB)
* Pydantic para validación de modelos

---

## Definición del Servicio (.proto)

Archivo: `proto/nintendo.proto`

```proto
service NintendoService {
  rpc CreateConsole(CreateConsoleRequest) returns (ConsoleResponse);
  rpc GetConsoleById(GetConsoleByIdRequest) returns (ConsoleResponse);
  rpc ListConsoles(ListConsolesRequest) returns (ListConsolesResponse);
  rpc UpdateConsole(UpdateConsoleRequest) returns (ConsoleResponse);
  rpc DeleteConsole(DeleteConsoleRequest) returns (DeleteConsoleResponse);
}
```

Mensajes principales:

```proto
message Console {
  string id = 1;
  string name = 2;
  string code_name = 3;
  int32 generation = 4;
  int32 release_year = 5;
  int32 discontinued_year = 6;
  string description = 7;
  repeated string regions = 8;
  bool portable = 9;
  string support_email = 10;
}
```

---

## Validaciones

Las validaciones se realizan mediante modelos Pydantic en `domain/models.py` y reglas de negocio en `application/services.py`.

| Campo                         | Regla                                                 | Código de Error gRPC |
| ----------------------------- | ----------------------------------------------------- | -------------------- |
| name                          | Obligatorio, mínimo 1 caracter                        | INVALID_ARGUMENT     |
| generation                    | Obligatorio, entero positivo                          | INVALID_ARGUMENT     |
| release_year                  | Obligatorio, año válido                               | INVALID_ARGUMENT     |
| support_email                 | Validación de correo si no está vacío                 | INVALID_ARGUMENT     |
| id                            | Debe ser un ObjectId válido                           | INVALID_ARGUMENT     |
| id inexistente                | Requerido para lectura/actualización                  | NOT_FOUND            |
| Duplicados (name + code_name) | No debe existir otra consola con la misma combinación | ALREADY_EXISTS       |

---

## Base de Datos

MongoDB (diferente al servicio SOAP de Pikmin que usa PostgreSQL).

Configuración:

* Puerto: 27017
* Base de datos: `nintendo_db`
* Colección: `consoles`

Esquema lógico:

```json
{
  "_id": "ObjectId",
  "name": "Nintendo Switch",
  "code_name": "NX",
  "generation": 8,
  "release_year": 2017,
  "discontinued_year": 0,
  "description": "...",
  "regions": ["JP", "NA", "EU"],
  "portable": true,
  "support_email": "support@nintendo.com"
}
```

Índices recomendados:

```js
db.consoles.createIndex({ name: 1, code_name: 1 }, { unique: true })
db.consoles.createIndex({ generation: 1 })
db.consoles.createIndex({ regions: 1 })
```

---

## Configuración

Variables de entorno:

```env
MONGO_URI=mongodb://mongo:27017
MONGO_DB_NAME=nintendo_db
MONGO_CONSOLES_COLLECTION=consoles
GRPC_HOST=0.0.0.0
GRPC_PORT=50051
LOG_LEVEL=INFO
```

---

## Docker

Este servicio se construye automáticamente utilizando el siguiente Dockerfile:

```dockerfile
FROM python:3.12-slim

WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY proto/ ./proto/
RUN python -m grpc_tools.protoc \
    -I./proto \
    --python_out=./nintendo_api/presentation \
    --grpc_python_out=./nintendo_api/presentation \
    ./proto/nintendo.proto

COPY nintendo_api/ ./nintendo_api/

EXPOSE 50051
CMD ["python", "-m", "nintendo_api.main"]
```

---

## Documentación Completa

Este README describe únicamente el microservicio gRPC.
La documentación de integración con REST y el uso dentro del sistema completo se encuentra en el archivo README del proyecto principal.

