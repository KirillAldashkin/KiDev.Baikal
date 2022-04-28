# KiDev.Baikal
**Use short F# scripts to define .NET projects and solutions!**

# How to use
1) Create a F# script file.
2) Import this library and `open` module.
3) Write definition of your solution.
4) `dotnet fsi solution.fsx`. This will generate `.proj` files based on your script.

# Example
Script file for this project
```fs
#r "nuget: KiDev.Baikal"
open KiDev.Baikal

solution(__SOURCE_DIRECTORY__)
    |> addProject( fs()
        |> framework "netstandard2.0"
        |> prop "Description" "Use short F# scripts to define .NET projects and solutions!"
        |> prop "PackageId" "KiDev.Baikal"
        |> prop "PackageLicenseExpression" "MIT"
        |> prop "Version" "0.2.0"
        |> prop "Authors" "AldashkinKirill"
        |> none [
            Add "KiDev.Baikal.fsx";
            Add "Examples/**";
            Add ".gitignore";
            Add "README.md";
            Add "LICENSE";
        ]
        |> compile [
            Add "Types.fs";
            Add "SDKResolver.fs";
            Add "Projects.fs";
            Add "Solutions.fs";
            Add "XMLWriter.fs";
            Add "Runner.fs"
        ])
    |> run
```
Also look to `Examples\` folder.

# Status
Currently this is a early prototype.