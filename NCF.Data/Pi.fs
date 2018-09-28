namespace NCF.Data

open System
open OSIsoft.AF.PI
open OSIsoft.AF.Time
open OSIsoft.AF.Data
open Ocnarf.Pi
open System.Net
open Deedle


[<AutoOpen>]
module Pi = 
    open Time

    let [<Literal>] private NPMPI = "AUNPMZDBS1"
    let [<Literal>] private NPMPIUser = "pilab"
    let [<Literal>] private NPMPIPwd = "aafire"

    type private proPi = Ocnarf.TypeProviders.PiProvider<NPMPI>
    let internal getPiServer name = 
        match name with
        | "" -> PIServers().DefaultPIServer
        | name -> PIServers().Item(name)

    type PiServer() =
        let pi = getPiServer NPMPI
        do pi.Connect(NetworkCredential(NPMPIUser, NPMPIPwd))
        let getTagValues (dtFrom: DateTime) (dtTo: DateTime) (filter:string) (tag: Tag) = 
            let pt = PIPoint.FindPIPoint(pi, tag.Name)
            pt.RecordedValues(AFTimeRange(AFTime(dtFrom), AFTime(dtTo)), AFBoundaryType.Interpolated, filter, false)
            |> Seq.map (fun v -> (v.Timestamp.LocalTime, v.Value))
        member x.Tags with get() = proPi.Tags
        member x.RawData(tags: Tag list, ts: TimeSpec) = 
            let dtf, dtt = toTimeRange ts
            tags |> Seq.map (getTagValues dtf dtt String.Empty)
        member x.Data(tags:Tag list, ts:TimeSpec, ?filter:string) = 
            let filter = defaultArg filter String.Empty
            let dtf, dtt = toTimeRange ts
            let getSeries (tag: Tag) = 
                let raw = getTagValues dtf dtt filter tag
                Series(raw |> Seq.map fst, raw |> Seq.map snd)
            let addTagToFrame (frame: Frame<DateTime, string>) (tag: Tag) = 
                frame.AddColumn(tag.Name, getSeries tag)
                frame
            tags |> List.fold addTagToFrame (Frame.ofRows [])
        
    let Pi = PiServer()
