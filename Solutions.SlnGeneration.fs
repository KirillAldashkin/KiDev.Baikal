namespace KiDev.Baikal

open System
open System.IO
open System.Collections.Generic

[<AutoOpen>]
module SolutionsSlnGenerator =
    /// Specifies to generate `.sln` file for current solution.
    let SlnFile file (solution: Solution) =
        { solution with SlnFile = ValueSome(file) }

    /// New unnamed `.sln` file
    let Unnamed = { 
        Name = ValueNone;
        RootItems = list.Empty
    }

    /// New named `.sln` file
    let Named name = { Unnamed with Name = ValueSome(name) }

    /// Adds top-level folder to solution.
    let AddFolder name items (file: SolutionFile) =
        let root = {
            Name = name;
            Child = items
        }
        { file with RootItems = root :: file.RootItems }

    /// Creates new solution folder.
    let SlnFolder name items = SlnItemFolder { Name = name; Child = items; }

    /// Creates new solution item.
    let SlnItem path = SlnItemFile { RelativePath = path; }

    let private getProjectTypeGuid (project: Project) =
        match project.Language with
            | "fs" -> Ok(Guid.Parse("6EC3EE1D-3C4E-46DD-8F32-0CC8E7565705"))
            | "cs" -> Ok(Guid.Parse("9A19103F-16F7-4668-BE54-9A1E7A4F7556"))
            | _ -> Error $"Could not write \"{project.Name}\": type\"{project.Language}\" is not supported"
    
    let private guidStr (guid: Guid) = $"{{{guid.ToString().ToUpper()}}}"
    
    let private itemsProjGuid = Guid.Parse("2150E333-8FDC-42A3-9474-1A3956D46DE8")

    let private writeProject typeGuid name path projGuid (writer: StreamWriter) =
        writer.WriteLine($"Project(\"{guidStr typeGuid}\") = \"{name}\", \"{path}\", \"{guidStr projGuid}\"")
        writer.WriteLine("EndProject")

    let rec private processFolder (folder: SolutionItemFolder) 
                                  (parent: ValueOption<SolutionItemFolder>) 
                                  (nesting: List<Guid * Guid>)
                                  (guids: Dictionary<SolutionItemFolder, Guid>)
                                  (writer: StreamWriter) = 
        let guid = Guid.NewGuid()
        guids.Add(folder, guid)
        match parent with
            | ValueSome par -> nesting.Add((guid, guids[par]))
            | ValueNone -> ()
        for item in folder.Child do
            match item with
                | SlnItemFile file -> ()
                | SlnItemFolder fld -> processFolder fld (ValueSome folder) nesting guids writer
        writer.WriteLine($"Project(\"{guidStr itemsProjGuid}\") = \"{folder.Name}\", \"{folder.Name}\", \"{guidStr guid}\"")
        writer.WriteLine("\tProjectSection(SolutionItems) = preProject")
        for item in folder.Child do
            match item with
                | SlnItemFolder fld -> ()
                | SlnItemFile file -> writer.WriteLine($"\t\t{file.RelativePath} = {file.RelativePath}")
        writer.WriteLine("\tEndProjectSection")
        writer.WriteLine("EndProject")
        
    let internal writeSlnFile (file: SolutionFile) (solution: Solution) =
        let scriptFileName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[1])
        let file = { file with Name = match file.Name with
                                        | ValueSome name -> ValueSome name
                                        | ValueNone -> ValueSome scriptFileName }
        printfn "Writing solution file \"%s.sln\"..." file.Name.Value

        let slnFilePath = Path.Combine(solution.Directory, file.Name.Value)
        let writer = new StreamWriter(slnFilePath + ".sln")
        writer.WriteLine "Microsoft Visual Studio Solution File, Format Version 12.00"

        for proj in solution.Projects do
            match getProjectTypeGuid(proj) with
                | Error e -> printfn "%s" e
                | Ok typeGuid ->
                    let projGuid = Guid.NewGuid()
                    let projPath = Path.Combine(proj.Folder, $"{proj.Name}.{proj.Language}proj")
                    writer |> writeProject typeGuid proj.Name projPath projGuid
        
        let nesting = List<Guid * Guid>()
        let guids = Dictionary<SolutionItemFolder, Guid>()
        for folder in file.RootItems do
            processFolder folder ValueNone nesting guids writer

        if nesting.Count > 0 then 
            writer.WriteLine("Global")
            writer.WriteLine("GlobalSection(NestedProjects) = preSolution")
            for (child, parent) in nesting do
                writer.WriteLine($"\t\t{guidStr child} = {guidStr parent}")
            writer.WriteLine("EndGlobalSection")
            writer.WriteLine("EndGlobal")
        writer.Close()
