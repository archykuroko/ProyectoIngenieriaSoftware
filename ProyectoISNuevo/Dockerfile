# 🔹 Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar archivos del proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo el código fuente y compilar la aplicación
COPY . ./
RUN dotnet publish -c Release -o /publish

# 🔹 Etapa 2: Ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copiar archivos de la etapa de compilación
COPY --from=build /publish .

# Exponer el puerto en el contenedor (5000 para HTTP)
EXPOSE 5000
ENV ASPNETCORE_URLS http://+:5000

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "ProyectoISNuevo.dll"]
