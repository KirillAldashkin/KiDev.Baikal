#r "../../bin/Debug/netstandard2.0/KiDev.Baikal.dll"
open KiDev.Baikal

Solution(__SOURCE_DIRECTORY__)
    |> AddProject(CS()
        |> Name "Core"
        |> Folder "Core"
        |> OutputType Library
        |> TargetFramework "netstandard2.0"
        |> LangVersion 10)
    |> AddProject(CS()
        |> Name "Windows"
        |> Folder "Windows"
        |> OutputType WinExe
        |> Depedencies [
            Project "Core"
        ])
    |> run