namespace KiDev.Baikal

[<AutoOpen>]
module Solutions =
    let mutable private cliTask = ""
    let mutable private cliBody = ""
    let mutable private cliArgs = Map<string, string> []

    let defaultTasks = Map [ 
        ("dotnet", dotnetTask);
        ("run", runTask);
        ("build", buildTask);
    ]

    do
        // first item is executable file path, and second item is script file name.
        let args: seq<string> = System.Environment.GetCommandLineArgs()[2..]
        let enum = args.GetEnumerator()
        if enum.MoveNext() then 
            cliTask <- enum.Current
            let mutable notFinished = true
            while (notFinished <- enum.MoveNext(); notFinished) && enum.Current[0] <> '!' do
                cliBody <- cliBody + enum.Current + " "
            if notFinished then
                let firstKey = enum.Current[1..]
                if enum.MoveNext() then
                    cliArgs <- cliArgs.Add(firstKey, enum.Current)
                    while enum.MoveNext() do
                        let key = enum.Current[1..]
                        if enum.MoveNext() then 
                            cliArgs <- cliArgs.Add(key, enum.Current)

    /// Creates a new solution in a specified folder.
    let Solution folder = {
        Parameters = {
            Target = "";
            Arguments = cliArgs;
            Task = cliTask;
            Body = cliBody;
        };
        Tasks = defaultTasks;
        Directory = folder; 
        Projects = []; 
    }

    /// Sets a argument in a solution.
    let Arg key value (solution: Solution) = 
        let newArgs = solution.Parameters.Arguments.Add(key, value)
        let newParams = { solution.Parameters with Arguments = newArgs }
        { solution with Parameters = newParams } 

    /// Sets a argument in a solution.
    let Target name (solution: Solution) = 
        let newParams = { solution.Parameters with Target = name }
        { solution with Parameters = newParams } 

    /// Adds project to a solution.
    let AddProject (project: Project) (solution: Solution) = { solution with Projects = project :: solution.Projects }

    /// Adds task to a solution.
    let AddTask name task (solution: Solution) = { solution with Tasks = solution.Tasks.Add(name, task) }