#!/bin/bash
# Install .NET
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.0-preview.1 --install-dir ./dotnet

export PATH="$PATH:$(pwd)/dotnet"

# Publish
./dotnet/dotnet publish -c Release -o out
