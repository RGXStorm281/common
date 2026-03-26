# Commons NuGet library

Hey, awesome that you're checking out my `commons` repository!

This is the place where I collect all the C# utility libraries that lay the foundation for my other home projects. But don't be fooled, there has gone some serious effort into making them, and I think there is some cool stuff to be found!

The design of my libraries follows three core values:

1. **Discoverability:** I love autocompletion in IDEs. In my opinion, member based autocompletion is one of OOPs biggest advantages, because it allows to get a quick and precise overview of available options. My libraries are designed to support discovery through autocompletion as good as possible:
    - They specifically incorporate structures that allow member autocompletion in the first place. For example, different implementations of an interface are often grouped in a static class in form of factory methods, such that available implementations are listed on member autocompletion on the static class.
    - Members are often grouped semantically through their naming, for example in my `FormBuilder` class all methods that add to the structure contain the prefix "With", while methods that configure the current node start with "Use". This allows easy semantic filtering by typing out the prefix.
2. **Reliability:** These libraries are meant to be the backbone of many applications and need to be reliable. To accommodate for that, the most important pieces were all developed in a strict test-first development cycle. This is mirrored in the nearly 400 unit tests covering the most important business logic. But to me, reliability also means a clear definition of what a piece of code can and can't do. I want to use intended features without worrying, but I also don't want to stumble into edge cases unknowingly. For that reason, I plan out complex libraries thoroughly and put great emphasis on the known limitations in my documentation.
3. **Extensibility:** My favorite coding pattern is the strategy pattern. It allows to mirror semantics in architecture, and therefore ensures extensibility conceptually. Most components in these libraries are decoupled with interfaces, such that you can add or replace functionality to your heart's contents!

If you keep these core principles in mind, I think you will have no trouble navigating my codebase. Feel free to look around!

Before we get to the fun part though, let's get the legal stuff out of the way.

## Licensing

This project uses dual licensing:

