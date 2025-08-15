FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Filedownload.csproj", "./"]
RUN dotnet restore "Filedownload.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Filedownload.csproj" -c ${configuration} -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Filedownload.csproj" -c ${configuration} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Filedownload.dll"]