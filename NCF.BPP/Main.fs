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

    type ReportAccess = 
        | Unknown
        | Forbidden
        | Embeddable of string

    type Report = {
        name : string
        id: string
        groupId: string
        embedUrl : string
        access : ReportAccess
        }

    type Workspace = {
        name : string
        id : string
        reports : Map<string, Report>
        }

    type Workspaces = Map<string, Workspace>

    let private renderReport (r:Report) : Doc =
        div [text r.name] :> Doc

    let private renderReports (reports:Map<string, Report>) = 
        reports 
        |> Map.toSeq
        |> Seq.map (fun (_, r) -> renderReport r)
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
                    [w.reports |> renderReports]
                ]
            ]
            :> Doc

    let private renderWorkspaces (workspaces:Workspaces) =
        workspaces 
        |> Map.toSeq
        |> Seq.mapi (fun i (_, w) -> renderWorkspace i w)
        |> Doc.Concat

    let Main (workspaces:Workspaces) =
        divAttr 
            [attr.``class`` "container"]
            [divAttr 
                [attr.``class`` "panel-group"; attr.id "accordion"]
                [workspaces |> renderWorkspaces]
            ]

    let body w h : Doc list =
        [
            divAttr [attr.id "embedReportHtml"; attr.hidden "true"] [text h]
            Main w
        ]


module Templating =
    open WebSharper.UI.Next.Html

    type MainTemplate = Templating.Template<"Main.html">

    let Main w h =
        Content.Page(     
            MainTemplate()
                .Body(FrontEnd.body w h)
                .Doc())
 
module Site =
    let private convert (pbiWs:PowerBI.Workspaces) : FrontEnd.Workspaces =
        pbiWs
        |> Map.map (fun _ w -> 
            {
            name = w.group.Name
            id = w.group.Id
            reports = w.reports 
                        |> Map.map (fun _ r -> 
                                {
                                name = r.Name
                                id = r.Id
                                groupId = w.group.Id
                                embedUrl = r.EmbedUrl
                                access = FrontEnd.Unknown}) })
    let workspaces = 
        async {return PowerBI.workspaces} 
        |> Async.RunSynchronously
        |> convert

    let private HomePage (ctx: Context<EndPoint>) =
        let path = sprintf "%sRep.html" ctx.RootFolder
        Templating.Main workspaces (File.ReadAllText(path))
        

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
