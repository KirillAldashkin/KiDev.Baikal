[![Nuget](https://img.shields.io/nuget/v/kidev.baikal?style=plastic)](https://www.nuget.org/packages/KiDev.Baikal)
# KiDev.Baikal
**Use short F# scripts to define .NET projects and solutions!**

# How to use
1) Create and open a F# script file.
2) Import this library and `open` module:
```fs
#r "nuget: KiDev.Baikal, 0.3.0"
open KiDev.Baikal
```
3) Write definition of your solution:
```fs
Solution(__SOURCE_DIRECTORY__) // F# literal to specify the directory that contains this script
    |> AddProject(FS()
        |> TargetFramework "net6.0"
        |> OutputType Exe
        |> Compile [ Include "Program.fs" ])
    |> run
```
4) Run `dotnet fsi your_script.fsx`. This will generate `.*proj` and `.sln` files based on your script.

# Example
Script file for this project:
```fs
#r "nuget: KiDev.Baikal, 0.3.0"
open KiDev.Baikal

Solution(__SOURCE_DIRECTORY__)
    |> AddProject(FS()
        |> TargetFramework "netstandard2.0"
        |> NuGetPackaging "KiDev.Baikal" "0.3.0" (PackInfo
            |> Description "Use short F# scripts to define .NET projects and solutions!"
            |> License "MIT"
            |> Authors [ "AldashkinKirill" ])
        |> None [
            Include "KiDev.Baikal.fsx";
            Include "Examples/**";
            Include ".gitignore";
            Include "README.md";
            Include "LICENSE";
        ]
        |> Compile [
            Include "Types.fs";
            Include "SDKResolver.fs";
            Include "Projects.fs";
            Include "Projects.Depedencies.fs";
            Include "Projects.SourceItems.fs";
            Include "Projects.NuGet.fs";
            Include "XMLWriter.fs";
            Include "DefaultTasks.fs";
            Include "Solutions.fs";
            Include "Solutions.SlnGeneration.fs";
            Include "Runner.fs"
        ])
    |> run
```
Also look to `Examples\` folder.

# Status
Currently this is a prototype.
