namespace NCF.Excel

open System
open System.Text.RegularExpressions
open FSharp.Data.TypeProviders
open ExcelDna.Integration
open System.Windows.Forms
open NetOffice.ExcelApi
open NetOffice.OfficeApi.Tools

module CitectAlarmHistory =
    open System.Data.Linq.SqlClient
    open System.Threading.Tasks

    [<Literal>]
    //let private connectionString = @"Data Source=203.15.179.12;Integrated Security=True;Initial Catalog=CitectAlarms;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    let private connectionString = @"Data Source=203.15.179.12;Integrated Security=False;User ID=ncf;Password=ncf1234;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    type dbSchema = SqlDataConnection<connectionString>
    let db = dbSchema.GetDataContext()

    //let cmd = new SqlCommandProvider<"select * from Information_schema.Columns where table_name = @tableName",connectionString>(connectionString)
    //let out = cmd.Execute(tableName="NCFEvent")
    //let fields = out |> Seq.map (fun x -> (x.COLUMN_NAME,x.DATA_TYPE)) |> Seq.toList

    let private _getHistory ug opd winder tFrom tTo =
        query {
            for ev in db.NCFEvent do
            where (SqlMethods.Like(ev.Location, (match ug, opd, winder with
                                                                                | true, false, false -> @"%UG%"
                                                                                | false, true, false -> @"%OPD%"
                                                                                | false, false, true -> @"%WINDER%"
                                                                                | true, true, false -> @"%[UO][GP]%"
                                                                                | true, false, true -> @"%[UW][GI]%"
                                                                                | false, true, true -> @"%[OW][PI]%"
                                                                                | _ -> @"%[UOW][GPI]%")
                                                                            ))
            where (ev.TimeOn >= tFrom && ev.TimeOn < tTo)
            take 5000
            sortBy ev.TimeOn
            select [box ev.Tag; box ev.Name; box ev.Desc; box ev.Location; box ev.TimeOn; box ev.TimeOff; box ev.Duration]
            }
    
    type QueryType =
        | Query of bool * bool * bool * DateTime * DateTime
        | ByTime of DateTime * DateTime
        | From of DateTime
        | Last24H

    let private getHistory query =
        let ug, opd, winder, tFrom, tTo = 
            match query with
                | Query (ug, opd, winder, tFrom, tTo) -> ug, opd, winder, tFrom, tTo
                | ByTime (tFrom, tTo) -> true, true, true, tFrom, tTo
                | From tFrom -> true, true, true, tFrom, DateTime.Now
                | Last24H -> true, true, true, DateTime.Now.AddDays(-1.), DateTime.Now

        _getHistory ug opd winder tFrom tTo

    let internal buildNewSheetFromQuery query =
        Cursor.Current <- Cursors.WaitCursor
        try
            let excelApplication = new Application(null, ExcelDna.Integration.ExcelDnaUtil.Application)
            excelApplication.DisplayAlerts <- false
            //let utils = new CommonUtils(excelApplication)
            let mutable wb = excelApplication.ActiveWorkbook
            let workSheet = match wb with
                            | null -> wb <- excelApplication.Workbooks.Add(); wb.Worksheets.[1]
                            | _ -> wb.Worksheets.Add()
                            :?> Worksheet

            excelApplication.ScreenUpdating <- false

            ["Tag"; "Name"; "Desc"; "Location"; "TimeOn"; "TimeOff"; "Duration"]
            |> Seq.iteri (fun i s -> workSheet.Cells.[1, i + 1].Value <- s)

            getHistory query |> Seq.iteri (fun i a -> a |> Seq.iteri (fun j o -> workSheet.Cells.[i + 2, j + 1].Value <- o))
            
            excelApplication.ScreenUpdating <- true
            workSheet.Columns.AutoFit() |> ignore
        with x ->
            MessageBox.Show(x.Message, "Error in gettin Citect Alarm History") |> ignore

        Cursor.Current <- Cursors.Default

    //[<ExcelFunction(Name = "NCF_GETCITECTALARMHISTORY", Description = "Get Citect Alarm Events", Category = "NCF-Citect Alarm History")>]
    //let getCitectAlarmHistory () =
    //    let a = array2D [seq {for i in 1..4 -> box (float i)} |> Seq.toArray; seq {for i in 5..8 -> box (float i)} |> Seq.toArray]
    //    NCF.Excel.AsyncFunctions.ArrayResizer.Resize(a)
