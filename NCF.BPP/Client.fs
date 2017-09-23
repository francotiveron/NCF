namespace NCF.BPP

open WebSharper

[<JavaScript>]
module Client =
    open WebSharper.UI.Next
    open WebSharper.UI.Next.Html
    open WebSharper.JavaScript
    open WebSharper.JQuery
    open WebSharper.UI.Next.Client.Attr
    open WebSharper.UI.Next.Client.HtmlExtensions
    open NCF.BPP.PowerBI.JSExtension
    open Microsoft.PowerBI.Api.V2.Models
    open WebSharper.UI.Next.Client.HtmlExtensions

    type Permissions = Read = 0 | ReadWrite = 1 | Copy = 2 | Create = 4 | All = 7
    type TokenType = Aad = 0 | Embed = 1

    let private sleep ms =
        JS.SetTimeout (fun () -> ()) ms |> ignore

    let private openReport name embedUrl reportId embedToken =
        let html = JQuery("#embedReportHtml").Text()
        let view = JS.Window.Open() :?> PowerBIView
        view.Document.Write(html)

        let powerbi_settings = PowerBISettings(FilterPaneEnabled = true, NavContentPaneEnabled = true)
        
        let powerbi_conf = 
            PowerBIConfig(
                Type = "report", 
                TokenType = int TokenType.Embed,
                AccessToken = embedToken, 
                EmbedUrl = embedUrl,
                Id = reportId,
                Permissions = int Permissions.All,
                Settings = powerbi_settings)

        while
            try view.Init("NCF.BPP - " + name, powerbi_conf); false
            with (err) -> true
            do sleep 1
        //JS.SetTimeout (fun () -> view.Init("NCF.BPP - " + name, powerbi_conf)) 3000 |> ignore
        //JQuery(view.Document).Ready((fun () -> view.Init("NCF.BPP - " + name, powerbi_conf))).Ignore
        //view.Document.AddEventListener("load", (fun () -> view.Init("NCF.BPP - " + name, powerbi_conf)), false)

    let private getEmbedToken gId rId =
        async { return! Server.getEmbedTokenAsync gId rId }

    let reportClicked (e:Dom.Element) _ =
       async {
            let div = e.ParentElement
            let gId, rId, embedUrl = div.GetAttribute("data-groupId"), div.GetAttribute("data-reportId"), div.GetAttribute("data-embedUrl")
            JQuery("*").Css("cursor", "progress").Ignore
            let! token = getEmbedToken gId rId
            JQuery("*").Css("cursor", "default").Ignore
            match token with
            | Ok embedToken -> openReport e.TextContent embedUrl rId embedToken
            | Error message -> JS.Alert message
        } |> Async.Start

    let f _ (e:Dom.Element) =
        let gId, rId = e.GetAttribute("data-groupId"), e.GetAttribute("data-reportId")
        e.TextContent <- sprintf "%s - %s" gId rId

    let init _ =
        //JQuery.Of("[data-reportId]").Text("Pippo").Ignore
        JQuery.Of("[data-reportId]").Each(f).Ignore
   
    let Main () =
        //JS.Document.
        divAttr [(*OnAfterRender init*)] []
        //JQuery.Of(init)
        

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