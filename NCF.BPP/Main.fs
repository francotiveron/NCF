namespace NCF.BPP

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open PowerBI
open System.IO

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/about">] About

module Templating =
    open WebSharper.UI.Next.Html

    type MainTemplate = Templating.Template<"Main.html">

    // Compute a menubar where the menu item for the given endpoint is active
    let MenuBar (ctx: Context<EndPoint>) endpoint : Doc list =
        let ( => ) txt act =
             liAttr [if endpoint = act then yield attr.``class`` "active"] [
                aAttr [attr.href (ctx.Link act)] [text txt]
             ]
        [
            li ["Home" => EndPoint.Home]
            li ["About" => EndPoint.About]
        ]

    let doc ctx action (workspaces:Workspace seq) (title: string) (body: Doc list) = 
        MainTemplate()
                .Body(body)
                //.AccessToken(report.AccessToken)
                //.EmbedUrl(report.EmbedUrl)
                //.ReportId(report.ReportId)
                .Doc()
    
 
    let Main ctx action report (title: string) (body: Doc list) =
        let doc = doc ctx action report title body
        Content.Page(doc)
 
module Site =
    open WebSharper.UI.Next.Html
    open PowerBI

    let HomePage (ctx: Context<EndPoint>) =
        let path = sprintf "%sRep.html" ctx.RootFolder
        let workspaces = getWorkspaces()
        let html = File.ReadAllText(path)
        Templating.Main ctx EndPoint.Home workspaces "Home" [
            divAttr [attr.id "embedReportHtml"; attr.hidden "true"] [text html]
            div [client <@ Client.Main(workspaces) @>]
        ]

    //let AboutPage ctx =
    //    Templating.Main ctx EndPoint.About () "About" [
    //        h1 [text "About"]
    //        p [text "This is a template WebSharper client-server application."]
    //    ]

    [<Website>]
    let Main =
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home -> HomePage ctx
            | EndPoint.About -> HomePage ctx
        )
