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

type Source = 
    | Add of string
    | Remove of string

type Project = {
    IsWritten: bool
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

type Solution = {
    Directory: string
    Projects: Project list
    Arguments: Map<string, string>
    Keys: string list
}

type DotNetVersion = {
    Version: System.Version
    Prefix: string
}