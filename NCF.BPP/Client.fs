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
    open Microsoft.PowerBI.Api.V2.Models
    open WebSharper.UI.Next.Client.HtmlExtensions

    let private openEmbedPage pbiType name embedUrl id embedToken =
        let html = 
            JQuery("#embedReportHtml").Text()
             .Replace("${{rplcType}}", pbiType)
             .Replace("${{rplcTitle}}", "NCF.BPP - " + name)
             .Replace("${{rplcEmbedToken}}", embedToken)
             .Replace("${{rplcEmbedUrl}}", embedUrl)
             .Replace("${{rplcId}}", id)    

        let view = JS.Window.Open()
        view.Document.Write(html)

    let private getEmbedToken gId rId =
        async { return! Server.getEmbedTokenAsync gId rId }

    let pbiLinkClicked (e:Dom.Element) _ =
       async {
            let div = e.ParentElement
            let pbiType, gId, id, embedUrl = div.GetAttribute("data-pbiType"), div.GetAttribute("data-groupId"), div.GetAttribute("data-resourceId"), div.GetAttribute("data-embedUrl")
            JQuery("*").Css("cursor", "progress").Ignore
            let! token = getEmbedToken gId id
            JQuery("*").Css("cursor", "default").Ignore
            match token with
            | Ok embedToken -> openEmbedPage pbiType e.TextContent embedUrl id embedToken
            | Error message -> JS.Alert message
        } |> Async.Start
