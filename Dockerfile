# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy all .csproj files and the .sln file to restore dependencies first.
COPY ["CongestionTaxCalculator.sln", "."]
COPY ["src/CongestionTaxCalculator.Api/CongestionTaxCalculator.Api.csproj", "src/CongestionTaxCalculator.Api/"]
COPY ["src/CongestionTaxCalculator.Application/CongestionTaxCalculator.Application.csproj", "src/CongestionTaxCalculator.Application/"]
COPY ["src/CongestionTaxCalculator.Domain/CongestionTaxCalculator.Domain.csproj", "src/CongestionTaxCalculator.Domain/"]
COPY ["src/CongestionTaxCalculator.Infrastructure/CongestionTaxCalculator.Infrastructure.csproj", "src/CongestionTaxCalculator.Infrastructure/"]

# Restore NuGet packages
RUN dotnet restore "src/CongestionTaxCalculator.Api/CongestionTaxCalculator.Api.csproj"

# Copy the rest of the source code
COPY . .

# Publish the API project in Release configuration. The output will be in /app/publish
WORKDIR "/src/src/CongestionTaxCalculator.Api"
RUN dotnet publish "CongestionTaxCalculator.Api.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Create the final, lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Copy the published output from the 'build' stage
COPY --from=build /app/publish .

# The default ASP.NET Core port inside the container is 8080.
# The base image already exposes this, but it's good practice to declare it.
EXPOSE 8080

# This is the command that will be run when the container starts
ENTRYPOINT ["dotnet", "CongestionTaxCalculator.Api.dll"]