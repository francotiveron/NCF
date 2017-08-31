namespace NCF.Excel

open System
open ExcelDna.Integration

type AddIn () =
    let _manageExc (x:obj) = 
        match x with
        | :? Exception -> box (x.ToString())
        | _ -> box "???"

    interface IExcelAddIn with
        member this.AutoOpen() = 
            ()
            //ExcelAsyncUtil.Initialize()
            ExcelIntegration.RegisterUnhandledExceptionHandler(new UnhandledExceptionHandler(_manageExc))
        member this.AutoClose() = 
            ()


