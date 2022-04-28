namespace KiDev.Baikal

open System.IO

/// Tool to simplify writing XML files.
type XMLWriter(writer: TextWriter) = class
    member val private indent = 0 with get, set

    member private this.tabs() = new string('\t', this.indent)

    /// Writes a tag with name and content.
    member this.tagGroup(name, action) =
        writer.WriteLine $"{this.tabs()}<{name}>"
        this.indent <- this.indent + 1
        action()
        this.indent <- this.indent - 1
        writer.WriteLine $"{this.tabs()}</{name}>"

    /// Writes a tag with name, parameters and content.
    member this.tagGroup(name, query, action) =
        writer.WriteLine $"{this.tabs()}<{name} {query}>"
        this.indent <- this.indent + 1
        action()
        this.indent <- this.indent - 1
        writer.WriteLine $"{this.tabs()}</{name}>"

    /// Writes a single-line tag with name and content.
    member this.tag(name, value) =
        writer.WriteLine $"{this.tabs()}<{name}>{value}</{name}>"

    /// Writes a single-line tag without content.
    member this.tag(value) =
        writer.WriteLine $"{this.tabs()}<{value}/>"

    /// Writes a single-line tag with name, parameters and content.
    member this.tag(name, query, value) =
        writer.WriteLine $"{this.tabs()}<{name} {query}>{value}</{name}>"
end