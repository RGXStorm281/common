echo "Cleaning repository"
find /workspaces/common/src -type d \( -name "bin" -o -name "obj" \) -path "*" -exec rm -rf {} +
dotnet nuget locals all --clear