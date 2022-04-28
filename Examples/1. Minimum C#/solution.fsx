#r "nuget: KiDev.Baikal"
open KiDev.Baikal

solution(__SOURCE_DIRECTORY__)
    |> addProject(cs())
    |> run