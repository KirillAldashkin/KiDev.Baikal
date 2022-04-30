#r "../../bin/Debug/netstandard2.0/KiDev.Baikal.dll"
open KiDev.Baikal

solution(__SOURCE_DIRECTORY__)
    |> addProject(fs()
        |> prop "OutputType" "exe"
        |> compile [ Add "Program.fs" ])
    |> run