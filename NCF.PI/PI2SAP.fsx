#r "OSIsoft.AFSDK"
open OSIsoft.AF.PI
open OSIsoft.AF.Time
open System
open System.Net
open System.IO

[<Literal>]
let csvPathFormat = @"c:\temp\PI2SAP\PI_SAP_{0:ddMMyyyy}.CSV"
[<Literal>]
let csvHeader = @"MeasPnt,Time of Measurement,Date of Measurement,Counter Reading,Doc Text,Person Took Measurement"
[<Literal>]
let csvFormat = @"{0},{1:HHmmss},{1:ddMMyyyy},{2:0},PI2SAP - {3},PI Historian"

let collective = (new PIServers()).DefaultPIServer
let user = new NetworkCredential("pilab", "aafire")
collective.Connect(user)

let isValid s = not (String.IsNullOrWhiteSpace(s))

let tags = File.ReadLines(__SOURCE_DIRECTORY__ + @"\tags.txt") |> Seq.where isValid |> List.ofSeq

let dt = DateTime.Today.AddDays(-1.0).AddHours(23.0)

let print file =
    fprintfn file "%s" csvHeader
    tags |> Seq.map (
        fun tag -> 
            let pt = PIPoint.FindPIPoint(@"\\AUNPMZDBS1\" + tag)
            try 
                fprintfn file "%s" (String.Format(csvFormat, pt.Name, dt, pt.RecordedValue(new AFTime(dt))  , "OK"))
            with | x ->
                fprintfn file "%s" (String.Format(csvFormat, pt.Name, dt, -1, "ERROR " + x.Message))
        )

// Running from FSI, the script name is first, and other args after
match fsi.CommandLineArgs with
    | [| scriptName; "--toFile" |] -> 
        use file = File.CreateText(String.Format(csvPathFormat, dt))
        print file
    | _ -> print stdout
