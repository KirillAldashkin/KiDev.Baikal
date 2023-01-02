#r "../../bin/Debug/netstandard2.0/KiDev.Baikal.dll"
open KiDev.Baikal

// EXAMPLE NOT READY
Solution(__SOURCE_DIRECTORY__)
    |> AddProject(CS()
        |> Name "Core"
        |> OutputType Library
        |> TargetFramework "netstandard2.0"
        |> LangVersion 10)
    |> AddProject(CS()
        |> Name "Windows"
        |> OutputType WinExe
        |> None [
            Update "*.dll" |> CopyToOutput Always;
        ]
        |> Depedencies [
            Project "Core"
        ])
    |> run