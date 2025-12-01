FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

ENV MINOMBRE_ES=Milton

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["TrainerApi.csproj", "./"]
RUN dotnet restore "./TrainerApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TrainerApi.csproj" -c $configuration -o /app/build

RUN ls

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "TrainerApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrainerApi.dll"]