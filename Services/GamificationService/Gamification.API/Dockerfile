FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Gamification/Gamification.API/Gamification.API.csproj", "Services/Gamification/Gamification.API/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["Services/Gamification/Gamification.Application/Gamification.Application.csproj", "Services/Gamification/Gamification.Application/"]
COPY ["Services/Gamification/Gamification.Domain/Gamification.Domain.csproj", "Services/Gamification/Gamification.Domain/"]
COPY ["Services/Gamification/Gamification.Infrastructure/Gamification.Infrastructure.csproj", "Services/Gamification/Gamification.Infrastructure/"]
RUN dotnet restore "./Services/Gamification/Gamification.API/./Gamification.API.csproj"
COPY . .
WORKDIR "/src/Services/Gamification/Gamification.API"
RUN dotnet build "./Gamification.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gamification.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gamification.API.dll"]