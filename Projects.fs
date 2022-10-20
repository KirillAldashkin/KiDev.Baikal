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

    let private addSource name list (project: Project) = 
        let newSrc = if project.Sources.ContainsKey name then
                         List.append project.Sources[name] list
                     else
                         list
        { project with Sources = project.Sources.Add(name, newSrc) }


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

    /// Adds "Compile" sources to a project
    let Compile list (project: Project) = addSource "Compile" list project

    /// Adds "EmbeddedResource" sources to a project
    let EmbeddedResource list (project: Project) = addSource "EmbeddedResource" list project

    /// Adds "None" sources to a project
    let None list (project: Project) = addSource "None" list project

    /// Adds depedencies to a project.
    let Depedencies list (project: Project) = { project with Depedencies = List.append project.Depedencies list }

    /// Creates an "Include" item.
    let Include cont:Source = { Content = cont; Type = "Include"; CopyToOutputDirectory = "Never"; }

    /// Creates an "Exclude" item
    let Exclude cont:Source = { Content = cont; Type = "Exclude"; CopyToOutputDirectory = "Never"; }

    /// Creates an "Remove" item
    let Remove cont:Source = { Content = cont; Type = "Remove"; CopyToOutputDirectory = "Never"; }

    /// Creates an "Update" item
    let Update cont:Source = { Content = cont; Type = "Update"; CopyToOutputDirectory = "Never"; }

    let CopyToOutput copy (item:Source) = { item with CopyToOutputDirectory = copy }
    let Never = "Never"
    let Always = "Always"
    let OnlyNew = "PreserveNewest"

    /// Sets property of a project.
    let Prop name value (project: Project) = { project with Properties = project.Properties.Add(name, value) }

    /// Creates a NuGet depedency.
    let NuGet id version = NuGet { Id = id; Version = version; }

    /// Creates a project depedency.
    let Project name = Project { Name = name; ResolvedPath = ""; }

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