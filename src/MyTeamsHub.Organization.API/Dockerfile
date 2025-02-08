# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MyTeamsHub.Organization.API/MyTeamsHub.Organization.API.csproj", "src/MyTeamsHub.Organization.API/"]
COPY ["src/MyTeamsHub.Core.Domain/MyTeamsHub.Core.Domain.csproj", "src/MyTeamsHub.Core.Domain/"]
COPY ["src/MyTeamsHub.Core.Application/MyTeamsHub.Core.Application.csproj", "src/MyTeamsHub.Core.Application/"]
COPY ["src/MyTeamsHub.Persistence.Core/MyTeamsHub.Persistence.Core.csproj", "src/MyTeamsHub.Persistence.Core/"]
COPY ["src/MyTeamsHub.Persistence.Models/MyTeamsHub.Persistence.Models.csproj", "src/MyTeamsHub.Persistence.Models/"]
COPY ["src/MyTeamsHub.Organization.API.Models/MyTeamsHub.Organization.API.Models.csproj", "src/MyTeamsHub.Organization.API.Models/"]
COPY ["src/MyTeamsHub.Persistence/MyTeamsHub.Persistence.csproj", "src/MyTeamsHub.Persistence/"]
RUN dotnet restore "./src/MyTeamsHub.Organization.API/MyTeamsHub.Organization.API.csproj"
COPY . .
WORKDIR "/src/src/MyTeamsHub.Organization.API"
RUN dotnet build "./MyTeamsHub.Organization.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyTeamsHub.Organization.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyTeamsHub.Organization.API.dll"]