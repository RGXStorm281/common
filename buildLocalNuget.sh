dotnet build /workspaces/common/src/Common.Util/Common.Util.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Util/Common.Util.csproj -o /local-nuget
dotnet restore /workspaces/common/src