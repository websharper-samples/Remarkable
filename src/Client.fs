namespace Samples

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Templating
open WebSharper.UI.Client
open WebSharper.Remarkable
open WebSharper.HighlightJS

/// A live Markdown editor: Remarkable renders the document and highlight.js
/// colors the fenced code blocks.
[<JavaScript>]
module Client =

    type Template = Templating.Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let private initial =
        """# WebSharper + Remarkable

Type **Markdown** on the left and see it rendered live on the right.

- `Remarkable` parses the Markdown
- `highlight.js` colors the code blocks
- [links](https://websharper.com) work too

```fsharp
let greet name =
    printfn "Hello, %s!" name

greet "WebSharper"
```

> Edit this text and watch the preview update.
"""

    [<SPAEntryPoint>]
    let Main () =
        // Register the languages used by the fenced code blocks.
        Hljs.RegisterLanguage("fsharp", Language.Fsharp)
        Hljs.RegisterLanguage("javascript", Language.Javascript)
        Hljs.RegisterLanguage("typescript", Language.Typescript)
        Hljs.RegisterLanguage("csharp", Language.Csharp)
        Hljs.RegisterLanguage("python", Language.Python)
        Hljs.RegisterLanguage("xml", Language.Xml)
        Hljs.RegisterLanguage("json", Language.Json)
        Hljs.RegisterLanguage("css", Language.Css)
        Hljs.RegisterLanguage("bash", Language.Bash)
        Hljs.RegisterLanguage("sql", Language.Sql)
        Hljs.RegisterLanguage("markdown", Language.Markdown)

        let options = WebSharper.Remarkable.Options()
        options.Html <- true
        options.Typographer <- true
        options.LangPrefix <- "language-"

        let remarkable : WebSharper.Remarkable.Remarkable = Remarkable(options)
        let source = Var.Create initial

        let output =
            div [
                attr.``class`` "markdown-body"
                on.viewUpdate source.View (fun element text ->
                    element.InnerHTML <- remarkable.Render(text)
                    Hljs.HighlightAll())
            ] []

        Template()
            .Source(source)
            .Output(output)
            .Bind()
