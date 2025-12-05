./clean-repository.sh

dotnet restore /workspaces/common/src/Common.Util/Common.Util.csproj
rm -f /local-nuget/RobinEpple.Common.Util.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Util/Common.Util.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Util/Common.Util.csproj -o /local-nuget

dotnet restore /workspaces/common/src/Common.SourceGenerators.Abstractions/Common.SourceGenerators.Abstractions.csproj
rm -f /local-nuget/RobinEpple.Common.SourceGenerators.Abstractions.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.SourceGenerators.Abstractions/Common.SourceGenerators.Abstractions.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.SourceGenerators.Abstractions/Common.SourceGenerators.Abstractions.csproj -o /local-nuget

dotnet restore /workspaces/common/src/Common.SourceGenerators/Common.SourceGenerators.csproj
rm -f /local-nuget/RobinEpple.Common.SourceGenerators.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.SourceGenerators/Common.SourceGenerators.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.SourceGenerators/Common.SourceGenerators.csproj -o /local-nuget

dotnet restore /workspaces/common/src/Common.Forms.Wrappers.Abstractions/Common.Forms.Wrappers.Abstractions.csproj
rm -f /local-nuget/RobinEpple.Common.Forms.Wrappers.Abstractions.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Forms.Wrappers.Abstractions/Common.Forms.Wrappers.Abstractions.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Forms.Wrappers.Abstractions/Common.Forms.Wrappers.Abstractions.csproj -o /local-nuget

dotnet restore /workspaces/common/src/Common.Forms/Common.Forms.csproj
rm -f /local-nuget/RobinEpple.Common.Forms.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Forms/Common.Forms.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Forms/Common.Forms.csproj -o /local-nuget

dotnet restore /workspaces/common/src/Common.Forms.Wrappers/Common.Forms.Wrappers.csproj
rm -f /local-nuget/RobinEpple.Common.Forms.Wrappers.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Forms.Wrappers/Common.Forms.Wrappers.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Forms.Wrappers/Common.Forms.Wrappers.csproj -o /local-nuget

dotnet restore src