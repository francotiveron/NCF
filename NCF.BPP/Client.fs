namespace NCF.BPP

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open NCF.BPP.PowerBI.JSExtension
open WebSharper.JavaScript
open WebSharper.JQuery
open PowerBI

[<JavaScript>]
module Client =

    let Main () =
        div [text "Test"]
        //Doc.Verbatim "JQuery.Of(\"[data-reportId]\").Text(\"Pippo\").Ignore"

//[<JavaScript>]
//module Client =

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

    //let private renderReport (r:Report) : Doc =
    //    div [
    //        (if r.``public`` then
    //            aAttr [] [text r.name]
    //            else p [text r.name])
    //        ]
    //        :> Doc
    //let private renderReport (r:Report) : Doc =
    //    div [text r.name] :> Doc

    //let private renderReports (reports:Map<string, Report>) = 
    //    reports 
    //    |> Map.toSeq
    //    |> Seq.map (fun (_, r) -> renderReport r)
    //    |> Doc.Concat

    //let private renderWorkspace i (w:Workspace) : Doc = 
    //    divAttr 
    //        [attr.``class`` "panel panel-default"]
    //        [divAttr 
    //            [attr.``class`` "panel-heading"]
    //            [h4Attr
    //                [attr.``class`` "panel-title"]
    //                [aAttr
    //                    [attr.``data-`` "toggle" "collapse"; attr.``data-`` "parent" "#accordion"; attr.href (sprintf "#collapse%d" (i + 1))]
    //                    [text w.name]
    //                ]
    //            ]
    //        ;divAttr
    //            [attr.``class`` "panel-collapse collapse"; attr.id (sprintf "collapse%d" (i + 1))]
    //            [divAttr
    //                [attr.``class`` "panel-body"]
    //                [w.reports |> renderReports]
    //            ]
    //        ]
    //        :> Doc

    //let private renderWorkspaces (workspaces:Workspaces) =
    //    workspaces 
    //    |> Map.toSeq
    //    |> Seq.mapi (fun i (_, w) -> renderWorkspace i w)
    //    |> Doc.Concat

    //let Main (workspaces:Workspaces) =
    //    divAttr 
    //        [attr.``class`` "container"]
    //        [divAttr 
    //            [attr.``class`` "panel-group"; attr.id "accordion"]
    //            [workspaces |> renderWorkspaces]
    //        ]

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