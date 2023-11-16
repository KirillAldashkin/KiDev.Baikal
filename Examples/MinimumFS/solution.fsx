#r "../../bin/Debug/netstandard2.0/KiDev.Baikal.dll"

Solution(__SOURCE_DIRECTORY__)
    |> AddProject(FS()
        |> OutputType Exe
        |> Compile [ Include "Program.fs" ])
    |> run