- **Source code** is licensed under Apache License 2.0, see [LICENSE](LICENSE)
- **Documentation texts** in `Common.Html` derived from MDN Web Docs are licensed under [CC BY-SA 2.5](https://creativecommons.org/licenses/by-sa/2.5/). Where applicable, source links are included in the code. See [LICENSE-MDN](LICENSE-MDN).

## So ... what's in the box?

### A super powerful form framework.

The centerpiece of this entire codebase is definitely my form framework in `Common.Forms`. It's a powerful modelling tool for input forms, that covers validation, visibility rules, data binding and much more. It is designed to handle complex use-cases like grids with in-place editing, polymorph model structures and even recursive form definitions. And yes, all of this is supported in model binding.

But don't worry, if all you need is a simple two-input login form, I got you covered! The flow api makes modeling a form easy and readable, and the source generator in `Common.Forms.Wrappers` will abstract away all the complex inner workings and lets you interact with your form as if it were a native C# class!

If I caught your interest, have a look at the more detailed documentation [here]()! At least checkout the dependencies section, one of them might need your attention.

In it's core, the form framework is designed to be platform independent, but since I am a web developer by heart, some helpers for handling the framework in ASP.NET can be found in `Common.WebUi` ([documentation](doc/common_web_ui.md)) and `Common.Forms.Html` contains some HTML rendering helpers. The later also builds on the second core library:

### A custom DSL for HTML rendering.

I have to be honest: While I love the core architecture of ASP.NET, I am starting to question the usefulness of the Razor Syntax. Mostly because tooling support is awful, for example auto formatting will always mess up the moment a page contains a little more than the bare minimum of C# code. I also never warmed up with tag helpers, I always felt like they were cumbersome to write and annoying to use.

That's why I took it upon myself to design a custom Domain Specific Language (DSL) based on static functions, found in `Common.Html`. This allows perfect tool supported formatting and easy extensibility by implementing the ASP.NET interface `IHtmlContent`. Being based on this interface, my rendering framework is also fully compatible with Razor, so mix and match to your hearts contents! You can find examples and further documentation [here](doc/common_html.md).

But this DSL would only be half as useful, if it weren't for the awesome documentation on the [Mozilla Developer Network (MDN)](https://developer.mozilla.org/de/). I have been using this documentation for years to learn and understand HTML and CSS. So the logical next step was, to bring this documentation closer to where I code - in the form of doc-comments! Whenever you write an HTML tag or attribute with my DSL, the doc-comment will serve you the first paragraph of the MDN documentation. This is also the reason for the dual-licensing of this project, I take absolutely no credit for the documentation text.

What I do take credit for, is for ...

### A little coding wizardry.

If you ever worked on a multi-target project with shared business logic between different components and platforms, you probably came across the pain of async overloads. I like async, but I don't like writing the same function twice. I am fed up with fixing the same bug twice - you come across a bug in debugging, fix it, and forget there's a second overload that also needs adjusting. Well, two months later the same procedure starts over...

So when I first read of C# source generators, I was honestly surprised there was absolutely no sign of an async overload generator anywhere. So I guess I had to do it myself ...

The generator in `Common.SourceGenerators` is designed to be opt-in, low maintenance and non-destructive. 99% of the async code I write consists of basic imperative control structures with some async function calls in between. The generator is specifically designed to handle exactly that: Search through the syntax tree, replace function calls with async overloads where available and fall back to the unmodified syntax tree if it hits an unknown section.

The generator can handle a lot, but of course it has limitations. So if you need to make use of complex async patterns or certain functions absolutely need to be called in async, I advice to proofread the generated code at least after the first generation, and to check out the more detailed documentation [here](doc/common_source_generators.md).

But what would a home project be without ...

### A dusty toolbox for all the miscellaneous helpers that accumulated over time.

The project `Common.Util` is meant to be exactly that: A low dependency toolkit that contains some small but useful code snippets. The biggest feature is probably the static `Comparison` class, which allows to compare two arbitrarily typed enumerables based on a key definition. You can find a more thorough list of contents [here](doc/common_util.md). And I fully admit that some of the contents are up to my personal preference in coding style, so don't be too upset when you have a different opinion ;)

I decided to move the managed cache implementation to its own package `Common.Caching` to keep dependencies minimal. You can find the dedicated documentation [here](doc/common_caching.md).

## Nice, how can I use this?

Uhm, a [nuget.org](https://www.nuget.org) release is planned, but not yet realized. So in the meantime, I guess you have to build the NuGet packages yourself. But don't worry, that's easy. You just need to run the script `build-local-nuget.sh` and will receive a folder ready to use as local NuGet source, that you can add to other projects using `dotnet nuget add source /path/to/folder -n local`. But I recommend you read through the following sections first and run the script in a container, because it will kill all dotnet processes in advance, and I don't want to be responsible for your broken system ^^.

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

The initial spin-up might take a little, since it will download the Debian container base and run all the startup scripts. If you want to see the progress in more detail, just click on the progress panel in the lower right corner. When everything is booted up, you should see the C# extensions running, so there should be a new "Solution Explorer" in the files tab and a new tab for unit tests should appear.

## Troubleshooting

DevContainers are a good step towards eliminating local dependencies, but reality always finds a way to mess things up. Here are a few things I have stumbled across:

- **Git and line endings**: Git has a very neat feature, where it will check out a repository with the line endings matching the host system and converts them back to the repository default on commit - cool, except if you then proceed to mount the folder into a Linux virtual machine and get lots of compile errors because of "unknown \r characters" on Windows. Hopefully I have managed to configure the repository appropriately now, such that this won't happen anymore, but if you run into an issue like that please let me know, so I can have a look why my settings didn't catch your case.
- **The test explorer is not recognizing the test projects**: That is still a mystery to me... Most of the times just clicking the refresh button will build the solution and show all tests afterwards. But sometimes the extension appears to miss test projects that have previously been registered in the solution. To fix that, go to the solution explorer, right click and "Remove" the missing test project, then re-add it by right clicking the solution and selecting "add existing project". The test extension should now have registered the test project and it will remember it even after rolling back all changes in git... -\\\_(シ)\_/-

## NuGet

This project provides NuGet Packages. Since I have not yet proceeded to host them on some public platform, they currently live in a mounted folder called /local-nuget in the home directory of your host machine. To build the current version, go to the root directory "/workspaces/common" in the terminal of the dev container ("Terminal > New Terminal" while connected) and use the provided build script:

```
cd /workspaces/common
./build-local-nuget.sh
```

Of course you can build them manually or just a subset you need. But this is a little annoying because the packages built upon each other, so they need to be built in the correct order to resolve dependencies successfully.

You might notice a disclaimer printed to the console when the script is executed, that all dotnet processes have been killed. That is, because the script clears the local NuGet source before building, and the VS Code language server really does not like that. It will start scanning the system repeatedly packages and re-evaluate the entire solution, which not only slows down the build, but can actually completely lock up or crash the container.

There is probably a more elegant solution, but killing dotnet processes does the job to let the build run uninterrupted. To reboot the language server it is enough to reload the VS code window by entering the command panel `CTRL+SHIFT+P`/`CMD+SHIFT+P` and selecting the option `Developer: Reload Window`. This re-initializes the connection to the docker container and reboots all necessary services.

## Code style

This repository uses two greedy code formatters for code style:

- [csharpier](https://csharpier.com/docs/About) for C# code.
- [prettier](https://prettier.io) for everything else.
  They both format files on saving.

Naming conventions are encoded in `src/.editorconfig`, but they mostly follow C# defaults, so nothing crazy going on here. If some files are copied to the repo or updated externally, the following commands can be used to format all files:

```
cd /workspaces/common/src
dotnet csharpier .
```

## Metrics

For some fun code metrics install [cloc](https://github.com/AlDanial/cloc?tab=readme-ov-file#install-via-package-manager) (on the host machine), navigate to the git directory of this project (outside the container) and run the following command:

```
cloc src --out=lines_of_code.md --md --exclude-dir="bin,obj,lib"
```

The output will be printed to the file `lines_of_code.md`.

### Happy Coding! :D
