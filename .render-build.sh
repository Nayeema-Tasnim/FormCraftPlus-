#!/bin/bash

set -e  # Stop on error

echo "Installing .NET 9.0.100 SDK..."
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet

# Add local .NET to PATH
export PATH="$(pwd)/dotnet:$PATH"

echo "Installed .NET version:"
dotnet --version

echo "Restoring project..."
dotnet restore

echo "Installing dotnet-ef locally..."
dotnet new tool-manifest --force
dotnet tool install dotnet-ef --version 9.0.0 --local

echo "Running database migrations..."
dotnet tool restore
dotnet ef database update

echo "Publishing the project..."
dotnet publish -c Release -o out

echo "Build complete!"
