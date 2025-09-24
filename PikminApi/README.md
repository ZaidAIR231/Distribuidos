# Pikmin SOAP API

API SOAP para gestionar Pikmin: asigna capitán, color, cuántos hay en la **cebolla** y su hábitat.
Incluye `createPikmin` y `getPikminById`.

## Requisitos

* Java 17 (solo si vas a compilar localmente; con Docker no es necesario)
* Insomnia (o Postman) para probar SOAP
* Docker y Docker Compose

## Tecnología (desarrollo)

* Java 17 • Spring Boot • Spring Web Services (SOAP)
* Hibernate (JPA) • PostgreSQL 15
* Flyway (migraciones) • JAXB (XSD → clases)
* Docker / Docker Compose

---

## Instalación y arranque

### 1) Clonar la rama del PR

```bash
git clone -b feature/PikminApi --single-branch https://github.com/ZaidAIR231/Distribuidos.git
cd Distribuidos/PikminApi
```

### 2) Levantar con Docker Compose

```bash
docker-compose up -d
```

Verifica que ambos servicios estén **Up**:

```bash
docker-compose ps
```

Ejemplo de salida esperada:

```
NAME            IMAGE           COMMAND                  STATE   PORTS
olimar-postgres postgres:15     "docker-entrypoint.s…"   Up      0.0.0.0:3315->5432/tcp
pikmin-api      pikmins-api     "java -jar /app/app…"   Up      0.0.0.0:8088->8081/tcp
```

La aplicación escucha **dentro** del contenedor en **8081**, pero está publicada en tu máquina en **[http://localhost:8088](http://localhost:8088)**.

### 3) WSDL

WSDL: `http://localhost:8088/ws/pikmin.wsdl`
Endpoint SOAP (para enviar requests): `http://localhost:8088/ws`  ← Ojo: no es la URL del WSDL.

---

## Probar con Insomnia

1. Abre Insomnia → Import.
2. Elige URL e introduce: `http://localhost:8088/ws/pikmin.wsdl`.
3. Pulsa Scan, verás 2 requests (create y get). Haz Import.

Luego, en cada request:

* En la parte superior, asegúrate que la URL sea `http://localhost:8088/ws` (no la del WSDL).
* Headers: `Content-Type: text/xml;charset=UTF-8` (SOAPAction no es necesario).

Verás las dos operaciones:

* `createPikmin`
* `getPikminById`

---

## Ejemplos de uso

### 1) `createPikmin`

**Body (Insomnia / SOAP):**

```xml
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:sch="http://pikmin.onion.com/ws">
  <soapenv:Header/>
  <soapenv:Body>
    <sch:createPikminRequest>
      <sch:pikmin>
        <sch:captainName>Olimar</sch:captainName>
        <sch:color>Red</sch:color>
        <sch:onionCount>30</sch:onionCount>
        <sch:habitat>Forest of Hope</sch:habitat>
      </sch:pikmin>
    </sch:createPikminRequest>
  </soapenv:Body>
</soapenv:Envelope>
```

**Respuesta esperada:**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <ns2:createPikminResponse xmlns:ns2="http://pikmin.onion.com/ws">
      <ns2:pikmin>
        <ns2:id>9a4c3b57-0bfc-4a24-8d37-8f1aa65f5b1f</ns2:id>
        <ns2:captainName>Olimar</ns2:captainName>
        <ns2:color>Red</ns2:color>
        <ns2:onionCount>30</ns2:onionCount>
        <ns2:habitat>Forest of Hope</ns2:habitat>
      </ns2:pikmin>
    </ns2:createPikminResponse>
  </soap:Body>
</soap:Envelope>
```

### 2) `getPikminById`

**Body:**

```xml
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:sch="http://pikmin.onion.com/ws">
  <soapenv:Header/>
  <soapenv:Body>
    <sch:getPikminByIdRequest>
      <sch:id>9a4c3b57-0bfc-4a24-8d37-8f1aa65f5b1f</sch:id>
    </sch:getPikminByIdRequest>
  </soapenv:Body>
</soapenv:Envelope>
```

**Respuesta (si existe):**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <ns2:getPikminByIdResponse xmlns:ns2="http://pikmin.onion.com/ws">
      <ns2:pikmin>
        <ns2:id>9a4c3b57-0bfc-4a24-8d37-8f1aa65f5b1f</ns2:id>
        <ns2:captainName>Olimar</ns2:captainName>
        <ns2:color>Red</ns2:color>
        <ns2:onionCount>30</ns2:onionCount>
        <ns2:habitat>Forest of Hope</ns2:habitat>
      </ns2:pikmin>
    </ns2:getPikminByIdResponse>
  </soap:Body>
</soap:Envelope>
```

**Fault (si no existe o el `id` no es válido):**

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <soap:Fault>
      <faultcode>soap:Server</faultcode>
      <faultstring>Pikmin not found: ...</faultstring>
    </soap:Fault>
  </soap:Body>
</soap:Envelope>
```

---

## Notas, restricciones y datos importantes

* Campos obligatorios en `createPikmin`:

  * `captainName` (máx. 100 chars)
  * `color` (máx. 40 chars)
  * `onionCount` (int ≥ 0)
  * `habitat` (máx. 100 chars)

* Base de datos: PostgreSQL 15. La tabla se crea con Flyway (migración `V1__create_pikmins_table.sql`) al levantar la app.

* Puertos:

  * App: host 8088 → contenedor 8081
  * DB: host 3315 → contenedor 5432

* Tiempo de arranque: la primera vez puede tardar un poco (descarga imágenes, crea DB y migra).

