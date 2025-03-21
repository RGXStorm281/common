# Robins home project nuget library

This is the project for building some reliable libraries to use in my home projects.

## NuGet

This project provides NuGet Packages. To build the current version, go to the root directory and use the following command in the terminal:

```
cd /workspaces/common
./buildLocalNuget.sh
```

## Code style

This repository uses two greedy code formatters for code style:

-   [csharpier](https://csharpier.com/docs/About) for C# code.
-   [prettier](https://prettier.io) for everything else.
    They both format each file on save.

If some files are copied to the repo or updated externally, the following commands can be used to format all files:

```
cd /workspaces/common/src
dotnet csharpier .
```

## Metrics

For some fun code metrics install [cloc](https://github.com/AlDanial/cloc?tab=readme-ov-file#install-via-package-manager), navigate to the git directory of this project (outside the container) and run the following command:

```
cloc src --out=lines_of_code.md --md --exclude-dir="bin,obj"
```

The output will be printed to the file `lines_of_code.md`
