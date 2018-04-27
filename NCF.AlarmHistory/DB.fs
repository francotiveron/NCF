module NCF.AlarmHistory.DB

open System
open FSharp.Data.Sql
open CitectAlarmEvent

//type private dbSchema = SqlDataConnection<"Data Source=DESKTOP-0N8IBVK; Initial Catalog=CitectAlarms;Integrated Security=True;Pooling=False">
//let private db = dbSchema.GetDataContext()
//let private conn = db.Connection
//conn.ConnectionString <- "context connection=true"
//conn.Open()

[<Literal>]
let private connectionString = "Data Source=DESKTOP-0N8IBVK; Initial Catalog=CitectAlarms;Integrated Security=True;Pooling=False"
[<Literal>]
let private dbVendor = Common.DatabaseProviderTypes.MSSQLSERVER
type private dbSchema = SqlDataProvider<dbVendor, connectionString>
let private ctx = dbSchema.GetDataContext(SelectOperations.DatabaseSide)
let private db = ctx.Dbo

let private saveEvent (e:CitectAlarmEvent) =
    db.NcfEventSink.Create(
        Tag = e.tag, 
        Name = e.name, 
        Desc = e.desc,
        Location = "cmoc/npm/" + e.zone + "/" + (string e.area),
        Priority = e.priority,
        TimeOn = e.timeOn.Value, 
        TimeOff = e.timeOff.Value,
        Duration = (float32 (e.timeOff.Value - e.timeOn.Value).TotalSeconds)
        )
    |> ignore


//let private saveEvent (e:CitectAlarmEvent) =
//    let r = 
//        new dbSchema.ServiceTypes.NCFEventSink(
//            Tag = e.tag, 
//            Name = e.name, 
//            Desc = e.desc,
//            Location = "cmoc/npm/" + e.zone + "/" + (string e.area),
//            Priority = e.priority,
//            TimeOn = e.timeOn.Value, 
//            TimeOff = e.timeOff.Value,
//            Duration = (float32 (e.timeOff.Value - e.timeOn.Value).TotalSeconds)
//            )
//    db.NCFEventSink.InsertOnSubmit(r)

//Recordable events are OFF with event time >= OffTime and OffTime > OnTime
let manage (e:CitectAlarmEvent) (logger: string->unit) = 
    match e.state, e.time, e.timeOn, e.timeOff with
    | On, _, _, _ -> ()
    | _, _, None, _ -> ()
    | NotOn, t, Some tOn, Some tOff when t >= tOff && tOff > tOn -> saveEvent e
    | _ -> logger "Invalid Event"

    try ctx.SubmitUpdates() with
    | x -> logger x.Message

//let manage (e:CitectAlarmEvent) (logger: string->unit) = 
//    match e.state, e.time, e.timeOn, e.timeOff with
//    | On, _, _, _ -> ()
//    | _, _, None, _ -> ()
//    | NotOn, t, Some tOn, Some tOff when t >= tOff && tOff > tOn -> 
//        if query {for event in db.NCFEvent do
//                  select (event.Tag, event.TimeOn, event.TimeOff)
//                  contains (e.tag, tOn, tOff)}
//        then () else saveEvent e
//    | _ -> logger "Invalid Event"

//    try db.DataContext.SubmitChanges() with
//    | x -> logger x.Message

//let quarantine message reason = 
//    let nr = new dbSchema.ServiceTypes.NCFQuarantinedMessage(Message = message, Reason = reason)
//    db.NCFQuarantinedMessage.InsertOnSubmit(nr)
//    db.DataContext.SubmitChanges()

let quarantine message reason = 
    db.NcfQuarantinedMessage.Create(Message = message, Reason = reason) |> ignore
    ctx.SubmitUpdates()

