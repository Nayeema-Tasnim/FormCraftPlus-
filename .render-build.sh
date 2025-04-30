#!/bin/bash
set -e

echo "Installing .NET 9.0.100 SDK..."
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet

export PATH="$(pwd)/dotnet:$PATH"

echo "Restoring..."
dotnet restore

echo "Publishing..."
dotnet publish -c Release -o out

echo "✅ Build complete!"
