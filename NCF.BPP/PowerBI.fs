module NCF.BPP.PowerBI

open System
open System.Configuration
open Microsoft.PowerBI.Api.V2
open Microsoft.PowerBI.Api.V2.Models
open Microsoft.PowerBI.Security
open Microsoft.Rest
open Microsoft.IdentityModel.Clients.ActiveDirectory

type Report = {
    name : string
    id: string
    embedUrl : string
    token : string
    }

type Workspace = {
    name : string
    id : string
    reports : Report list
    }

let private powerBiClient =
    let credential = new UserPasswordCredential("franco.tiveron@northparkes.com", "TvrFnc66T23");
    let authenticationContext = new AuthenticationContext("https://login.windows.net/common/oauth2/authorize/")
    let authenticationResult = authenticationContext.AcquireTokenAsync("https://analysis.windows.net/powerbi/api", "41d4e4f3-2469-4525-8464-e523634e1aef", credential) |> Async.AwaitTask |> Async.RunSynchronously
    let tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer")
    new PowerBIClient(new Uri("https://api.powerbi.com/"), tokenCredentials)

let private getEmbedToken (pbic:PowerBIClient) gId rId =
    let generateTokenRequestParameters = new GenerateTokenRequest(accessLevel = "view")
    pbic.Reports.GenerateTokenInGroup(gId, rId, generateTokenRequestParameters).Token

let private getReports (pbic:PowerBIClient) (grp:Models.Group) = 
    pbic.Reports.GetReportsInGroup(grp.Id).Value
    |> Seq.map (fun r -> {name = r.Name; id = r.Id; embedUrl = r.EmbedUrl; token = getEmbedToken pbic grp.Id r.Id})
    |> Seq.toList

let private getGroups (pbic:PowerBIClient) = 
    pbic.Groups.GetGroups().Value
    |> Seq.map (fun g -> {name = g.Name; id = g.Id; reports = getReports pbic g })

let getWorkspaces () = 
    async {
        return powerBiClient |> getGroups
    } |> Async.RunSynchronously

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
