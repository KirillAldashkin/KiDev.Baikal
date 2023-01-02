namespace KiDev.Baikal

[<AutoOpen>]
module ProjectsNuGet =
    let private PropIfSome name value project =
        match value with
            | None -> project
            | Some vl -> project |> Prop name vl

    /// Adds information about NuGet package that should be created from this project.
    let NuGetPackaging name version (options: NuGetPackagingInfo) (project: Project) =
        let mapAuthors lst = System.String.Join(",", lst |> Seq.ofList)
        project
            |> Prop "PackageId" name
            |> Prop "Version" version
            |> PropIfSome "Description" options.Description
            |> PropIfSome "PackageLicenseExpression" options.License
            |> PropIfSome "Authors" (Option.map mapAuthors options.Authors)

    /// Empty NuGet packaging info.
    let PackInfo = {
        Authors = Option<string list>.None;
        License = Option<string>.None;
        Description = Option<string>.None;
    }

    /// Sets authors for this NuGet package.
    let Authors authors pack =
        { pack with Authors = Some authors }

    /// Sets license for this NuGet package.
    let License license pack =
        { pack with License = Some license }

    /// Sets description for this NuGet package.
    let Description description pack =
        { pack with Description = Some description }