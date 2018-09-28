#load "bin\Debug\NCF.Data.fsx"
open NCF.Data

//let mutable lastSqlEvent = Unchecked.defaultof<FSharp.Data.Sql.Common.QueryEvents.SqlEventData>
//FSharp.Data.Sql.Common.QueryEvents.SqlQueryEvent |> Event.add (fun data -> lastSqlEvent <- data)
//Scada.Alarms("004").Filter(Time.Last24H) |> IO.print

let downtime = Ampla.Downtimes(Time.Last24H).Filter("Winder") |> IO.pick |> Seq.head
let alarm = Scada.Alarms(Time.Before downtime.Start) |> IO.pick |> Seq.head
let tags = [Pi.Tags.SCS_122ASRi01_AT_PowerAvg; Pi.Tags.SCS_122CV004_AT_P80; Pi.Tags.SCS_123CV006_AT_P80]
let data = Pi.Data(tags, Time.Before alarm.Start)
IO.chart data


//let ds = Ampla.Downtimes(Time.Last24H) :> IQuery<_>
//open NCFData_QueryBuilder
//let d = ncdQuery {
//    for d in ds do 
//    where (d.Equipment.Contains("Winder"))
//    take 3
//    select d
//}
//d |> Seq.toList
