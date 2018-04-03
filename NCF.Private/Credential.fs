module NCF.Private.Credential

open System.Security.Cryptography

type User = {
    email : string
    password : string
    appClientId : string
}


//let user1 = {email = "npm.powerbi@npmine.onmicrosoft.com"; password = @"BIP@rk35"; appClientId = "9db25365-fe20-47c8-b33c-9cd13620ae88"}
//let user1 = {email = "franco.tiveron@northparkes.com"; password = @"TvrFnc66T23F"; appClientId = "41d4e4f3-2469-4525-8464-e523634e1aef"}
let user1 = {email = "npm.powerbi@northparkes.com"; password = @"BIP@rk35"; appClientId = "04eb5d0e-8a47-4f5a-b0a4-23c9f2c4401f"}
