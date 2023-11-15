namespace KiDev.Baikal

[<AutoOpen>]
module ProjectsDepedencies =
    /// Adds depedencies to a project.
    let Depedencies list (project: Project) = { project with Depedencies = List.append project.Depedencies list }

    /// Creates a NuGet depedency.
    let NuGet id version = NuGet { 
        Id = id; Version = version; 
        IncludeAssets = "all"; ExcludeAssets = "none"; 
        PrivateAssets = "contentfiles;analyzers;build" 
    }

    /// Creates a project depedency.
    let Project name = Project { Name = name; ResolvedPath = ""; }

    /// Sets 'PrivateAssets' for a NuGet depedency
    let PrivateAssets (list: string seq) (dep: Depedency) = 
        match dep with
            | NuGet n -> Depedency.NuGet { n with PrivateAssets = System.String.Join(";", list) }
            | Project p -> failwithf "Can't add assets to project depedency"

    /// Sets 'IncludeAssets' for a NuGet depedency
    let IncludeAssets (list: string seq) (dep: Depedency) = 
        match dep with
            | NuGet n -> Depedency.NuGet { n with IncludeAssets = System.String.Join(";", list) }
            | Project p -> failwithf "Can't add assets to project depedency"

    /// Sets 'ExcludeAssets' for a NuGet depedency
    let ExcludeAssets (list: string seq) (dep: Depedency) = 
        match dep with
            | NuGet n -> Depedency.NuGet { n with ExcludeAssets = System.String.Join(";", list) }
            | Project p -> failwithf "Can't add assets to project depedency"

    let compile = "compile"
    let runtime = "runtime"
    let contentFiles = "contentFiles"
    let build = "build"
    let buildMultitargeting = "buildMultitargeting"
    let buildTransitive = "buildTransitive"
    let analyzers = "analyzers"
    let native = "native"
    let none = "none"
    let all = "all"