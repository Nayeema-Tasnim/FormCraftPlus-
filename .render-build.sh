#!/bin/bash

set -e

echo "Installing .NET 9.0.100 SDK..."
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet

export PATH="$(pwd)/dotnet:$PATH"

echo "Installing dotnet-ef (v8.0.4)..."
./dotnet/dotnet tool install --global dotnet-ef --version 8.0.4
export PATH="$PATH:$HOME/.dotnet/tools"

echo "Restoring..."
./dotnet/dotnet restore

echo "Updating database..."
dotnet-ef database update

echo "Publishing..."
./dotnet/dotnet publish -c Release -o out

echo "✅ Build complete!"
