namespace KiDev.Baikal

open System
open System.Text
open System.Diagnostics

module internal SDKResolver =
    let mutable lastCurrent = ""
    let mutable lastPreview = ""

    do
        let info = ProcessStartInfo("dotnet", "--list-sdks")
        info.RedirectStandardOutput <- true
        info.StandardOutputEncoding <- Encoding.UTF8
        info.UseShellExecute <- false
        info.CreateNoWindow <- true
        let proc = Process.Start info
        proc.WaitForExit()
        let input = proc.StandardOutput
        while not input.EndOfStream do
            let raw = input.ReadLine().Split(' ')[0]
            let firstDot = raw.IndexOf '.'
            let secondDot = raw.IndexOf('.', firstDot+1)
            let rawVersion = raw[..(secondDot-1)]
            // for example: "6.0.300-preview.22204.3 [C:\Program Files\dotnet\sdk]" should become "6.0" after code above
            let version = Version.Parse(rawVersion)
            let prefix = if version < new Version(5, 0) then "netcoreapp" else "net"
            if raw.Contains "preview" || raw.Contains "rc" then
                lastPreview <- prefix + rawVersion
            else
                lastCurrent <- prefix + rawVersion
        if lastCurrent = "" then 
            lastCurrent <- lastPreview
        elif lastPreview = "" then
            lastPreview <- lastCurrent