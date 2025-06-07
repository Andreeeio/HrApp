FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "HrApp.MVC.csproj"
RUN dotnet publish "HrApp.MVC.csproj" -c Release -o /src/publish

FROM base AS final
WORKDIR /app
COPY --from=build /src/publish .
ENTRYPOINT ["dotnet", "HrApp.MVC.dll"]