namespace KiDev.Baikal

[<AutoOpen>]
module Projects =
    let private empty lang: Project = {
        IsWritten = false;
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
    let cs() = { empty "cs" with Properties = Map [ ("ImplicitUsings", "true"); ("Nullable", "enable") ] }

    /// Creates a new F# project.
    let fs() = { empty "fs" with Properties = Map [("GenerateDocumentationFile", "true")] }
    
    /// Sets name of a project.
    let name name (project: Project) = { project with Name = name }

    /// Sets folder of a project.
    let folder folder (project: Project) = { project with Folder = folder }

    /// Sets "TargetFramework" version of a project.
    let framework version (project: Project) = { project with TargetFramework = version }

    /// Sets "TargetFramework" platform of a project.
    let platform kind (project: Project) = { project with TargetFrameworkPlatform = if kind = "" then "" else $"-{kind}" }

    /// Adds "Compile" sources to a project
    let compile list (project: Project) = addSource "Compile" list project

    /// Adds "EmbeddedResource" sources to a project
    let embeddedResource list (project: Project) = addSource "EmbeddedResource" list project

    /// Adds "None" sources to a project
    let none list (project: Project) = addSource "None" list project

    /// Adds depedencies to a project.
    let depedencies list (project: Project) = { project with Depedencies = List.append project.Depedencies list }

    /// Sets property of a project.
    let prop name value (project: Project) = { project with Properties = project.Properties.Add(name, value) }

    /// Creates a NuGet depedency.
    let nuget id version = NuGet { Id = id; Version = version; }

    /// Creates a project depedency.
    let project name = Project { Name = name; ResolvedPath = ""; }
