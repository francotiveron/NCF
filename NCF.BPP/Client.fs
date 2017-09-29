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
    //open NCF.BPP.PowerBI.JSExtension
    open Microsoft.PowerBI.Api.V2.Models
    open WebSharper.UI.Next.Client.HtmlExtensions

    type Permissions = Read = 0 | ReadWrite = 1 | Copy = 2 | Create = 4 | All = 7
    type TokenType = Aad = 0 | Embed = 1

    let private sleep ms =
        JS.SetTimeout (fun () -> ()) ms |> ignore
(*
    let private openReport1 name embedUrl reportId embedToken =
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

        //view.Document.AddEventListener("DOMContentLoaded", (fun () -> view.Init("NCF.BPP - " + name, powerbi_conf)), false)
        while
            try view.Init("NCF.BPP - " + name, powerbi_conf); false
            with (err) -> true
            do sleep 1
        //JS.SetTimeout (fun () -> view.Init("NCF.BPP - " + name, powerbi_conf)) 3000 |> ignore
        //JQuery(view.Document).Ready((fun () -> view.Init("NCF.BPP - " + name, powerbi_conf))).Ignore
        //view.Document.AddEventListener("load", (fun () -> view.Init("NCF.BPP - " + name, powerbi_conf)), false)
*)
    let private openReport name embedUrl reportId embedToken =
        let html = 
            JQuery("#embedReportHtml").Text()
             .Replace("${{rplcTitle}}", "NCF.BPP - " + name)
             .Replace("${{rplcEmbedToken}}", embedToken)
             .Replace("${{rplcEmbedUrl}}", embedUrl)
             .Replace("${{rplcReportId}}", reportId)

        let view = JS.Window.Open()
        view.Document.Write(html)

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