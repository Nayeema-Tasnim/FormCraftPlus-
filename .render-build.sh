#!/bin/bash
set -e

# 1) Install .NET 9 SDK
curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet

# 2) Add to PATH
export PATH="$(pwd)/dotnet:$PATH"

echo "Using .NET SDK: $(dotnet --version)"

# 3) Restore packages
dotnet restore

# 4) Publish the app
dotnet publish -c Release -o out

echo "✅ Build complete"


