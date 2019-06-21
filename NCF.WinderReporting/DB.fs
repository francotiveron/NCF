module NCF.WinderReporting.DB

open System
open FSharp.Data.Sql

[<Literal>]
let private connectionString = "Data Source=DESKTOP-0N8IBVK; Initial Catalog=WinderReporting;Integrated Security=True;Pooling=False"
[<Literal>]
let private dbVendor = Common.DatabaseProviderTypes.MSSQLSERVER
type private dbSchema = SqlDataProvider<dbVendor, connectionString, UseOptionTypes = true>
let private ctx = dbSchema.GetDataContext("Data Source=AUNPMZAP2; Initial Catalog=WinderReporting;Integrated Security=True;Pooling=False", SelectOperations.DatabaseSide)
//let private ctx = dbSchema.GetDataContext() //for test
let private db = ctx.Dbo

let saveEvent typ body =
    let record = db.NcfEventSink.Create()
    record.Type <- typ
    record.Body <- body
    ctx.SubmitUpdates()
    