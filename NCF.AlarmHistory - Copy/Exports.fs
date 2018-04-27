module NCF.AlarmHistory.Exports

open Microsoft.SqlServer.Server

[<SqlProcedure>]
let ProcessCitectAlarmMessages() =
    Processor.processLoop()
    0