# ===== build =====
FROM eclipse-temurin:17-jdk-alpine AS build
WORKDIR /app

# Maven solo en build
RUN apk add --no-cache maven

COPY pom.xml .

COPY src ./src
RUN mvn -q -DskipTests package
RUN ls -la target/

# ===== runtime =====
FROM eclipse-temurin:17-jre-alpine
WORKDIR /app

# Copia el JAR final
COPY --from=build /app/target/*.jar /app/app.jar

# La app escucha en 8081 (por application.properties)
EXPOSE 8081

ENTRYPOINT ["java","-jar","/app/app.jar"]
