#load "bin\Debug\NCF.Data.fsx"
open NCF.Data
open System

let data = Pi.Data([Pi.Tags.SCS_122ASRi01_AT_PressurePeakAvg], Time.Last (TimeSpan.FromDays(10.0)), "'SCS_122ASRi01_AT_PressurePeakAvg' > 8")
let getAlarms time = Scada.Alarms(Time.Around time)
let alla = seq {for t in data.RowKeys do yield! (getAlarms t)}
let als = IO.pick alla
let alarms = getAlarms DateTime.Today
let all = IO.pick alarms
IO.print alarms
IO.print alla
