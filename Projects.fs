namespace KiDev.Baikal

[<AutoOpen>]
module Projects =
    let private emptyProject lang: Project = {
        TargetFramework = "current";
        TargetFrameworkPlatform = "";
        Sdk = "Microsoft.NET.Sdk";
        Properties = Map [];
        Language = lang;
        Name = "";
        Folder = "";
        Sources = Map [];
        Depedencies = [];
    }

    /// Creates a new C# project.
    let CS() = { emptyProject "cs" with Properties = Map [ ("ImplicitUsings", "true"); ("Nullable", "enable") ] }

    /// Creates a new F# project.
    let FS() = { emptyProject "fs" with Properties = Map [("GenerateDocumentationFile", "true")] }
    
    /// Sets name of a project.
    let Name name (project: Project) = { project with Name = name }

    /// Sets folder of a project.
    let Folder folder (project: Project) = { project with Folder = folder }

    /// Sets "TargetFramework" version of a project.
    let TargetFramework version (project: Project) = { project with TargetFramework = version }

    /// Sets "TargetPlatform" property of a project.
    let TargetPlatform kind (project: Project) = { project with TargetFrameworkPlatform = if kind = "" then "" else $"-{kind}" }

    /// Sets property of a project.
    let Prop name value (project: Project) = { project with Properties = project.Properties.Add(name, value) }

    /// Sets "LangVersion" of a project.
    let LangVersion vers (project: Project) = project |> Prop "LangVersion" (string vers)
    let Preview = "preview"
    let Latest = "latest"
    let LatestMajor = "latestMajor"

    /// Sets "AllowUnsafeBlocks" of a project.
    let AllowUnsafe (allow: bool) (project: Project) = project |> Prop "AllowUnsafeBlocks" (if allow then "true" else "false")

    /// Sets project's OutputType.
    let OutputType otype (project: Project) = Prop "OutputType" otype project
    let Library = "library"
    let Exe = "exe"
    let WinExe = "winExe"

    /// Confogures project as WinForms (OutputType=WinExe; TargetPlatform=Windows; UseWindowsForms=true)
    let WinForms (project: Project) = 
        project 
        |> OutputType WinExe
        |> TargetPlatform "windows"
        |> Prop "UseWindowsForms" "true"