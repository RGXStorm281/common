#!/bin/bash
echo "Waiting for .NET installation..."

# Wait until 'dotnet' command is available
until command -v dotnet &> /dev/null
do
    sleep 1
done

echo "Dotnet is installed. Version:"
# From here it is safe to call dotnet
dotnet --version

echo "Initializing workloads..."
sudo dotnet workload update

echo "Restoring tools..."
dotnet tool restore --tool-manifest=/workspaces/common/src/.config/dotnet-tools.json

echo "Mounting local nuget folder..."
dotnet nuget add source /local-nuget -n local

echo "Restoring solution..."
dotnet restore /workspaces/common/src