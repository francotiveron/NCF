module NCF.BPP.Reports

open System
open System.Configuration
open Microsoft.PowerBI.Api.V2
open Microsoft.PowerBI.Api.V2.Models
open Microsoft.PowerBI.Security
open Microsoft.Rest
open Microsoft.IdentityModel.Clients.ActiveDirectory
//workspaceCollection .. roles are for real apps (to generate new real accessToken
//code for real apps look  at ReportTest() not ReportSample()
let workspaceCollection = ConfigurationManager.AppSettings.["powerbi:WorkspaceCollection"]
let workspaceId = ConfigurationManager.AppSettings.["powerbi:WorkspaceId"]
let accessKey = ConfigurationManager.AppSettings.["powerbi:AccessKey"]
let apiUrl = ConfigurationManager.AppSettings.["powerbi:ApiUrl"]
let roles = ConfigurationManager.AppSettings.["powerbi:Roles"]

    //<add key="authorityUrl" value="https://login.windows.net/common/oauth2/authorize/" />
    //<add key="resourceUrl" value="https://analysis.windows.net/powerbi/api" />
    //<add key="apiUrl" value="https://api.powerbi.com/" />
    //<add key="embedUrlBase" value="https://app.powerbi.com/" />
    //<add key="clientId" value="41d4e4f3-2469-4525-8464-e523634e1aef" />
    //<add key="groupId" value="" />

    //<!-- Note: Do NOT leave your credentials on code. Save them in secure place. -->
    //<add key="pbiUsername" value="franco.tiveron@northparkes.com" />
    //<add key="pbiPassword" value="XXXXXXX" />


//look at description in App.conf how to get fresh sample and ready token - look at ReportSample()
let sampleAccessToken = ConfigurationManager.AppSettings.["powerbi:AccessToken"]
    
let expiration = ConfigurationManager.AppSettings.["powerbi:Expiration"] //minutes

let createRoles () = 
    seq {yield roles}

let createExpiration() = 
    TimeSpan.FromMinutes (float expiration)

let powerBiClient () =
    let credential = new UserPasswordCredential("franco.tiveron@northparkes.com", "TvrFnc66T23");
    let authenticationContext = new AuthenticationContext("https://login.windows.net/common/oauth2/authorize/")
    let authenticationResult = authenticationContext.AcquireTokenAsync("https://analysis.windows.net/powerbi/api", "41d4e4f3-2469-4525-8464-e523634e1aef", credential)
                                |> Async.AwaitTask |> Async.RunSynchronously
    let tokenCredentials = new TokenCredentials(authenticationResult.AccessToken, "Bearer")
    let client = new PowerBIClient(new Uri("https://api.powerbi.com/"), tokenCredentials)
    client

type ReportType = {
    ReportId : string
    EmbedUrl : string
    AccessToken : string
}

let Report () =
    use client = powerBiClient()                    
    let groups = client.Groups.GetGroupsAsync() |> Async.AwaitTask |> Async.RunSynchronously
    let groupId = groups.Value.[2].Id
    let reports = client.Reports.GetReportsInGroupAsync(groupId) |> Async.AwaitTask |> Async.RunSynchronously
    let report = reports.Value.[1]
    //let embedToken = PowerBIToken.CreateReportEmbedToken(workspaceCollection, workspaceId, report.Value.Id)
    let generateTokenRequestParameters = new GenerateTokenRequest(accessLevel = "view")
    let embedToken = client.Reports.GenerateTokenInGroupAsync(groupId, report.Id, generateTokenRequestParameters) |> Async.AwaitTask |> Async.RunSynchronously
    {ReportId = report.Id; EmbedUrl = report.EmbedUrl; AccessToken = embedToken.Token}


let ReportTest () = 
    Report()