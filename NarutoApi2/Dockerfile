FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV MINOMBRE_ES=Zaid


USER app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["NarutoApi.csproj", "./"]
RUN dotnet restore "NarutoApi.csproj"
COPY . .
RUN ls
RUN dotnet build "NarutoApi.csproj" -c ${configuration} -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "NarutoApi.csproj" -c ${configuration} -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NarutoApi.dll"]