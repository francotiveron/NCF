module NCF.BPP.PowerBI

open System
open System.Configuration
open Microsoft.PowerBI.Api.V2
open Microsoft.PowerBI.Api.V2.Models
open Microsoft.PowerBI.Security
open Microsoft.Rest
open Microsoft.IdentityModel.Clients.ActiveDirectory
open System.Runtime.InteropServices
open WebSharper.Sitelets.Content

let private powerBiClient =
    let credential = new UserPasswordCredential("franco.tiveron@northparkes.com", "TvrFnc66T23");
    let authenticationContext = new AuthenticationContext("https://login.windows.net/common/oauth2/authorize/")
    let authenticationResult = authenticationContext.AcquireTokenAsync("https://analysis.windows.net/powerbi/api", "41d4e4f3-2469-4525-8464-e523634e1aef", credential) |> Async.AwaitTask |> Async.RunSynchronously
    let tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer")
    new PowerBIClient(new Uri("https://api.powerbi.com/"), tokenCredentials)

type Workspace = {
    group : Group
    reports : Map<string, Report>
    }

type Workspaces = Map<string, Workspace>

let private reports gid =
    powerBiClient.Reports.GetReportsInGroup(gid).Value
    |> Seq.fold (fun (m:Map<string, Report>) (r:Report) -> Map.add r.Id r m) Map.empty

let private groups : Workspaces = 
    powerBiClient.Groups.GetGroups().Value
    |> Seq.map (fun g -> g, reports g.Id)
    |> Seq.fold (fun m (g, rs) -> Map.add g.Id {group = g; reports = rs} m) Map.empty

let workspaces = groups


//let private getEmbedToken (pbic:PowerBIClient) gId rId =
//    let generateTokenRequestParameters = new GenerateTokenRequest(accessLevel = "view")
//    try pbic.Reports.GenerateTokenInGroup(gId, rId, generateTokenRequestParameters).Token, true
//    with x -> x.Message, false

//let private buildReport (pbic:PowerBIClient) (gId:string) (r:Models.Report) =
//    let t, p = getEmbedToken pbic gId r.Id
//    {name = r.Name; embedUrl = r.EmbedUrl; ``public`` = p; token = t}

//let private getReports (pbic:PowerBIClient) (grp:Models.Group) = 
//    pbic.Reports.GetReportsInGroup(grp.Id).Value
//    |> Seq.map (buildReport pbic grp.Id)
//    |> Seq.toList

//let private getGroups (pbic:PowerBIClient) = 
//    pbic.Groups.GetGroups().Value
//    |> Seq.map (fun g -> {name = g.Name; reports = getReports pbic g })

//let getWorkspaces () = 
//    async {
//        return powerBiClient |> getGroups
//    } |> Async.RunSynchronously |> Seq.toList

    //let! groups = getGroupsAsync
    //let getWorkspace (group:Group) =
    //    {name = group.Name; id = group.Id; reports = []}
    //use! client = powerBiClient()                    
    //let! groups = client.Groups.GetGroupsAsync() |> Async.AwaitTask
    //let groupId = groups.Value.[2].Id
    //let reports = client.Reports.GetReportsInGroupAsync(groupId) |> Async.AwaitTask |> Async.RunSynchronously
    //let report = reports.Value.[1]
    ////let embedToken = PowerBIToken.CreateReportEmbedToken(workspaceCollection, workspaceId, report.Value.Id)
    //let generateTokenRequestParameters = new GenerateTokenRequest(accessLevel = "view")
    //let embedToken = client.Reports.GenerateTokenInGroupAsync(groupId, report.Id, generateTokenRequestParameters) |> Async.AwaitTask |> Async.RunSynchronously
    //{ReportId = report.Id; EmbedUrl = report.EmbedUrl; AccessToken = embedToken.Token}
    //return []
