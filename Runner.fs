namespace KiDev.Baikal

open System.IO

[<AutoOpen>]
module Runner = 
    let resolve (solution: Solution) (from: Project) (depedency: ProjectDepedency): Depedency = 
        let mutable downPath = ""
        let sep = Path.DirectorySeparatorChar
        for folder in from.Folder.Split sep do
            downPath <- downPath + (if folder = "." then "." else "..") + sep.ToString()
        let project = List.find(fun p -> p.Name = depedency.Name) solution.Projects
        Project { depedency with ResolvedPath = Path.Combine(downPath, project.Folder, $"{project.Name}.{project.Language}proj") }

    let private validateProject (solution: Solution) (project: Project): Result<Project, string> =
        let mutable project = project
        let mutable errors = ""

        if project.Name = "" && solution.Projects.Length > 1 then 
            errors <- errors + "\r\nThere is more than one project and name is not set"
        elif project.Name = "" then
            project <- { project with Name = Path.GetDirectoryName solution.Directory; Folder = "." }

        if project.Folder = "" then project <- { project with Folder = project.Name }
        if project.TargetFramework = "current" then project <- { project with TargetFramework = SDKResolver.lastCurrent }
        if project.TargetFramework = "preview" then project <- { project with TargetFramework = SDKResolver.lastPreview }

        let targetFramework = $"{project.TargetFramework}{project.TargetFrameworkPlatform}"
        project <- { project with Properties = project.Properties.Add("TargetFramework", targetFramework) }
        if errors = "" then Ok project else Error errors

    let private writeProject (solution: Solution) (project: Project) =
        let path = Path.Combine(solution.Directory, project.Folder, $"{project.Name}.{project.Language}proj")
        let writer = new StreamWriter(path)
        let xml = XMLWriter(writer)
        xml.tagGroup("Project", $"Sdk=\"{project.Sdk}\"", fun () -> 
            xml.tagGroup("PropertyGroup", fun () ->
                for pair in project.Properties do
                    xml.tag(pair.Key, pair.Value)
            )
            for pair in project.Sources do
                if not pair.Value.IsEmpty then
                    xml.tagGroup("ItemGroup", fun() -> 
                        for source in pair.Value do
                            match source with
                                | Add add -> xml.tag($"{pair.Key} Include=\"{add}\"")
                                | Remove remove -> xml.tag($"{pair.Key} Remove=\"{remove}\"")
                    )
            if not project.Depedencies.IsEmpty then
                xml.tagGroup("ItemGroup", fun() -> 
                    for depedency in project.Depedencies do
                        match depedency with
                            | NuGet nuget -> xml.tag($"PackageReference Include=\"{nuget.Id}\" Version=\"{nuget.Version}\"")
                            | Project proj -> xml.tag($"ProjectReference Include=\"{proj.ResolvedPath}\"")
                )
        )
        writer.Close()
        { project with IsWritten = true }

    let private updateProject (solution: Solution) (project: Project) = 
        let mutable project = project
        if not project.IsWritten then
            project <- match validateProject solution project with       
                        | Ok proj -> writeProject solution proj
                        | Error error -> printf "%s" error; project
        project

    let checkDepedencies (solution: Solution) (project: Project) = 
        let checkedDeps = [ for depedency in project.Depedencies do 
                                match depedency with
                                    | NuGet nuget -> NuGet nuget
                                    | Project proj -> resolve solution project proj ]
        { project with Depedencies = checkedDeps }


    /// Runs solution.
    let run (solution: Solution) = 
        let mutable checkedProjects: Project list = [ for project in solution.Projects do updateProject solution project ]
        checkedProjects <- [ for project in checkedProjects do checkDepedencies solution project ] 
        // Run solution here
        { solution with Projects = checkedProjects }