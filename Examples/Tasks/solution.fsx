#r "../../bin/Debug/netstandard2.0/KiDev.Baikal.dll"
open KiDev.Baikal

Solution(__SOURCE_DIRECTORY__)
    |> AddProject(FS()
        |> OutputType Exe
        |> Prop "PublishSingleFile" "true"
        |> Prop "PublishTrimmed" "true"
        |> Compile [ Include "Program.fs" ])
    |> AddTask "publish" (fun sol -> 
        for rid in ["win-x86"; "win-x64"; "linux-x64"; "linux-arm"; "linux-arm64"; "osx-x64"; "osx.11.0-arm64" ] do 
            sol.dotnet $"publish $project$ -c Release -r {rid} --self-contained")
    |> run