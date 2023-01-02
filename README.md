# KiDev.Baikal
**Use short F# scripts to define .NET projects and solutions!**

# How to use
1) Create a F# script file.
2) Import this library and `open` module.
3) Write definition of your solution.
4) Run `dotnet fsi your_script.fsx`. This will generate `.*proj` and `.sln` files based on your script.

# Example
Script file for this project
```fs
#r "bin/Debug/netstandard2.0/KiDev.Baikal.dll"
open KiDev.Baikal

Solution(__SOURCE_DIRECTORY__)
    |> AddProject(FS()
        |> TargetFramework "netstandard2.0"
        |> Prop "Description" "Use short F# scripts to define .NET projects and solutions!"
        |> Prop "PackageId" "KiDev.Baikal"
        |> Prop "PackageLicenseExpression" "MIT"
        |> Prop "Version" "0.2.2"
        |> Prop "Authors" "AldashkinKirill"
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
Currently this is a early prototype.