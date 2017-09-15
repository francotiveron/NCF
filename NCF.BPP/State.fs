module NCF.BPP.State

open System
open System.Threading.Tasks
open WebSharper.UI.Next.CSharp.Html.SvgAttributes

type Report = {
    name : string
    id: string
    groupId: string
    embedUrl : string
    }

type Workspace = {
    name : string
    id : string
    reports : Map<string, Report>
    }

type Workspaces = Map<string, Workspace>

let private getEmbedToken gid rid = 
    async {return PowerBI.getEmbedToken gid rid} |> Async.StartAsTask

let private convert (w:PowerBI.Workspaces) =
    w
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
                            }) })

let mutable private workspaces = Map.empty
let mutable private lastRefresh = DateTime.MinValue

let internal refresh () =
    PowerBI.refresh()
    workspaces <- convert PowerBI.workspaces
    lastRefresh <- DateTime.Now

let internal getWorkspaces () =
    if (DateTime.Now - lastRefresh) > TimeSpan.FromHours(24.) then refresh()
    workspaces