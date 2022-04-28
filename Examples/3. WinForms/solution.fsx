#r "nuget: KiDev.Baikal"
open KiDev.Baikal

solution(__SOURCE_DIRECTORY__)
    |> addProject(cs()
        |> platform "windows"
        |> prop "UseWindowsForms" "true")
    |> run