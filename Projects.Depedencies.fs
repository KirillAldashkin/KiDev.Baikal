namespace KiDev.Baikal

[<AutoOpen>]
module ProjectsDepedencies =
    /// Adds depedencies to a project.
    let Depedencies list (project: Project) = { project with Depedencies = List.append project.Depedencies list }

    /// Creates a NuGet depedency.
    let NuGet id version = NuGet { Id = id; Version = version; }

    /// Creates a project depedency.
    let Project name = Project { Name = name; ResolvedPath = ""; }