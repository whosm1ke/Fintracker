FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /src
COPY ["Fintracker.API/Fintracker.API.csproj", "Fintracker.API/"]
COPY ["Fintracker.Application/Fintracker.Application.csproj", "Fintracker.Application/"]
COPY ["Fintracker.Domain/Fintracker.Domain.csproj", "Fintracker.Domain/"]
COPY ["Fintracker.Identity/Fintracker.Identity.csproj", "Fintracker.Identity/"]
COPY ["Fintracker.Persistence/Fintracker.Persistence.csproj", "Fintracker.Persistence/"]
COPY ["Fintracker.Infrastructure/Fintracker.Infrastructure.csproj", "Fintracker.Infrastructure/"]
RUN dotnet restore "Fintracker.API/Fintracker.API.csproj"
COPY . .
WORKDIR "/src/Fintracker.API"
RUN dotnet build "Fintracker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ENV ASPNETCORE_ENVIRONMENT=Production
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Fintracker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fintracker.API.dll"]
