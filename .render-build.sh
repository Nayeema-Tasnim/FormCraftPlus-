#!/bin/bash

# Install .NET 9.0.100 SDK
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet

# Add .NET to PATH
export PATH="$PATH:$(pwd)/dotnet"

# Log installed version
./dotnet/dotnet --version

# Publish the project
./dotnet/dotnet publish -c Release -o out
