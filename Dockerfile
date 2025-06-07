# 1. SDK do kompilacji
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Skopiuj tylko pliki projektowe na start, żeby przyspieszyć restore
COPY ["HrApp.MVC/HrApp.MVC.csproj", "HrApp.MVC/"]
COPY ["HrApp.Application/HrApp.Application.csproj", "HrApp.Application/"]
COPY ["HrApp.Domain/HrApp.Domain.csproj", "HrApp.Domain/"]
COPY ["HrApp.Infrastructure/HrApp.Infrastructure.csproj", "HrApp.Infrastructure/"]

# Restore zależności
RUN dotnet restore "HrApp.MVC/HrApp.MVC.csproj"

# Skopiuj resztę
COPY . .

WORKDIR /src/HrApp.MVC
RUN dotnet publish -c Release -o /app/publish

# 2. Obraz finalny
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HrApp.MVC.dll"]
