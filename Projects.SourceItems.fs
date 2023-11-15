namespace KiDev.Baikal

[<AutoOpen>]
module ProjectsSources =
    /// Adds custom sources to a project
    let private AddSource name list (project: Project) = 
        let newSrc = if project.Sources.ContainsKey name then
                         List.append project.Sources[name] list
                     else
                         list
        { project with Sources = project.Sources.Add(name, newSrc) }

    /// Adds "Compile" sources to a project
    let Compile list (project: Project) = AddSource "Compile" list project

    /// Adds "EmbeddedResource" sources to a project
    let EmbeddedResource list (project: Project) = AddSource "EmbeddedResource" list project

    /// Adds "None" sources to a project
    let None list (project: Project) = AddSource "None" list project

    /// Creates an "Include" item.
    let Include cont:Source = { Content = cont; Type = "Include"; CopyToOutputDirectory = ""; }

    /// Creates an "Exclude" item
    let Exclude cont:Source = { Content = cont; Type = "Exclude"; CopyToOutputDirectory = ""; }

    /// Creates an "Remove" item
    let Remove cont:Source = { Content = cont; Type = "Remove"; CopyToOutputDirectory = ""; }

    /// Creates an "Update" item
    let Update cont:Source = { Content = cont; Type = "Update"; CopyToOutputDirectory = ""; }

    /// Sets "CopyToOutput" for an item
    let CopyToOutput copy (item:Source) = { item with CopyToOutputDirectory = copy }
    let Never = "Never"
    let Always = "Always"
    let OnlyNew = "PreserveNewest"