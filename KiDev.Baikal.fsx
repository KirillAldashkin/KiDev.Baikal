#r "bin/Debug/netstandard2.0/KiDev.Baikal.dll"
open KiDev.Baikal

Solution(__SOURCE_DIRECTORY__)
    |> AddProject(FS()
        |> TargetFramework "netstandard2.0"
        |> Prop "Description" "Use short F# scripts to define .NET projects and solutions!"
        |> Prop "PackageId" "KiDev.Baikal"
        |> Prop "PackageLicenseExpression" "MIT"
        |> Prop "Version" "0.2.2"
        |> Prop "Authors" "AldashkinKirill"
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
            Include "XMLWriter.fs";
            Include "DefaultTasks.fs";
            Include "Solutions.fs";
            Include "Runner.fs"
        ])
    |> run