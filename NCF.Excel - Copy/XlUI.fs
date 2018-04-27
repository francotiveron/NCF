namespace NCF.Excel

open NetOffice.ExcelApi
open ExcelDna.Integration

module XlUI =
    let private fillSheet (data : obj[,]) =
        ExcelAsyncUtil.QueueAsMacro(fun () -> 
            let rng = new ExcelReference(0, data.GetLength(0) - 1, 0, data.GetLength(1) - 1)
            rng.SetValue(data) |> ignore
            XlCall.Excel(XlCall.xlcSelectEnd, 2) |> ignore
            XlCall.Excel(XlCall.xlcSelectEnd, 4) |> ignore
            XlCall.Excel(XlCall.xlcSelect, new ExcelReference(rng.RowFirst, rng.RowLast, 4, 5)) |> ignore
            XlCall.Excel(XlCall.xlcFormatNumber, "dd/MMM/yyyy HH:mm:ss") |> ignore
            let colparams = [|box 10; box rng; box false; box 3|]
            XlCall.Excel(XlCall.xlcColumnWidth, colparams) |> ignore
            XlCall.Excel(XlCall.xlcSelectEnd, 1) |> ignore
            XlCall.Excel(XlCall.xlcSelectEnd, 3) |> ignore
        )

    let internal newSheet data =
        let excelApplication = new Application(null, ExcelDna.Integration.ExcelDnaUtil.Application)
        excelApplication.DisplayAlerts <- false
        let mutable wb = excelApplication.ActiveWorkbook
        let workSheet = match wb with
                        | null -> wb <- excelApplication.Workbooks.Add(); wb.Worksheets.[1]
                        | _ -> wb.Worksheets.Add()
                        :?> Worksheet
        fillSheet data
