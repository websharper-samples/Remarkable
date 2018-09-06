namespace Samples

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.Remarkable
open WebSharper.Google.CodePrettify

[<JavaScript>]
module HelloWorld =

    [<SPAEntryPoint>]
    let Main =
        let Config =
            Options(
                Linkify = true,
                Typographer = true,
                LangPrefix = "prettyprint linenums lang-"
            )
        
        let Md = new Remarkable(Config)
        let rvInput = Var.Create "```fsharp\nlet x = 0\nlet f a = a*2\nf x\n```"
        
        div [] [
            p [] [text "Type markdown text here:"]
            Doc.InputArea [attr.``class`` "input"; attr.rows "20"; attr.value "# Hello world!"] rvInput
            hr [] []
            h4 [] [text "Result"]
            div [
                attr.id "output"
                attr.``class`` "output"
                on.viewUpdate rvInput.View (fun e v -> 
                    JQuery.Of("#output").Empty().Append(Md.Render v).Ignore
                    PR.PrettyPrint()
                )
            ][]
        ]
        |> Doc.RunById "main"
        
