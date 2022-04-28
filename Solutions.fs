namespace KiDev.Baikal

[<AutoOpen>]
module Solutions =
    let mutable private cliArgs = Map<string, string>([])
    let mutable private cliKeys: string list = []

    do
        let args: seq<string> = System.Environment.GetCommandLineArgs()
        let enum = args.GetEnumerator()
        while enum.MoveNext() do
            if enum.Current[0] = '-' then 
                let key = enum.Current[1..]
                if enum.MoveNext() then
                    let value = enum.Current
                    cliArgs <- cliArgs.Add(key, value)
                else
                    cliKeys <- key :: cliKeys
            else
                cliKeys <- enum.Current :: cliKeys



    /// Creates a new solution in a specified folder.
    let solution folder = { 
        Directory = folder; 
        Projects = []; 
        Arguments = cliArgs; 
        Keys = cliKeys 
    }

    /// Sets a argument in a solution.
    let arg key value (solution: Solution) = { solution with Arguments = solution.Arguments.Add(key, value) }

    /// Sets a key in a solution.
    let key name (solution: Solution) = { solution with Keys = name :: solution.Keys }

    /// Adds project to a solution.
    let addProject (project: Project) (solution: Solution) = { solution with Projects = project :: solution.Projects }