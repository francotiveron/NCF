module NCF.BPP.State

open System
open System.Threading.Tasks
open WebSharper.UI.Next.CSharp.Html.SvgAttributes
open PowerBI

type Metadata = 
    {
    ``type`` : PBIResourceType
    name : string
    id: string
    groupId: string
    embedUrl : string
    mutable embedToken : (string * DateTime) option
    }
    member this.Type 
        with get() = 
            match this.``type`` with
            | Dashboard -> "dashboard"
            | Report -> "report"
    member this.TypeId 
        with get() = 
            match this.``type`` with
            | Dashboard -> "D"
            | Report -> "R"

type Workspace = {
    name : string
    id : string
    resources : Map<string, Metadata>
    }

type Workspaces = Map<string, Workspace>
let mutable private workspaces = Map.empty

let private group2Workspace gId (group:Group) =
    {
    name = group.group.Name
    id = gId
    resources = group.resources
                |> Map.map (fun _ r -> 
                        {
                        ``type`` = r.``type``
                        name = r.Name
                        id = r.Id
                        groupId = gId
                        embedUrl = r.EmbedUrl
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
        if workspaces.ContainsKey(gId) && workspaces.[gId].resources.ContainsKey(id) then
            let m = workspaces.[gId].resources.[id]
            match m.embedToken with
            | Some (token, expiration) when (DateTime.UtcNow < expiration) -> Ok token
            | _ -> 
                match PowerBI.getEmbedToken m.``type`` gId id with
                | Ok (token, exp) -> m.embedToken <- Some (token, (if exp.HasValue then exp.Value else DateTime.UtcNow) ); Ok token
                | Error msg -> Error msg
        else
            Error "Report not Found"
    with x -> 
        Error x.Message
