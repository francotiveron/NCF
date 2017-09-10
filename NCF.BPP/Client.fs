namespace NCF.BPP

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html
open NCF.BPP.PowerBI.JSExtension
open WebSharper.JavaScript
open WebSharper.JQuery
open PowerBI

[<JavaScript>]
module Client =

    //let fun1 (report) =
    //    let powerbi_settings = PowerBISettings(FilterPaneEnabled = true)
        
        //let powerbi_conf = 
        //    PowerBIConfig(
        //        TokenType = 1,
        //        AccessToken = report.AccessToken, 
        //        Type = "report", 
        //        EmbedUrl = report.EmbedUrl,
        //        Id = report.ReportId,
        //        Settings = powerbi_settings)

        //let h = JQuery("#embedReportHtml").Text()
        //let w = JS.Window.Open() :?> Wi
        //w.Document.Write(h)
        //w.Init(powerbi_conf)


    let private renderWorkspace (w:Workspace) : Doc list = 
        divAttr [] [text w.name]

    let Main (workspaces:Workspace seq) =
        workspaces |> Seq.map renderWorkspace |> Doc.Concat

(*
        let powerbi_target = divAttr [attr.style "height:720px"] []
        
        //let html = Doc.Element "" [] [doc] |> (fun elt -> elt.Html)
        let powerbi_settings = PowerBISettings(FilterPaneEnabled = true)
        
        let powerbi_conf = 
            PowerBIConfig(
                TokenType = 1,
                AccessToken = report.AccessToken, 
                Type = "report", 
                EmbedUrl = report.EmbedUrl,
                Id = report.ReportId,
                Settings = powerbi_settings)
          
        let start_embed () : unit = 
            async {
              Powerbi.Embed(powerbi_target.Dom, powerbi_conf)
            } |> Async.Start

        div [
            Doc.Button "Start embed powerbi" [attr.``class`` "btn btn-primary"]  (fun _ -> start_embed())

            powerbi_target
        ]
*)
(*
        open System.Reflection.Emit

        let rvInput = Var.Create ""
        let submit = Submitter.CreateOption rvInput.View
        let vReversed =
            submit.View.MapAsync(function
                | None -> async { return "" }
                | Some input -> Server.DoSomething input
            )
        div [
            Doc.Input [] rvInput
            Doc.Button "Send" [] submit.Trigger
            hr []
            h4Attr [attr.``class`` "text-muted"] [text "The server responded:"]
            divAttr [attr.``class`` "jumbotron"] [h1 [textView vReversed]]
        ]
 *)