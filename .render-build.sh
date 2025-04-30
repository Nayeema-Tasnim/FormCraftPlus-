#!/bin/bash

# Install stable .NET 9 SDK
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet

# Add to PATH
export PATH="$PATH
