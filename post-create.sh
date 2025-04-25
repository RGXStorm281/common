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

echo "Installing and linking libmagic..."
sudo apt update
sudo apt install -y libmagic1
sudo apt install -y libmagic-dev

# Find libmagic.so.1
LIBMAGIC_PATH=$(find /usr -name "libmagic.so.1" 2>/dev/null | head -n 1)

if [ -n "$LIBMAGIC_PATH" ]; then
    echo "Found libmagic.so.1 at: $LIBMAGIC_PATH"

    # Decide where to place the symlink
    LINK_DIR="/usr/lib"

    # Check if it's already linked
    if [ ! -e "$LINK_DIR/libmagic-1.so" ]; then
        echo "Creating symlink at $LINK_DIR/libmagic-1.so"
        if sudo ln -s "$LIBMAGIC_PATH" "$LINK_DIR/libmagic-1.so"; then
            echo "Symlink created successfully."
        else
            echo "Failed to create symlink. You might need elevated permissions."
        fi
    else
        echo "Symlink already exists."
    fi
else
    echo "libmagic.so.1 not found. Make sure libmagic is installed."
fi

echo "Initialization complete"