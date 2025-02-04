# Etapa 1: Construcción de la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código y construir la aplicación
COPY . ./
RUN dotnet publish -c Release -o /out

# Etapa 2: Imagen final con solo el runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar los archivos compilados desde la etapa anterior
COPY --from=build /out ./

# Especificar el puerto en el que corre la app
EXPOSE 8080

# Comando para ejecutar la aplicación
CMD ["dotnet", "rp_api.dll"]
