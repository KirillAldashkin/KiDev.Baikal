[![Nuget](https://img.shields.io/nuget/v/kidev.baikal?style=plastic)](https://www.nuget.org/packages/KiDev.Baikal)
# KiDev.Baikal
**Use short F# scripts to define .NET projects and solutions!**
> This **is not a build system**, but just a nicer alternative to the `.__proj` and `.sln` files.

# How to use
1) Create and open a F# script file.
2) Import this library and `open` module:
```fs
#r "nuget: KiDev.Baikal, 0.3.1"
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
4) Run `dotnet fsi your_script.fsx`. This will generate `.*proj` (and optionally, `.sln`) files based on your script.

# Example
[Script file](https://github.com/KirillAldashkin/KiDev.Baikal/blob/main/KiDev.Baikal.fsx) for this project.
Also look to `Examples\` folder.

# Status
Currently this is a prototype.

Things to do before releasing `1.0.0`:
1) Tasks depedency/error handling
2) Bunch of helper functions to write tasks
3) Solution script and generated files timestamping
4) Support more features of `.**proj` files