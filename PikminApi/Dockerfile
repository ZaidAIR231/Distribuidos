# Dockerfile para construir SOLO el Servidor SOAP (PikminApi)

# ====== ETAPA BUILDER ======
FROM maven:3.9.9-eclipse-temurin-17 AS builder
WORKDIR /app
# El pom.xml ya debe estar en la carpeta PikminApi
COPY pom.xml .
RUN mvn -B dependency:go-offline
COPY src ./src
# Ejecuta el build, incluyendo el JAXB plugin si lo tiene
RUN mvn -B clean package -DskipTests

# ====== ETAPA RUNTIME (TARGET FINAL) ======
FROM eclipse-temurin:17-jre
WORKDIR /app
COPY --from=builder /app/target/*.jar /app/app.jar
EXPOSE 8081
ENTRYPOINT ["java","-jar","/app/app.jar"]
