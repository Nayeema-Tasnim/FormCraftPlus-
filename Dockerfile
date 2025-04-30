# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore
COPY . ./
RUN dotnet restore "./finalproject.csproj"

# Build and publish
RUN dotnet publish "./finalproject.csproj" -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Expose port
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "finalproject.dll"]
