#!/bin/bash
set -e  # Exit immediately if any command fails

./clean-repository.sh

# Common.Util
dotnet restore /workspaces/common/src/Common.Util/Common.Util.csproj
rm -f /local-nuget/RobinEpple.Common.Util.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Util/Common.Util.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Util/Common.Util.csproj -o /local-nuget

# Common.Caching.Abstractions
dotnet restore /workspaces/common/src/Common.Caching.Abstractions/Common.Caching.Abstractions.csproj
rm -f /local-nuget/RobinEpple.Common.Caching.Abstractions.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Caching.Abstractions/Common.Caching.Abstractions.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Caching.Abstractions/Common.Caching.Abstractions.csproj -o /local-nuget

# Common.Caching
dotnet restore /workspaces/common/src/Common.Caching/Common.Caching.csproj
rm -f /local-nuget/RobinEpple.Common.Caching.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Caching/Common.Caching.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Caching/Common.Caching.csproj -o /local-nuget

# Common.SourceGenerators.Abstractions
dotnet restore /workspaces/common/src/Common.SourceGenerators.Abstractions/Common.SourceGenerators.Abstractions.csproj
rm -f /local-nuget/RobinEpple.Common.SourceGenerators.Abstractions.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.SourceGenerators.Abstractions/Common.SourceGenerators.Abstractions.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.SourceGenerators.Abstractions/Common.SourceGenerators.Abstractions.csproj -o /local-nuget

# Common.SourceGenerators
dotnet restore /workspaces/common/src/Common.SourceGenerators/Common.SourceGenerators.csproj
rm -f /local-nuget/RobinEpple.Common.SourceGenerators.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.SourceGenerators/Common.SourceGenerators.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.SourceGenerators/Common.SourceGenerators.csproj -o /local-nuget

# Common.Forms.Wrappers.Abstractions
dotnet restore /workspaces/common/src/Common.Forms.Wrappers.Abstractions/Common.Forms.Wrappers.Abstractions.csproj
rm -f /local-nuget/RobinEpple.Common.Forms.Wrappers.Abstractions.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Forms.Wrappers.Abstractions/Common.Forms.Wrappers.Abstractions.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Forms.Wrappers.Abstractions/Common.Forms.Wrappers.Abstractions.csproj -o /local-nuget

# Common.Forms
dotnet restore /workspaces/common/src/Common.Forms/Common.Forms.csproj
rm -f /local-nuget/RobinEpple.Common.Forms.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Forms/Common.Forms.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Forms/Common.Forms.csproj -o /local-nuget

# Common.Forms.Wrappers
dotnet restore /workspaces/common/src/Common.Forms.Wrappers/Common.Forms.Wrappers.csproj
rm -f /local-nuget/RobinEpple.Common.Forms.Wrappers.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Forms.Wrappers/Common.Forms.Wrappers.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Forms.Wrappers/Common.Forms.Wrappers.csproj -o /local-nuget

# Common.Html
dotnet restore /workspaces/common/src/Common.Html/Common.Html.csproj
rm -f /local-nuget/RobinEpple.Common.Html.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Html/Common.Html.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Html/Common.Html.csproj -o /local-nuget

# Common.Forms.Html
dotnet restore /workspaces/common/src/Common.Forms.Html/Common.Forms.Html.csproj
rm -f /local-nuget/RobinEpple.Common.Forms.Html.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.Forms.Html/Common.Forms.Html.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.Forms.Html/Common.Forms.Html.csproj -o /local-nuget

# Common.WebUi
dotnet restore /workspaces/common/src/Common.WebUi/Common.WebUi.csproj
rm -f /local-nuget/RobinEpple.Common.WebUi.[0-9]*.nupkg
dotnet build /workspaces/common/src/Common.WebUi/Common.WebUi.csproj --configuration="Release"
dotnet pack /workspaces/common/src/Common.WebUi/Common.WebUi.csproj -o /local-nuget

dotnet restore src