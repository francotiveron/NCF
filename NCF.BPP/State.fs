module NCF.BPP.State

open System
open System.Threading.Tasks
open WebSharper.UI.Next.CSharp.Html.SvgAttributes
open PowerBI

type Report = {
    name : string
    id: string
    groupId: string
    embedUrl : string
    mutable embedToken : string option
    }

type Workspace = {
    name : string
    id : string
    reports : Map<string, Report>
    }

type Workspaces = Map<string, Workspace>

let private group2Workspace gId (group:Group) =
    {
    name = group.group.Name
    id = gId
    reports = group.reports 
                |> Map.map (fun _ r -> 
                        {
                        name = r.report.Name
                        id = r.report.Id
                        groupId = gId
                        embedUrl = r.report.EmbedUrl
                        embedToken = None
                        }) }

let mutable private workspaces = Map.empty
let mutable private lastRefresh = DateTime.MinValue

let internal refresh () =
    workspaces <- PowerBI.getGroups() |> Map.map group2Workspace
    lastRefresh <- DateTime.Now

let internal getWorkspaces () =
    if (DateTime.Now - lastRefresh) > TimeSpan.FromHours(24.) then refresh()
    workspaces

let internal getEmbedToken gId rId =
    try 
        if workspaces.ContainsKey(gId) && workspaces.[gId].reports.ContainsKey(rId) then
            let rpt = workspaces.[gId].reports.[rId]
            match rpt.embedToken with
            | Some token -> Ok token
            | None -> 
                match PowerBI.getEmbedToken gId rId with
                | Ok token -> rpt.embedToken <- Some token; Ok token
                | it -> it
        else
            Error "Report not Found"
    with x -> 
        Error x.Message
