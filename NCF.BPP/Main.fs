namespace NCF.BPP

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open System.IO

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/about">] About

module FrontEnd =
    open WebSharper.UI.Next.Html
    open State
    open WebSharper.UI.Next.Html.Tags

    let private f1 _ = ()

    let private renderReport groupId (r:Report) : Doc =
        divAttr 
            [
                attr.``data-`` "groupId" groupId
                attr.``data-`` "reportId" r.id
                attr.``data-`` "embedUrl" r.embedUrl
            ]
            [
                aAttr [attr.href "#"; on.click <@ Client.reportClicked @>]  [text r.name]
            ] 
            :> Doc

    let private renderReports groupId (reports:Map<string, Report>) = 
        reports 
        |> Map.toSeq
        |> Seq.map (fun (_, r) -> renderReport groupId r)
        |> Doc.Concat

    let private renderWorkspace i (w:Workspace) : Doc = 
        divAttr 
            [attr.``class`` "panel panel-default"]
            [divAttr 
                [attr.``class`` "panel-heading"]
                [h4Attr
                    [attr.``class`` "panel-title"]
                    [aAttr
                        [attr.``data-`` "toggle" "collapse"; attr.``data-`` "parent" "#accordion"; attr.href (sprintf "#collapse%d" (i + 1))]
                        [text w.name]
                    ]
                ]
            ;divAttr
                [attr.``class`` "panel-collapse collapse"; attr.id (sprintf "collapse%d" (i + 1))]
                [divAttr
                    [attr.``class`` "panel-body"]
                    [w.reports |> renderReports w.id]
                ]
            ]
            :> Doc

    let private renderWorkspaces (workspaces:Workspaces) =
        workspaces 
        |> Map.toSeq
        |> Seq.mapi (fun i (_, w) -> renderWorkspace i w)
        |> Doc.Concat

    let private powerBIAccordion (workspaces:Workspaces) =
        divAttr 
            [attr.``class`` "container"]
            [
            h5 [text "Select Workspace to see the list of reports in it, then click on the report to open it"]
            divAttr 
                [attr.``class`` "panel-group"; attr.id "accordion"]
                [workspaces |> renderWorkspaces]
            ]

    let private home workspaces rptPageHtml : Doc list =
        [
        divAttr [attr.id "embedReportHtml"; attr.hidden "true"] [text rptPageHtml]
        powerBIAccordion workspaces
        div [client <@ Client.Main() @>]
        ]

    let bodyHome workspaces rptPageHtml : Doc list =
        [
        divAttr [attr.``class`` "page-header"]
            [
            h2Attr [attr.``class`` "text-center"] [text "Power BI Reports"]
            ]
        divAttr [attr.``class`` "jumbotron"] (home workspaces rptPageHtml)
        ]

    let bodyAbout () : Doc list =
        About.paragraphs 
        |> List.map (fun (title, content) -> [h2 [text title] :> Doc; pAttr [attr.``class`` "about-text"] content :> Doc])
        |> List.concat

module Templating =
    open WebSharper.UI.Next.Html

    type Template = Templating.Template<"Template.html">

    let Home workspaces html =
        Content.Page(     
            Template()
                .Body(FrontEnd.bodyHome workspaces html)
                .HomeActive("active")
                .Doc())

    let About () =
        Content.Page(     
            Template()
                .Body(FrontEnd.bodyAbout())
                .AboutActive("active")
                .Doc())

 
module Site =
    let private HomePage (ctx: Context<EndPoint>) =
        let path = sprintf "%sRep.html" ctx.RootFolder
        Templating.Home (State.getWorkspaces()) (File.ReadAllText(path))
        

    let private AboutPage _ =
        Templating.About()

    [<Website>]
    let Main =
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home -> HomePage ctx
            | EndPoint.About -> AboutPage ctx
        )
