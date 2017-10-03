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
    mutable embedToken : (string * DateTime) option
    }

type Dashboard = {
    name : string
    id: string
    groupId: string
    embedUrl : string
    mutable embedToken : (string * DateTime) option
    }

type Workspace = {
    name : string
    id : string
    reports : Map<string, Report>
    dashboards : Map<string, Dashboard>
    }

type Workspaces = Map<string, Workspace>
let mutable private workspaces = Map.empty

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
                        }) 
    dashboards = group.dashboards 
                |> Map.map (fun _ d -> 
                        {
                        name = d.dashboard.DisplayName
                        id = d.dashboard.Id
                        groupId = gId
                        embedUrl = d.dashboard.EmbedUrl
                        embedToken = None
                        })                         
    }

let internal refresh () =
    workspaces <- PowerBI.getGroups() |> Map.map group2Workspace
    //lastRefresh <- DateTime.Now

let internal getWorkspaces () =
    //if (DateTime.Now - lastRefresh) > TimeSpan.FromHours(24.) then refresh()
    if workspaces.IsEmpty then refresh()
    workspaces

let internal getEmbedToken gId id =
    try 
        if workspaces.ContainsKey(gId) && workspaces.[gId].reports.ContainsKey(id) then
            let rpt = workspaces.[gId].reports.[id]
            match rpt.embedToken with
            | Some (token, expiration) when (DateTime.UtcNow < expiration) -> Ok token
            | _ -> 
                match PowerBI.getReportEmbedToken gId id with
                | Ok (token, exp) -> rpt.embedToken <- Some (token, (if exp.HasValue then exp.Value else DateTime.UtcNow) ); Ok token
                | Error msg -> Error msg
        else if workspaces.ContainsKey(gId) && workspaces.[gId].dashboards.ContainsKey(id) then
            let dbd = workspaces.[gId].dashboards.[id]
            match dbd.embedToken with
            | Some (token, expiration) when (DateTime.UtcNow < expiration) -> Ok token
            | _ -> 
                match PowerBI.getDashboardEmbedToken gId id with
                | Ok (token, exp) -> dbd.embedToken <- Some (token, (if exp.HasValue then exp.Value else DateTime.UtcNow) ); Ok token
                | Error msg -> Error msg
        else
            Error "Report not Found"
    with x -> 
        Error x.Message
