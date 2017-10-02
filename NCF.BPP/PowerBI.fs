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
    let credential = new UserPasswordCredential(Credential.User1.Email, Credential.User1.Password);
    let authenticationContext = new AuthenticationContext(``as``.["AuthenticationAuthorityURL"])
    let authenticationResult = authenticationContext.AcquireTokenAsync(``as``.["PowerBIAuthURL"], ``as``.["AppClientId"], credential) |> Async.AwaitTask |> Async.RunSynchronously
    let tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer")
    new PowerBIClient(new Uri(``as``.["PowerBIApiURL"]), tokenCredentials)

let mutable private powerBiClient = powerBiClientRefresh()

type PBIGroup = Group
type PBIReport = Report

type Report = {
    report : PBIReport
    mutable token : EmbedToken
    }

type Group = {
    group : PBIGroup
    reports : Map<string, Report>
    }

let private getReports gid =
    powerBiClient.Reports.GetReportsInGroup(gid).Value
    |> Seq.fold (fun (m:Map<string, Report>) (r:PBIReport) -> Map.add r.Id {report = r; token = EmbedToken()} m) Map.empty

let internal getGroups () = 
    powerBiClient.Groups.GetGroups().Value
    |> Seq.map (fun g -> g, getReports g.Id)
    |> Seq.fold (fun m (g, rs) -> Map.add g.Id {group = g; reports = rs} m) Map.empty

let private generateTokenRequestParameters = new GenerateTokenRequest(accessLevel = "view")

let internal getEmbedToken gId rId =
    try 
        let token = powerBiClient.Reports.GenerateTokenInGroup(gId, rId, generateTokenRequestParameters)
        Ok (token.Token, token.Expiration)
    with _ -> 
        powerBiClient <- powerBiClientRefresh()
        try 
            let token = powerBiClient.Reports.GenerateTokenInGroup(gId, rId, generateTokenRequestParameters)
            Ok (token.Token, token.Expiration)
        with x -> 
            Error x.Message
