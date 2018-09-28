namespace NCF.Data

open System
open Microsoft.FSharp.Data.UnitSystems.SI

module Time =
    type TimeRange = DateTime * DateTime

    type TimeSpec = 
        | Last of TimeSpan
        | Last24H
        | BeforeBy of DateTime * TimeSpan
        | Before of DateTime
        | AroundBy of DateTime * TimeSpan
        | Around of DateTime
        | AfterBy of DateTime * TimeSpan
        | After of DateTime

    let rec toTimeRange (t: TimeSpec) : DateTime * DateTime = 
        match t with
        | Last tSpan -> toTimeRange (BeforeBy (DateTime.Now, tSpan))
        | Last24H -> toTimeRange (Last (TimeSpan.FromHours(24.0)))
        | BeforeBy (t, tSpan) -> t.Add(-tSpan), t
        | Before t -> toTimeRange (BeforeBy (t, TimeSpan.FromMinutes(30.0)))
        | AroundBy (t, tSpan) -> t.Add(-tSpan), t.Add(tSpan)  
        | Around t -> toTimeRange (AroundBy (t, TimeSpan.FromMinutes(15.0)))
        | AfterBy (t, tSpan) -> t, t.Add(tSpan)
        | After t -> toTimeRange (AfterBy (t, TimeSpan.FromMinutes(30.0)))

