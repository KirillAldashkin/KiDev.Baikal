#r "../../bin/Debug/netstandard2.0/KiDev.Baikal.dll"
open KiDev.Baikal

solution(__SOURCE_DIRECTORY__)
    |> addProject(cs()
        |> prop "OutputType" "exe")
    |> run