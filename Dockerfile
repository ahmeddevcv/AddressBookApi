# Base image to run the ASP.NET Core application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Image used to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY . .
RUN dotnet restore

# Build and publish the application
RUN dotnet publish -c Release -o /app/publish

# Final image used to run the app
FROM base AS final
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "AddressBookApi.dll"]
