
set -e


curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --version 9.0.100 --install-dir ./dotnet


export PATH="$(pwd)/dotnet:$PATH"

echo "Using .NET SDK: $(dotnet --version)"


dotnet restore

dotnet publish -c Release -o out

echo "✅ Build complete"


