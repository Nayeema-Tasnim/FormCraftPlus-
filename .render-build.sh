#!/bin/bash

set -e  # Stop script execution on any error

echo "Installing .NET 9.0.100 SDK..."
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet

# Add .NET to PATH
export PATH="$PATH:$(pwd)/dotnet"

# Log installed version
echo "Installed .NET version:"
./dotnet/dotnet --version

echo "Restoring dependencies..."
./dotnet/dotnet restore

echo "Applying database migrations..."
./dotnet/dotnet ef database update

echo "Publishing the project..."
./dotnet/dotnet publish -c Release -o out

echo "Build complete!"
