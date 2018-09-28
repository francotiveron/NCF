#r @"Deedle.dll"
#r @"ExcelApi.dll"
#r @"FSharp.Data.SqlProvider.dll"
#r @"NCF.Data.UI.exe"
#r @"Newtonsoft.Json.dll"
#r @"Ocnarf.Ampla.dll"
#r @"Ocnarf.Pi.dll"
#r @"OSIsoft.AFSDK.dll"
#r @"XPlot.Plotly.dll"
#r @"NCF.Data.dll"

module private Common = 
    let prt1 (dt: System.DateTime) = dt.ToString()

do fsi.AddPrinter Common.prt1
do fsi.AddPrinter(fun (printer:Deedle.Internal.IFsiFormattable) -> "\n" + (printer.Format()))