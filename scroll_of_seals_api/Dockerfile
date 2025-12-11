# Imagen base de ejecución 
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Variable de entorno 
ENV NINJA_NAME=Zaid
ENV ASPNETCORE_URLS=http://+:5000

USER app

# Etapa de compilación
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src


COPY ["scroll_of_seals_api.csproj", "./"]
RUN dotnet restore "./scroll_of_seals_api.csproj"


COPY . .

# Compilamos el proyecto
RUN dotnet build "scroll_of_seals_api.csproj" -c $configuration -o /app/build

# Etapa de publicación
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "scroll_of_seals_api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Imagen final 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Puerto gRPC 
EXPOSE 5000

ENTRYPOINT ["dotnet", "scroll_of_seals_api.dll"]
