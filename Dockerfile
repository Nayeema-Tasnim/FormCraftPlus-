# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /app

# copy csproj and restore as a distinct layer
COPY ["finalproject.csproj", "./"]
RUN dotnet restore "./finalproject.csproj"

# copy everything else and build
COPY . .
RUN dotnet publish "finalproject.csproj" -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
EXPOSE 80
ENTRYPOINT ["dotnet", "finalproject.dll"]
