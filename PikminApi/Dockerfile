FROM eclipse-temurin:17-jdk-alpine AS build
WORKDIR /app
RUN apk add --no-cache maven

COPY pom.xml .

COPY src ./src
RUN mvn -q -DskipTests package
RUN ls -la target/
FROM eclipse-temurin:17-jre-alpine
WORKDIR /app
COPY --from=build /app/target/*.jar /app/app.jar
EXPOSE 8081
ENTRYPOINT ["java","-jar","/app/app.jar"]
