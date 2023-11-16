#r "../../bin/Debug/netstandard2.0/KiDev.Baikal.dll"
open System.IO
open System.IO.Compression

let (/) a b = Path.Combine(a, b)

let platforms = [
    ("win-x64", "Tasks.exe"); 
    ("linux-x64", "Tasks"); 
    ("osx-x64", "Tasks")
]

Solution(__SOURCE_DIRECTORY__)
    |> AddProject(FS()
        |> OutputType Exe
        |> Prop "PublishSingleFile" "true"
        |> Prop "PublishTrimmed" "true"
        |> Compile [ Include "Program.fs" ])
    |> AddTask "publish" (fun sol -> 
        printfn "Building..."
        for (rid, _) in platforms do 
            sol.dotnet $"publish $project$ -c Release -r {rid} --self-contained"
        use str = File.Create("executables.zip")
        printfn "Packaging..."
        let arch = new ZipArchive(str, ZipArchiveMode.Create)
        for (rid, execName) in platforms do
            let path = sol.Directory / "bin" / "Release" / "net7.0" / rid / "publish" / execName
            use toArch = arch.CreateEntry($"{rid}-{execName}", CompressionLevel.SmallestSize).Open()
            use fromFile = File.OpenRead(path)
            fromFile.CopyTo(toArch)
        printfn "Done")
    |> run