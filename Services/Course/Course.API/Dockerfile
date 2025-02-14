FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Course/Course.API/Course.API.csproj", "Services/Course/Course.API/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["Services/Course/Course.Application/Course.Application.csproj", "Services/Course/Course.Application/"]
COPY ["Services/Course/Course.Domain/Course.Domain.csproj", "Services/Course/Course.Domain/"]
COPY ["Services/Course/Course.Infrastructure/Course.Infrastructure.csproj", "Services/Course/Course.Infrastructure/"]
RUN dotnet restore "./Services/Course/Course.API/./Course.API.csproj"
COPY . .
WORKDIR "/src/Services/Course/Course.API"
RUN dotnet build "./Course.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Course.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Course.API.dll"]