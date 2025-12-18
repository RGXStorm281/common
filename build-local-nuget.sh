./clean-repository.sh

dotnet restore /workspaces/common/src/Common.Util/Common.Util.csproj
rm -f /local-nuget/RobinEpple.Common.Util.*.nupkg
dotnet build /workspaces/common/src/Common.Util/Common.Util.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Util/Common.Util.csproj -o /local-nuget

dotnet restore /workspaces/common/src/Common.SourceGenerators.Abstractions/Common.SourceGenerators.Abstractions.csproj
rm -f /local-nuget/RobinEpple.Common.SourceGenerators.Abstractions.*.nupkg
dotnet build /workspaces/common/src/Common.SourceGenerators.Abstractions/Common.SourceGenerators.Abstractions.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.SourceGenerators.Abstractions/Common.SourceGenerators.Abstractions.csproj -o /local-nuget

dotnet restore /workspaces/common/src/Common.SourceGenerators/Common.SourceGenerators.csproj
rm -f /local-nuget/RobinEpple.Common.SourceGenerators.*.nupkg
dotnet build /workspaces/common/src/Common.SourceGenerators/Common.SourceGenerators.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.SourceGenerators/Common.SourceGenerators.csproj -o /local-nuget

dotnet restore /workspaces/common/src/Common.Forms/Common.Forms.csproj
rm -f /local-nuget/RobinEpple.Common.Forms.*.nupkg
dotnet build /workspaces/common/src/Common.Forms/Common.Forms.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Forms/Common.Forms.csproj -o /local-nuget

dotnet restore src