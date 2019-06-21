namespace NCF.Excel

open System
open NetOffice.ExcelApi
open ExcelDna.Integration

type NCFExcelSheetFormat = AlarmHistory | WinderDowntimes | WinderCycles

module XlUI =
    let private fillSheet format (data : obj[,]) =
        let applyFormat format = 
            let rng = new ExcelReference(0, data.GetLength(0) - 1, 0, data.GetLength(1) - 1)
            rng.SetValue(data) |> ignore
            XlCall.Excel(XlCall.xlcSelectEnd, 2) |> ignore
            XlCall.Excel(XlCall.xlcSelectEnd, 4) |> ignore
            match format with
            | AlarmHistory ->
                XlCall.Excel(XlCall.xlcSelect, new ExcelReference(rng.RowFirst, rng.RowLast, 4, 5)) |> ignore
                XlCall.Excel(XlCall.xlcFormatNumber, "dd/MMM/yyyy HH:mm:ss") |> ignore
            | WinderDowntimes ->
                XlCall.Excel(XlCall.xlcSelect, new ExcelReference(rng.RowFirst, rng.RowLast, 0, 1)) |> ignore
                XlCall.Excel(XlCall.xlcFormatNumber, "dd/MMM/yyyy HH:mm:ss.000") |> ignore
                XlCall.Excel(XlCall.xlcSelect, new ExcelReference(rng.RowFirst, rng.RowLast, 2, 2)) |> ignore
                XlCall.Excel(XlCall.xlcFormatNumber, "d:hh:mm:ss.000") |> ignore
            | WinderCycles -> 
                XlCall.Excel(XlCall.xlcSelect, new ExcelReference(rng.RowFirst, rng.RowLast, 0, 1)) |> ignore
                XlCall.Excel(XlCall.xlcFormatNumber, "dd/MMM/yyyy HH:mm:ss.000") |> ignore
                XlCall.Excel(XlCall.xlcSelect, new ExcelReference(rng.RowFirst, rng.RowLast, 2, 2)) |> ignore
                XlCall.Excel(XlCall.xlcFormatNumber, "d:hh:mm:ss.000") |> ignore
                XlCall.Excel(XlCall.xlcSelect, new ExcelReference(rng.RowFirst, rng.RowLast, 3, 5)) |> ignore
                XlCall.Excel(XlCall.xlcFormatNumber, "mm:ss.000") |> ignore
                for i = rng.RowFirst + 2 to rng.RowLast do
                    let curStart:DateTime = unbox data.[i, 0]
                    let prevEnd:DateTime = unbox data.[i - 1, 1]
                    if curStart > prevEnd then
                        XlCall.Excel(XlCall.xlcSelect, new ExcelReference(i, i, 2, 2)) |> ignore
                        XlCall.Excel(XlCall.xlcFormula, "=(r[0]c[-2] - r[-1]c[-1])") |> ignore
            let colparams = [|box 10; box rng; box false; box 3|]
            XlCall.Excel(XlCall.xlcColumnWidth, colparams) |> ignore
            XlCall.Excel(XlCall.xlcSelectEnd, 1) |> ignore
            XlCall.Excel(XlCall.xlcSelectEnd, 3) |> ignore


        ExcelAsyncUtil.QueueAsMacro(fun () -> applyFormat format)

    let internal newSheet format data =
        let excelApplication = new Application(null, ExcelDna.Integration.ExcelDnaUtil.Application)

        //excelApplication.DisplayAlerts <- false
        let mutable wb = excelApplication.ActiveWorkbook
        let workSheet = match wb with
                        | null -> wb <- excelApplication.Workbooks.Add(); wb.Worksheets.[1]
                        | _ -> wb.Worksheets.Add()
                        :?> Worksheet
        fillSheet format data 
