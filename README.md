[![Nuget](https://img.shields.io/nuget/v/kidev.baikal?style=plastic)](https://www.nuget.org/packages/KiDev.Baikal)
# KiDev.Baikal
**Use short F# scripts to define .NET projects and solutions!**
> This is just a nicer alternative to `.*proj` and `.sln` files. If you need a fully-featured build system, consider using `Fake` or `Nuke`.

# How to use
1) Create and open a F# script file and import this library:
```fs
#r "nuget: KiDev.Baikal"
```
2) Write definition of your solution:
```fs
// __SOURCE_DIRECTORY__ - F# literal, path to directory that contains this script
Solution(__SOURCE_DIRECTORY__)
    |> AddProject(FS()
        |> TargetFramework "net6.0"
        |> OutputType Exe
        |> Compile [ Include "Program.fs" ])
    |> run
```
4) Run `dotnet fsi your_script.fsx`. This will generate `.*proj` (and optionally, `.sln`) files based on your script.

# Example
[Script file](https://github.com/KirillAldashkin/KiDev.Baikal/blob/main/KiDev.Baikal.fsx) for this project.
Also look to `Examples\` folder.

# Status
Currently this is a prototype.

Things to do before releasing `1.0.0`:
1) Tasks dependency/error handling
2) Bunch of helper functions to write tasks
3) Solution script and generated files timestamping
4) Support more features of `.**proj` files
5) Maybe, .NET tool or even a VS extension?