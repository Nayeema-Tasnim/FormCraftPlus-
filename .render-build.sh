#!/bin/bash

set -e  # Stop script execution on any error

echo "Installing .NET 9.0.100 SDK..."
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet


export PATH="$PATH:$(pwd)/dotnet"


echo "Installing dotnet-ef tool..."
./dotnet/dotnet tool install --global dotnet-ef --version 9.0.0-preview.3.24172.9
export PATH="$PATH:$HOME/.dotnet/tools"

echo "Installed .NET version:"
./dotnet/dotnet --version

echo "Restoring dependencies..."
./dotnet/dotnet restore

echo "Applying database migrations..."
dotnet-ef database update

echo "Publishing the project..."
./dotnet/dotnet publish -c Release -o out

echo "Build complete!"
