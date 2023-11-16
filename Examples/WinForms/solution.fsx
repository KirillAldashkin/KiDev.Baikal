#r "../../bin/Debug/netstandard2.0/KiDev.Baikal.dll"

Solution(__SOURCE_DIRECTORY__)
    |> AddProject(CS() |> WinForms)
    |> run