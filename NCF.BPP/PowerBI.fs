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
open NCF.Private

let private powerBiClientRefresh () =
    let ``as`` = ConfigurationManager.AppSettings
    let credential = new UserPasswordCredential(Credential.user1.email, Credential.user1.password);
    let authenticationContext = new AuthenticationContext(``as``.["AuthenticationAuthorityURL"])
    let authenticationResult = authenticationContext.AcquireTokenAsync(``as``.["PowerBIAuthURL"], Credential.user1.appClientId, credential) |> Async.AwaitTask |> Async.RunSynchronously
    let tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer")
    new PowerBIClient(new Uri(``as``.["PowerBIApiURL"]), tokenCredentials)

let mutable private powerBiClient = powerBiClientRefresh()

type PBIGroup = Group
type PBIReport = Report
type PBIDashboard = Dashboard
type PBIResourceType = Dashboard | Report
type PBIResource = D of PBIDashboard | R of PBIReport

type Report = {
    report : PBIReport
    mutable token : EmbedToken
    }

type Dashboard = {
    dashboard : PBIDashboard
    mutable token : EmbedToken
    }

type Resource = 
    {
    ``type``: PBIResourceType
    resource : PBIResource
    mutable token : EmbedToken
    }
    static member getId(r) = 
        match r with
        | D d -> d.Id
        | R r -> r.Id
    member this.Id
        with get() = Resource.getId(this.resource)
    member this.Name
        with get() =
            match this.resource with
            | D d -> d.DisplayName
            | R r -> r.Name
    member this.EmbedUrl
        with get() =
            match this.resource with
            | D d -> d.EmbedUrl
            | R r -> r.EmbedUrl

type Group = {
    group : PBIGroup
    resources : Map<string, Resource>
    }


let private getResources gid =
    seq {
        yield! powerBiClient.Dashboards.GetDashboardsInGroup(gid).Value |> Seq.map (fun d -> Dashboard, D d)
        yield! powerBiClient.Reports.GetReportsInGroup(gid).Value |> Seq.map (fun r -> Report, R r)
    }
    |> Seq.fold (fun (m:Map<string, Resource>) (t:PBIResourceType, r:PBIResource) -> Map.add (Resource.getId(r)) {``type`` = t; resource = r; token = EmbedToken()} m) Map.empty

let internal getGroups () = 
    powerBiClient.Groups.GetGroups().Value
    |> Seq.map (fun g -> g, getResources g.Id)
    |> Seq.fold (fun m (g, rs) -> Map.add g.Id {group = g; resources = rs} m) Map.empty

let private generateTokenRequestParameters = new GenerateTokenRequest(accessLevel = "view")

let internal getEmbedToken ``type`` gId id =
    let generator () = 
        match ``type`` with
        | Dashboard -> powerBiClient.Dashboards.GenerateTokenInGroup
        | Report -> powerBiClient.Reports.GenerateTokenInGroup

    try 
        let token = generator()(gId, id, generateTokenRequestParameters)
        Ok (token.Token, token.Expiration)
    with _ -> 
        powerBiClient <- powerBiClientRefresh()
        try 
            let token = generator()(gId, id, generateTokenRequestParameters)
            Ok (token.Token, token.Expiration)
        with x -> 
            Error x.Message

