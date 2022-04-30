#r "..\\..\\bin\\Debug\\netstandard2.0\\KiDev.Baikal.dll"
open KiDev.Baikal

solution(__SOURCE_DIRECTORY__)
    |> addProject(fs()
        |> prop "OutputType" "exe"
        |> prop "PublishSingleFile" "true"
        |> prop "PublishTrimmed" "true"
        |> compile [ Add "Program.fs" ])
    |> addTask "publish" (fun sol -> 
        for rid in ["win-x86"; "win-x64"; "linux-x64"; "linux-arm"; "linux-arm64"; "osx-x64"; "osx.11.0-arm64" ] do 
            sol.dotnet $"publish $project$ -c Release -r {rid} --self-contained")
    |> run