# Використовуйте офіційний образ .NET SDK для стадії збірки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Копіюйте .csproj та відновлюйте як окремий шар, це дозволяє Docker кешувати відновлення як окремий шар
COPY *.sln .
COPY Fintracker.API/*.csproj ./Fintracker.API/
COPY Fintracker.Application/*.csproj ./Fintracker.Application/
COPY Fintracker.Domain/*.csproj ./Fintracker.Domain/
COPY Fintracker.Identity/*.csproj ./Fintracker.Identity/
COPY Fintracker.Infrastructure/*.csproj ./Fintracker.Infrastructure/
COPY Fintracker.Persistence/*.csproj ./Fintracker.Persistence/
COPY Fintracker.TEST/*.csproj ./Fintracker.TEST/
RUN dotnet restore

# Копіюйте все і збирайте
COPY . .
RUN dotnet publish -c Release -o out

# Використовуйте офіційний образ .NET для стадії розгортання
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Встановіть змінні середовища
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /out .
ENTRYPOINT ["dotnet", "Fintracker.API.dll"]
