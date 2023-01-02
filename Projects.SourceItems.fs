﻿namespace KiDev.Baikal

[<AutoOpen>]
module ProjectsSources =
    let private addSource name list (project: Project) = 
        let newSrc = if project.Sources.ContainsKey name then
                         List.append project.Sources[name] list
                     else
                         list
        { project with Sources = project.Sources.Add(name, newSrc) }

    /// Adds "Compile" sources to a project
    let Compile list (project: Project) = addSource "Compile" list project

    /// Adds "EmbeddedResource" sources to a project
    let EmbeddedResource list (project: Project) = addSource "EmbeddedResource" list project

    /// Adds "None" sources to a project
    let None list (project: Project) = addSource "None" list project

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