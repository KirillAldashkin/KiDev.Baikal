#r "../../bin/Debug/netstandard2.0/KiDev.Baikal.dll"
open KiDev.Baikal

Solution(__SOURCE_DIRECTORY__)
    |> AddProject(FS()
        |> OutputType Exe
        |> Compile [ Include "Program.fs" ])
    |> run