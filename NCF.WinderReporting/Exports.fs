module NCF.WinderReporting.Exports

open Microsoft.SqlServer.Server
open System

[<SqlProcedure>]
let ProcessWinderReportingMessages() =
    Processor.processLoop()
    0
