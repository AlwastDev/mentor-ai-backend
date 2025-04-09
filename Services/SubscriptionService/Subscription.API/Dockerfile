FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Subscription/Subscription.API/Subscription.API.csproj", "Services/Subscription/Subscription.API/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["Services/Subscription/Subscription.Application/Subscription.Application.csproj", "Services/Subscription/Subscription.Application/"]
COPY ["Services/Subscription/Subscription.Domain/Subscription.Domain.csproj", "Services/Subscription/Subscription.Domain/"]
COPY ["Services/Subscription/Subscription.Infrastructure/Subscription.Infrastructure.csproj", "Services/Subscription/Subscription.Infrastructure/"]
RUN dotnet restore "./Services/Subscription/Subscription.API/./Subscription.API.csproj"
COPY . .
WORKDIR "/src/Services/Subscription/Subscription.API"
RUN dotnet build "./Subscription.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Subscription.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Subscription.API.dll"]