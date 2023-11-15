namespace KiDev.Baikal

open System.IO

[<AutoOpen>]
module Runner = 
    let private resolve (solution: Solution) (from: Project) (depedency: ProjectDepedency): Depedency = 
        let mutable downPath = ""
        let sep = Path.DirectorySeparatorChar
        for folder in from.Folder.Split sep do
            downPath <- downPath + (if folder = "." then "." else "..") + sep.ToString()
        let project = solution.Projects |> List.find (fun p -> p.Name = depedency.Name)
        Depedency.Project { depedency with ResolvedPath = Path.Combine(downPath, project.Folder, $"{project.Name}.{project.Language}proj") }

    let private validateProject (solution: Solution) (project: Project): Result<Project, string> =
        let mutable project = project
        let mutable errors = ""

        if project.Name = "" && solution.Projects.Length > 1 then 
            errors <- errors + "\r\nThere is more than one project and name is not set"
        elif project.Name = "" then
            let dir = DirectoryInfo(solution.Directory)
            project <- { project with Name = dir.Name; Folder = "." }

        if project.Folder = "" then project <- { project with Folder = project.Name }
        if project.TargetFramework = "current" then project <- { project with TargetFramework = SDKResolver.lastCurrent }
        if project.TargetFramework = "preview" then project <- { project with TargetFramework = SDKResolver.lastPreview }

        let targetFramework = $"{project.TargetFramework}{project.TargetFrameworkPlatform}"
        project <- { project with Properties = project.Properties.Add("TargetFramework", targetFramework) }
        if errors = "" then Ok project else Error errors

    let ( *? ) a b = (if a = null then b else a)

    let private writeProject (solution: Solution) (project: Project) =
        let folder = Path.Combine(solution.Directory, project.Folder)
        Directory.CreateDirectory(folder) |> ignore
        let path = Path.Combine(folder, $"{project.Name}.{project.Language}proj")
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
                            xml.tagGroup(pair.Key, $"{source.Type}=\"{source.Content}\"", fun () -> 
                                xml.tag("CopyToOutputDirectory", source.CopyToOutputDirectory *? "Never")
                            )
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
        project

    let private updateProject (solution: Solution) (project: Project) = 
        match validateProject solution project with       
            | Ok proj -> writeProject solution proj
            | Error error -> printf "%s" error; project

    let private updateDepedencies (solution: Solution) (project: Project) = 
        let checkedDeps = [ for depedency in project.Depedencies do 
                                match depedency with
                                    | NuGet nuget -> NuGet nuget.Id nuget.Version
                                    | Project proj -> resolve solution project proj ]
        { project with Depedencies = checkedDeps }

    #if DEBUG
    let private connectDebugger() = 
        if not System.Diagnostics.Debugger.IsAttached then
            printfn "Debug mode: [W]ait for debugger, [L]aunch debugger"
            printf "[W/L/any other = don't debug]? "
            let ch = System.Char.ToLower(System.Console.ReadKey().KeyChar)
            printfn ""
            if ch = 'l' then 
                System.Diagnostics.Debugger.Launch() |> ignore
            else if ch = 'w' then
                while not System.Diagnostics.Debugger.IsAttached do
                    System.Threading.Thread.Sleep(500)
            printfn "Press any key to continue..."
            System.Console.ReadKey(true) |> ignore
            printfn ""
    #endif

    /// Runs solution.
    let run (solution: Solution) =
        let mutable solution = solution
        printfn "\"Baikal\" tool by Aldashkin Kirill"

        #if DEBUG
        connectDebugger()
        #endif

        printfn "Resolving depedencies..."
        let mutable checkedProjects: Project list = [ for project in solution.Projects do updateDepedencies solution project ]
        
        printfn "Validating and writing projects..."
        checkedProjects <- [ for project in checkedProjects do updateProject solution project ]
        solution <- { solution with Projects = checkedProjects }
        
        match solution.SlnFile with
            | ValueSome file -> writeSlnFile file solution
            | ValueNone -> ()

        printfn "Finding target..."
        let mutable newParams = solution.Parameters
        if solution.Projects.Length = 1 then
            newParams <- { newParams with Target = solution.Projects[0].Name }
        else
            let execProjects = List.where (fun proj -> 
                                            if proj.Properties.ContainsKey "OutputType" then
                                                let out = proj.Properties["OutputType"]
                                                out = "winexe" || out = "exe"
                                            else
                                                false) solution.Projects
            if execProjects.Length = 1 then
                newParams <- { newParams with Target = execProjects[0].Name }
        solution <- { solution with Parameters = newParams }

        let task = solution.Parameters.Task
        if task = "" then 
            printfn "No task specified, so nothing will be done"
        else
            if solution.Tasks.ContainsKey task then
                printfn "Running task \"%s\"..." task
                solution.Tasks[task] solution
            else
                printfn "Task \"%s\" not found" task