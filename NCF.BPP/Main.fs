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

    let private renderReport groupId (r:Report) : Doc =
        divAttr 
            [
                attr.``data-`` "groupId" groupId
                attr.``data-`` "reportId" r.id
            ]
            [
                a [text r.name]
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
            div [client <@ Client.Main() @>]
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
    let private HomePage (ctx: Context<EndPoint>) =
        let path = sprintf "%sRep.html" ctx.RootFolder
        Templating.Main (State.getWorkspaces()) (File.ReadAllText(path))
        

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
