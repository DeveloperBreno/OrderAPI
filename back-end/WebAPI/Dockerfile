FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY /out .
ENV ASPNETCORE_URLS=http://+:443
EXPOSE 443
ENTRYPOINT ["dotnet", "WebAPI.dll"]
