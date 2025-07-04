./clean-repository.sh

dotnet restore /workspaces/common/src/Common.Util/Common.Util.csproj
rm -f /local-nuget/RobinEpple.Common.Util.*.nupkg
dotnet build /workspaces/common/src/Common.Util/Common.Util.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Util/Common.Util.csproj -o /local-nuget

dotnet restore /workspaces/common/src/Common.Forms/Common.Forms.csproj
rm -f /local-nuget/RobinEpple.Common.Forms.*.nupkg
dotnet build /workspaces/common/src/Common.Forms/Common.Forms.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Forms/Common.Forms.csproj -o /local-nuget

dotnet restore /workspaces/common/src/Common.WebUi/Common.WebUi.csproj
rm -f /local-nuget/RobinEpple.Common.WebUi.*.nupkg
dotnet build /workspaces/common/src/Common.WebUi/Common.WebUi.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.WebUi/Common.WebUi.csproj -o /local-nuget