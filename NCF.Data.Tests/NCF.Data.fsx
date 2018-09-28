#r @"C:\Root\Project\Ocnarf\packages\Deedle.1.2.5\lib\net40\Deedle.dll"
#r @"C:\Root\Project\Ocnarf\packages\NetOffice.Excel.1.7.4.4\lib\net45\ExcelApi.dll"
#r @"C:\Root\Project\Ocnarf\packages\SQLProvider.1.1.44\lib\net451\FSharp.Data.SqlProvider.dll"
#r @"C:\Root\Project\NCF\NCF.Data.UI\bin\Debug\NCF.Data.UI.exe"
#r @"C:\Root\Project\Ocnarf\packages\Newtonsoft.Json.10.0.3\lib\net45\Newtonsoft.Json.dll"
#r @"C:\Root\Project\Ocnarf\Ocnarf.Ampla\bin\Debug\Ocnarf.Ampla.dll"
#r @"C:\Root\Project\Ocnarf\Ocnarf.Pi\bin\Debug\Ocnarf.Pi.dll"
#r @"C:\Program Files (x86)\PIPC\AF\PublicAssemblies\4.0\OSIsoft.AFSDK.dll"
#r @"C:\Root\Project\Ocnarf\packages\XPlot.Plotly.1.4.5\lib\net45\XPlot.Plotly.dll"
#r @"C:\Root\Project\NCF\NCF.Data\bin\Debug\NCF.Data.dll"


let prt1 (dt: System.DateTime) = dt.ToString()
do fsi.AddPrinter prt1
do fsi.AddPrinter(fun (printer:Deedle.Internal.IFsiFormattable) -> "\n" + (printer.Format()))