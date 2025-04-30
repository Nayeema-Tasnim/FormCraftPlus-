# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copy csproj and restore
COPY ["finalproject.csproj", "./"]
RUN dotnet restore "./finalproject.csproj"

# copy rest and publish
COPY . .
RUN dotnet publish "finalproject.csproj" -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
EXPOSE 80
ENTRYPOINT ["dotnet", "finalproject.dll"]
