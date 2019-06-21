module NCF.Excel.WRR //WinderReporting

open System
open FSharp.Data.Sql
open System.Data.Linq.SqlClient

[<Literal>]
let private connectionString = @"Data Source=203.15.179.12;Initial Catalog=WinderReporting;User ID=ncf;Password=ncf1234;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
[<Literal>]
let private dbVendor = Common.DatabaseProviderTypes.MSSQLSERVER
type private dbSchema = SqlDataProvider<dbVendor, connectionString>
let private db = dbSchema.GetDataContext(SelectOperations.DatabaseSide).Dbo

let private getDowntimes tFrom tTo =
    let qry = query {
        for ev in db.NcfWrrStates do
        where (ev.Id >= tFrom && ev.Id <= tTo)
        take 100000
        } 
    let records = qry |> Seq.toArray
    let downtimes = seq {
        let mutable i = 0
        while i < records.Length - 1 do
            if not records.[i].Status && records.[i + 1].Status
            then
                let ``begin`` = records.[i].Id
                let ``end`` = records.[i + 1].Id
                let duration = (``end`` - ``begin``).TotalDays
                yield [box ``begin``; box ``end``; box duration]
                i <- i + 2
            else
                i <- i + 1
    }
    ([box "Begin"; box "End"; box "Duration[m:s.ms]"] :: (downtimes |> Seq.toList)) |> array2D

let private getCycles tFrom tTo =
    let qry = query {
        for ev in db.NcfWrrCycles do
        where (ev.Start >= tFrom && ev.End <= tTo)
        take 100000
        select 
            [
            box ev.Start
            box ev.End
            null
            box (ev.End - ev.Start).TotalDays
            box (TimeSpan.FromMilliseconds((float)ev.LoadMs).TotalDays); 
            box (TimeSpan.FromMilliseconds((float)ev.UnloadMs).TotalDays); 
            box ev.Payload; 
            box (match (ev.Flags &&& 4s) with | 4s -> "WEST" | _ -> "EAST");
            box (ev.Flags &&& 3s); 
            ]
       } 
    ([
        box "Start"; 
        box "End"; 
        box "Gap[d:h:m:s.ms]"; 
        box "Duration[m:s.ms]"; 
        box "Load Time[m:s.ms]"; 
        box "Unload Time[m:s.ms]"; 
        box "Payload[kg]"; 
        box "Skip"; 
        box "Bin[1,2,3]"
     ] 
     :: (qry |> Seq.toList)
    ) |> array2D

let private newDowntimeSheet data = data |> XlUI.newSheet NCFExcelSheetFormat.WinderDowntimes
let private newCycleSheet data = data |> XlUI.newSheet NCFExcelSheetFormat.WinderCycles

let internal getLast24hDowntimes() = getDowntimes (DateTime.Now.AddDays(-1.)) DateTime.Now |> newDowntimeSheet

let internal getLast24hCycles() = getCycles (DateTime.Now.AddDays(-1.)) DateTime.Now |> newCycleSheet

let internal getQuery() =
    let ok, cycles, tFrom, tTo = UI.GetWRRQueryParameters()
    try
        if ok then 
            if cycles then getCycles tFrom tTo |> newCycleSheet
            else getDowntimes tFrom tTo |> newDowntimeSheet
        else ()
    with x -> System.Windows.Forms.MessageBox.Show(x.Message) |> ignore
