#r "nuget: KiDev.Baikal, 0.3.1"
open KiDev.Baikal

Solution(__SOURCE_DIRECTORY__)
    |> AddProject(FS()
        |> TargetFramework "netstandard2.0"
        |> NuGetPackaging "KiDev.Baikal" "0.3.1" (PackInfo
            |> Description "Use short F# scripts to define .NET projects and solutions!"
            |> License "MIT"
            |> Authors [ "AldashkinKirill" ])
        |> None [
            Include "KiDev.Baikal.fsx";
            Include "Examples/**";
            Include ".gitignore";
            Include "README.md";
            Include "LICENSE";
        ]
        |> Compile [
            Include "Types.fs";
            Include "SDKResolver.fs";
            Include "Projects.fs";
            Include "Projects.Depedencies.fs";
            Include "Projects.SourceItems.fs";
            Include "Projects.NuGet.fs";
            Include "XMLWriter.fs";
            Include "DefaultTasks.fs";
            Include "Solutions.fs";
            Include "Solutions.SlnGeneration.fs";
            Include "Runner.fs"
        ])
    |> run
