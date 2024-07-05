# primeiro exeute isso: dotnet publish -c Release -o ./out


# Use the official .NET 8 runtime image to run the app
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# Copy the build output from the local machine to the Docker image
COPY /out .

# Entry point for the application
ENTRYPOINT ["dotnet", "CreateAndEditUser.dll"]


# depois do docker build execute; docker run -it --rm createandedituser
