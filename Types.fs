namespace KiDev.Baikal

type NuGetDepedency = {
    Id: string
    Version: string
}

type ProjectDepedency = {
    Name: string
    ResolvedPath: string
}

type Depedency =
    | NuGet of NuGetDepedency
    | Project of ProjectDepedency

type Source = {
    Content: string
    Type: string
    CopyToOutputDirectory: string
}

type Project = {
    Sdk: string
    TargetFramework: string
    TargetFrameworkPlatform: string
    Properties: Map<string, string>
    Language: string
    Name: string
    Folder: string
    Sources: Map<string, Source list>
    Depedencies: Depedency list
}

type Parameters = {
    Task: string
    Body: string
    Target: string
    Arguments: Map<string, string>
}

type Solution = {
    Parameters: Parameters
    Directory: string
    Projects: Project list
    Tasks: Map<string, Solution -> Unit>
}

type DotNetVersion = {
    Version: System.Version
    Prefix: string
}