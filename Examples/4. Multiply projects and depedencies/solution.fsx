#r "nuget: KiDev.Baikal"
open KiDev.Baikal

solution(__SOURCE_DIRECTORY__)
    |> addProject(cs()
        |> name "Core"
        |> prop "OutputType" "library"
        |> depedencies [
            nuget "Silk.NET.Windowing.Common" "2.15.0";
            nuget "Silk.NET.OpenGLES" "2.15.0";
        ])
    |> addProject(cs()
        |> name "Desktop"
        |> prop "OutputType" "exe"
        |> depedencies [
            project "Core";
            nuget "Silk.NET.Windowing.Sdl" "2.15.0";
        ])
    |> addProject(cs()
        |> name "Android"
        |> prop "OutputType" "exe"
        |> platform "android"
        |> depedencies [
            project "Core";
            nuget "Silk.NET.Windowing.Android" "2.15.0";
        ])
    |> run