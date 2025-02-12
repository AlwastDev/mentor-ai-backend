FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Auth/Auth.API/Auth.API.csproj", "Services/Auth/Auth.API/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["Services/Auth/Auth.Application/Auth.Application.csproj", "Services/Auth/Auth.Application/"]
COPY ["Services/Auth/Auth.Domain/Auth.Domain.csproj", "Services/Auth/Auth.Domain/"]
COPY ["Services/Auth/Auth.Infrastructure/Auth.Infrastructure.csproj", "Services/Auth/Auth.Infrastructure/"]
RUN dotnet restore "./Services/Auth/Auth.API/./Auth.API.csproj"
COPY . .
WORKDIR "/src/Services/Auth/Auth.API"
RUN dotnet build "./Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.API.dll"]