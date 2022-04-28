#r "nuget: KiDev.Baikal"
open KiDev.Baikal

solution(__SOURCE_DIRECTORY__)
    |> addProject(fs()
        |> compile [ Add "Program.fs" ])
    |> run