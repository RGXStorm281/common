#!/bin/bash

# Remove cached files to make sure the container is initialized in a clean state
./clean-repository.sh

# Wait until 'dotnet' command is available
echo "Waiting for .NET installation..."
until command -v dotnet &> /dev/null
do
    sleep 1
done

# From here it is safe to call dotnet
echo "Dotnet is installed. Version:"
dotnet --version

# Initialize dotnet
echo "Initializing workloads..."
sudo dotnet workload update

echo "Restoring tools..."
dotnet tool restore --tool-manifest=/workspaces/common/src/.config/dotnet-tools.json

echo "Mounting local nuget folder..."
dotnet nuget add source /local-nuget -n local