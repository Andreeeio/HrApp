# Ten etap służy do kompilowania projektu usługi
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HrApp.MVC/HrApp.MVC.csproj", "HrApp.MVC/"]
COPY ["HrApp.Application/HrApp.Application.csproj", "HrApp.Application/"]
COPY ["HrApp.Domain/HrApp.Domain.csproj", "HrApp.Domain/"]
COPY ["HrApp.Infrastructure/HrApp.Infrastructure.csproj", "HrApp.Infrastructure/"]
RUN dotnet restore "./HrApp.MVC/HrApp.MVC.csproj"
COPY . .
WORKDIR "/src/HrApp.MVC"
RUN dotnet build "./HrApp.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Ten etap służy do publikowania projektu usługi do skopiowania do etapu końcowego
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HrApp.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Ten etap jest używany w środowisku produkcyjnym lub w przypadku uruchamiania z programu VS w trybie regularnym (domyślnie, gdy nie jest używana konfiguracja debugowania)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HrApp.MVC.dll"]