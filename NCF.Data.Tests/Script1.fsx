#load "NCF.Data.fsx"
open System
open NCF.Data
open Deedle
open XPlot.Plotly

//let mutable lastSqlEvent = Unchecked.defaultof<FSharp.Data.Sql.Common.QueryEvents.SqlEventData>
//FSharp.Data.Sql.Common.QueryEvents.SqlQueryEvent |> Event.add (fun data -> lastSqlEvent <- data)

let dq1 = Ampla.Downtimes .* Last24H
let dq2 = Ampla.Downtimes .* Last24H .* "Winder"
let d1 = pick dq1
let d = d1.[0]
let a = Scada.Alarms .* (Before d.Start)
let al = pick a
let ala = al.[0]
let tags = [Pi.Tags.SCS_122CV004_AT_P80; Pi.Tags.SCS_123CV006_AT_P80]
let data = Pi.Data(tags, (Before ala.Start))
let data1 = Pi.Data(tags, (Before d.Start))
[data; data1] |> chart
