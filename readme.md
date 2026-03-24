# Commons NuGet library

Welcome! This repository contains the libraries that make up the foundation of my future private coding projects.

Currently it contains the following packages:

- **Common.Util**: A set of small utility classes with minimal dependencies, that are supposed to make your life easier coding in C#.
- **Common.Forms**: A comprehensive framework for modelling complex input forms. For further details please have a look at the dedicated readme file [readme.common.forms.md](readme.common.forms.md)

Each library has its own test project.

## How do I work with this codebase?

The development environment for this project is a DevContainer. In short this means, that the entire environment is defined in configuration files and startup scripts, and is executed in a docker container. For details about the technology see the documentation page [here](https://code.visualstudio.com/docs/devcontainers/containers).

There are three main benefits about this:

- Setting up the environment is as easy as spinning up the container, and all dependencies will be installed automatically.
- The container will behave equally on every system, whether it's Windows, MacOS or Linux. More specifically: The container itself runs Linux (usually Debian based), so the applications will feel right at home on Linux servers or even more so in a docker hosting environment.
- If you are done with the project, just delete the container, and no traces will be left on your system.

Enough about selling DevContainers, lets get to specifics on how to open this repository. You will need to install the following three tools:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/): Default installation will do. Make sure it is configured for Linux containers, but that is the default. Just launch it once and make sure you click through the setup until you see the dashboard. After that you won't need to touch it again.
- [Visual Studio Code](https://code.visualstudio.com): Also just the default installation is fine. The only extension you need is:
    - [Dev Containers](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers): This connects VS Code to docker, and lets you view and manage the created containers from inside VS Code. On MacOS you might get prompted if you want to allow communication from VS Code to other apps -> That is necessary for this extension to control docker.

All of these tools are also available in pretty much all packet managers, [Homebrew](https://brew.sh) for example even recognizes and installs VS Code extensions.

Once you have all of that installed, checkout the git repository, open the folder in VS Code and either wait for the popup prompting you to reopen it in a container, or click on the remote symbol "><" in the lower left corner and select "reopen in container" manually. That is also where you can exit the remote connection btw.

The initial spin-up might take a little, since it will download the Debian container base and run all the startup scripts. If you want to see the progress in more detail, just click on the progress panel in the lower right corner. When everything is booted up, you should see the C# extensions running, so there should be a new "Solution Explorer" in the files tab and a new Tab for unit tests should appear.

## Troubleshooting

DevContainers are a good step towards eliminating local dependencies, but reality always finds a way to mess things up. Here are a few things I have stumbled across:

- **Git and line endings**: Git has a very neat feature, where it will check out a repository with the line endings matching the host system and converts them back to the repository default when committing - cool, except if you then proceed to mount the folder into a Linux virtual machine and get lots of compile errors because of "unknown \r characters" on Windows. Hopefully I have managed to configure the repository appropriately now, such that this won't happen anymore, but if you run into an issue like that please let me know, so I can have a look why my settings didn't catch your case.
- **The test explorer is not recognizing the test projects**: That is still a mystery to me... the test explorer extension appears to sometimes miss test projects that are already registered in the solution. To fix that go to the solution explorer, right click and "Remove" the missing test project, then re-add it by right clicking the solution and selecting "add existing project". The test extension should now have registered the test project and it will remember it even after rolling back all changes in git... -\\\_(シ)\_/-

## NuGet

This project provides NuGet Packages. Since I have not yet proceeded to host them on some public platform, they currently live in a mounted folder called /local-nuget in the home directory of your host machine. To build the current version, go to the root directory "/workspaces/common" in the terminal of the dev container ("Terminal > New Terminal" while connected) and use the following command:

```
cd /workspaces/common
./clean-repository.sh
./build-local-nuget.sh
```

## Code style

This repository uses two greedy code formatters for code style:

- [csharpier](https://csharpier.com/docs/About) for C# code.
- [prettier](https://prettier.io) for everything else.
  They both format files on saving.

If some files are copied to the repo or updated externally, the following commands can be used to format all files:

```
cd /workspaces/common/src
dotnet csharpier .
```

## Metrics

For some fun code metrics install [cloc](https://github.com/AlDanial/cloc?tab=readme-ov-file#install-via-package-manager) (on the host machine), navigate to the git directory of this project (outside the container) and run the following command:

```
cloc src --out=lines_of_code.md --md --exclude-dir="bin,obj,lib"
```

The output will be printed to the file `lines_of_code.md`
