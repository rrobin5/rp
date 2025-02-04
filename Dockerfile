# Usa la imagen base de .NET
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Usar una imagen SDK para construir
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copiar el archivo .sln y .csproj
COPY ["rp_api/rp_api.csproj", "rp_api/"]

# Restaurar las dependencias
RUN dotnet restore "rp_api/rp_api.csproj"

# Copiar el resto del código fuente
COPY . .

# Construir el proyecto
WORKDIR "/src/rp_api"
RUN dotnet build "rp_api.csproj" -c Release -o /app/build

# Publicar la aplicación
RUN dotnet publish "rp_api.csproj" -c Release -o /app/publish

# Usar la imagen base para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "rp_api.dll"]