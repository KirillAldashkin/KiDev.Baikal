namespace KiDev.Baikal

open System.IO
open System.Diagnostics

[<AutoOpen>]
module DefaultTasks =
    let private _dotnet command (solution: Solution) =
        let mutable command = command
        if command <> "" then
            let target = solution.Projects |> List.find (fun proj -> proj.Name = solution.Parameters.Target)
            let targetPath = Path.Combine(target.Folder, $"{target.Name}.{target.Language}proj")
            command <- command.Replace("$project$", targetPath)
        for pair in solution.Parameters.Arguments do
            command <- command.Replace($"${pair.Key}$", pair.Value)
        printfn ">> Starting \"dotnet %s\"..." command
        let startInfo = ProcessStartInfo("dotnet", command)
        startInfo.WorkingDirectory <- solution.Directory
        Process.Start(startInfo).WaitForExit()
        printfn ">> Finished \"dotnet %s\"." command

    let internal runTask (solution: Solution) = _dotnet "run $project$" solution
    let internal buildTask (solution: Solution) = _dotnet "build $project$" solution
    let internal publishTask (solution: Solution) = _dotnet "publish $project$" solution
    let internal dotnetTask (solution: Solution) = _dotnet solution.Parameters.Body solution

    /// Runs custom .NET CLI command.
    let dotnet command (solution: Solution) =
        _dotnet command solution
        solution

    type Solution with
        /// Runs custom .NET CLI command.
        member this.dotnet command = dotnet command this |> ignore