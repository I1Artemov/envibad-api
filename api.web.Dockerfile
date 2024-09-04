FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EnviBad.API.Web/EnviBad.API.Web.csproj", "EnviBad.API.Web/"]
COPY ["EnviBad.API.Common/EnviBad.API.Common.csproj", "EnviBad.API.Common/"]
COPY ["EnviBad.API.Core/EnviBad.API.Core.csproj", "EnviBad.API.Core/"]
COPY ["EnviBad.API.Infrastructure/EnviBad.API.Infrastructure.csproj", "EnviBad.API.Infrastructure/"]
RUN dotnet restore "./EnviBad.API.Web/EnviBad.API.Web.csproj"
COPY . .
WORKDIR "/src/EnviBad.API.Web"
RUN dotnet build "./EnviBad.API.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EnviBad.API.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnviBad.API.Web.dll"]