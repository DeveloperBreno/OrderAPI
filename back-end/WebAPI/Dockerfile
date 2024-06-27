# Usar a imagem base do .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o código-fonte do projeto para o contêiner
COPY . .

# Restaurar pacotes e compilar o projeto
#RUN dotnet restore
#RUN dotnet publish -c Release -o /out --runtime linux-x64

# Criar a imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY /out .

# Definir a variável de ambiente para o ASPNETCORE_URLS
ENV ASPNETCORE_URLS=http://+:5000

# Expor a porta 5000 para acesso externo
EXPOSE 5000

# Iniciar o aplicativo
ENTRYPOINT ["dotnet", "WebAPI.dll"]
