#!/bin/bash

set -e

echo "Installing .NET 9.0.100 SDK..."
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet

export PATH="$(pwd)/dotnet:$PATH"

echo "Restoring..."
./dotnet/dotnet restore

echo "Installing dotnet-ef locally..."
./dotnet/dotnet new tool-manifest  # Only needed once
./dotnet/dotnet tool install dotnet-ef --version 8.0.4

echo "Updating database..."
./dotnet/dotnet tool run dotnet-ef database update

echo "Publishing..."
./dotnet/dotnet publish -c Release -o out

echo "✅ Build complete!"
