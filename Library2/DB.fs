module DB

open FSharp.Data.Sql

[<Literal>]
let private dbVendor = Common.DatabaseProviderTypes.MSSQLSERVER
