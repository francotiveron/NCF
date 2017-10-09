module NCF.Private.Credential

open System.Security.Cryptography

type User = {
    email : string
    password : string
    appClientId : string
}


let user1 = {email = "npm.powerbi@npmine.onmicrosoft.com"; password = @"BIP@rk35"; appClientId = "9db25365-fe20-47c8-b33c-9cd13620ae88"}
