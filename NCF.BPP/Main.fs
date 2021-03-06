namespace NCF.BPP

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open System.IO

type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/about">] About
    | [<EndPoint "/refresh">] Refresh

module private Util = 
    let appPath (ctx: Context<EndPoint>) = 
        if ctx.ApplicationPath.Equals("/") then System.String.Empty else ctx.ApplicationPath

module FrontEnd =
    open WebSharper.UI.Next.Html
    open State
    open WebSharper.UI.Next.Html.Tags
    open System.Web

    let private renderResource groupId (m:Metadata) : Doc =
        divAttr 
            [
                attr.``data-`` "pbiType" m.Type
                attr.``data-`` "groupId" groupId
                attr.``data-`` "resourceId" m.id
                attr.``data-`` "embedUrl" m.embedUrl
            ]
            [
                aAttr [attr.href "#"; on.click <@ Client.pbiLinkClicked @>]  [text (m.TypeId + " -> " + m.name)]
            ] 
            :> Doc

    let private renderResources (w:Workspace) (resources:Map<string, Metadata>) = 
        resources
        |> Map.toSeq
        |> Seq.map (fun (_, r) -> r)
        |> Seq.filter (fun r -> r.``type`` = PowerBI.Dashboard || w.name <> "Asset Management")
        |> Seq.sortBy (fun r -> r.name)
        |> Seq.map (fun r -> renderResource w.id r)
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
                    //([w.resources |> renderResources w.id])
                    ([w.resources |> renderResources w])
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
        ]

    let bodyHome workspaces rptPageHtml : Doc list =
        [
        //Doc.WebControl(new Web.Require<Resources.BootStrapJS>())
        divAttr [attr.``class`` "page-header"]
            [
            h2Attr [attr.``class`` "text-center"] [text "Power BI Reports"]
            ]
        divAttr [attr.``class`` "jumbotron"] (home workspaces rptPageHtml)
        ]
   
    let bodyAbout () : Doc list =
        //Doc.WebControl(new Web.Require<Resources.BootStrapJS>())
        //:: 
        (About.paragraphs 
            |> List.map (fun (title, content) -> [h2 [text title] :> Doc; pAttr [attr.``class`` "about-text"] content :> Doc])
            |> List.concat)
            //@ [div [text (HttpContext.Current.User.Identity.Name)]]

module Templating =
    open WebSharper.UI.Next.Html

    type Template = Templating.Template<"Template1.html">

    let Home (ctx: Context<EndPoint>) workspaces html =
        Content.Page(     
            Template()
                .Body(FrontEnd.bodyHome workspaces html)
                .HomeActive("active")
                .Root(Util.appPath ctx)
                .Doc())

    let About (ctx: Context<EndPoint>) = 
        Content.Page(     
            Template()
                .Body(FrontEnd.bodyAbout())
                .AboutActive("active")
                .Root(Util.appPath ctx)
                .Doc())

 
module Site =
    let private HomePage (ctx: Context<EndPoint>) =
        let path = sprintf "%sRep.html" ctx.RootFolder
        Templating.Home ctx (State.getWorkspaces()) (File.ReadAllText(path).Replace(@"${Root}", Util.appPath ctx))
        

    let private AboutPage (ctx: Context<EndPoint>) =
        Templating.About ctx

    [<Website>]
    let Main = 
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home -> HomePage ctx
            | EndPoint.About -> AboutPage ctx
            | EndPoint.Refresh -> State.refresh(); HomePage ctx
        )
