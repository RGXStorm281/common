echo "Cleaning repository"
pkill dotnet
echo "|------------------------------------------------------------------------------------|"
echo "| All dotnet processes killed, please relaunch after the script has finished running |"
echo "|------------------------------------------------------------------------------------|"
find /workspaces/common/src -type d \( -name "bin" -o -name "obj" \) -path "*" -exec rm -rf {} +
dotnet nuget locals all --clear