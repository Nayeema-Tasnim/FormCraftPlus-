#!/bin/bash
set -e

echo "Installing .NET 9.0.100 SDK..."
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet

export PATH="$(pwd)/dotnet:$PATH"

echo "SDK version: $(dotnet --version)"

echo "Restoring NuGet packages..."
dotnet restore

echo "Ensuring EF Core CLI is available as a local tool..."
# Create (or overwrite) the tool manifest
dotnet new tool-manifest --force
dotnet tool install dotnet-ef --version 8.0.4
export PATH="$PATH:$HOME/.dotnet/tools"
dotnet tool restore

echo "Applying EF Core migrations to the database..."
dotnet ef database update --no-build --connection "$CONNECTION_STRING"

echo "Publishing the project..."
dotnet publish -c Release -o out

echo "✅ Build script complete."
