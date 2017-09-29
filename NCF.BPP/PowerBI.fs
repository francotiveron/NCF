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

type PBIGroup = Group
type PBIReport = Report

let private powerBiClientRefresh () =
    let ``as`` = ConfigurationManager.AppSettings
    let credential = new UserPasswordCredential(Credential.User1.Email, Credential.User1.Password);
    let authenticationContext = new AuthenticationContext(``as``.["AuthenticationAuthorityURL"])
    let authenticationResult = authenticationContext.AcquireTokenAsync(``as``.["PowerBIAuthURL"], ``as``.["AppClientId"], credential) |> Async.AwaitTask |> Async.RunSynchronously
    let tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer")
    new PowerBIClient(new Uri(``as``.["PowerBIApiURL"]), tokenCredentials)

let mutable private powerBiClient = powerBiClientRefresh()

type Report = {
    report : PBIReport
    mutable token : EmbedToken
    }

type Group = {
    group : PBIGroup
    reports : Map<string, Report>
    }

type Workspaces = Map<string, Group>

let private getReports gid =
    powerBiClient.Reports.GetReportsInGroup(gid).Value
    |> Seq.fold (fun (m:Map<string, Report>) (r:PBIReport) -> Map.add r.Id {report = r; token = EmbedToken()} m) Map.empty

let internal getGroups () = 
    powerBiClient.Groups.GetGroups().Value
    |> Seq.map (fun g -> g, getReports g.Id)
    |> Seq.fold (fun m (g, rs) -> Map.add g.Id {group = g; reports = rs} m) Map.empty

//let mutable internal workspaces = Map.empty

//let internal refresh () =
//    workspaces <- getGroups()

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
//let internal getEmbedToken gId rId =
//    try 
//        if workspaces.ContainsKey(gId) && workspaces.[gId].reports.ContainsKey(rId) then
//            let token = workspaces.[gId].reports.[rId].token
//            if token.Expiration.HasValue && token.Expiration.Value > DateTime.Now then
//                Ok token.Token
//            else
//                let tk = powerBiClient.Reports.GenerateTokenInGroup(gId, rId, generateTokenRequestParameters)
//                workspaces.[gId].reports.[rId].token <- tk
//                Ok tk.Token
//        else
//            Error "Report not Found"
//    with x -> 
//        Error x.Message
  //try 
   //     let token = powerBiClient.Reports.GenerateTokenInGroup(gId, rId, generateTokenRequestParameters)
   //     Ok ""
   // with x-> Error x.Message



(*
let private getReports gid =
    powerBiClient.Reports.GetReportsInGroup(gid).Value
    |> Seq.fold (fun (m:Map<string, Report>) (r:Report) -> Map.add r.Id r m) Map.empty

let private getGroups () = 
    powerBiClient.Groups.GetGroups().Value
    |> Seq.map (fun g -> g, getReports g.Id)
    |> Seq.fold (fun m (g, rs) -> Map.add g.Id {group = g; reports = rs} m) Map.empty

let mutable internal workspaces = Map.empty

let internal refresh () =
    workspaces <- getGroups()

let private generateTokenRequestParameters = new GenerateTokenRequest(accessLevel = "view")

let internal getEmbedToken gId rId =
    try Ok (powerBiClient.Reports.GenerateTokenInGroup(gId, rId, generateTokenRequestParameters).Token)
    with x-> Error x.Message
*)