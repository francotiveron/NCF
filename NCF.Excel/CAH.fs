module NCF.Excel.CAH //CitectAlarmHistory

open System
open FSharp.Data.Sql
open System.Data.Linq.SqlClient

[<Literal>]

let private connectionString = @"Data Source=203.15.179.12;Initial Catalog=CitectAlarms;Integrated Security=False;User ID=ncf;Password=ncf1234;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
//let private connectionString = @"Server=203.15.179.12; Database=CitectAlarms;User ID=ncf;Password=ncf1234;Connect Timeout=30"
[<Literal>]
let private dbVendor = Common.DatabaseProviderTypes.MSSQLSERVER
type private dbSchema = SqlDataProvider<dbVendor, connectionString>
let private db = dbSchema.GetDataContext(SelectOperations.DatabaseSide).Dbo

type private QueryType =
    | Query of bool * bool * bool * DateTime * DateTime
    | ByTime of DateTime * DateTime
    | From of DateTime
    | Last24H

let private _getHistory ug opd winder tFrom tTo =
    query {
        for ev in db.NcfEvent do
        where (ev.Location.Contains(match ug, opd, winder with
                                        | true, false, false -> @"%UG%"
                                        | false, true, false -> @"%OPD%"
                                        | false, false, true -> @"%WINDER%"
                                        | true, true, false -> @"%[UO][GP]%"
                                        | true, false, true -> @"%[UW][GI]%"
                                        | false, true, true -> @"%[OW][PI]%"
                                        | _ -> @"%[UOW][GPI]%")
                                    )
        where (ev.TimeOn >= tFrom && ev.TimeOn < tTo)
        take 100000
        sortBy ev.TimeOn
        select [box ev.Tag; box ev.Name; box ev.Desc; box ev.Location; box ev.TimeOn; box ev.TimeOff; box ev.Duration]
        } 

let private getHistory query =
    let ug, opd, winder, tFrom, tTo = 
        match query with
            | Query (ug, opd, winder, tFrom, tTo) -> ug, opd, winder, tFrom, tTo
            | ByTime (tFrom, tTo) -> true, true, true, tFrom, tTo
            | From tFrom -> true, true, true, tFrom, DateTime.Now
            | Last24H -> true, true, true, DateTime.Now.AddDays(-1.), DateTime.Now


    ([box "Tag"; box "Name"; box "Desc"; box "Location"; box "TimeOn"; box "TimeOff"; box "Duration"] 
    :: ((_getHistory ug opd winder tFrom tTo) 
    |> Seq.toList)) 
    |> array2D

let private newSheet data = data |> XlUI.newSheet NCFExcelSheetFormat.AlarmHistory

let internal getLast24h () =
    getHistory Last24H |> newSheet

let internal getByTime () =
    let ok, _, _, _, tFrom, tTo = UI.GetCAHQueryParameters()
    try
        if ok then getHistory (ByTime (tFrom, tTo)) |> newSheet
        else ()
    with x -> System.Windows.Forms.MessageBox.Show(x.Message) |> ignore

let internal getByZone () =
    let ok, ug, opd, wi, tFrom, tTo = UI.GetCAHQueryParameters(true)
    if ok then getHistory (Query (ug, opd, wi, tFrom, tTo)) |> newSheet
    else ()


//[<ExcelFunction(Name = "NCF_GETCITECTALARMHISTORY", Description = "Get Citect Alarm Events", Category = "NCF-Citect Alarm History")>]
//let getCitectAlarmHistory () =
//    let a = array2D [seq {for i in 1..4 -> box (float i)} |> Seq.toArray; seq {for i in 5..8 -> box (float i)} |> Seq.toArray]
//    NCF.Excel.AsyncFunctions.ArrayResizer.Resize(a)
