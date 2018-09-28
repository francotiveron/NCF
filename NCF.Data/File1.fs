module File1

//open System
//open System.Collections.Generic

//type TimeRangeList<'e>(getter: DateTime * DateTime -> List<'e>, ?maybe_tFrom: DateTime, ?maybe_tTo: DateTime) as this = 
//    inherit List<'e>()
//    let tTo = defaultArg maybe_tTo DateTime.Now
//    let tFrom = defaultArg maybe_tFrom (tTo.AddDays(-1.0))
//    do this.AddRange(getter(tFrom, tTo))

        
//type TimeRangeList<'e> = 
//    inherit List<'e>
//    val tFrom: DateTime
//    val tTo: DateTime
//    new (getter: DateTime * DateTime -> List<'e>, ?maybe_tFrom: DateTime, ?maybe_tTo: DateTime) = {
//            inherit List<'e>(getter(defaultArg maybe_tTo DateTime.Now, defaultArg maybe_tFrom ((defaultArg maybe_tTo DateTime.Now).AddDays(-1.0))))
//            tTo = defaultArg maybe_tTo DateTime.Now
//            tFrom = defaultArg maybe_tFrom ((defaultArg maybe_tTo DateTime.Now).AddDays(-1.0))
//        }

//open System

//type Alarm = {
//    Name: string
//    Start: DateTime
//    End: DateTime
//}

//type Downtime = {
//    Start: DateTime
//    End: DateTime
//    Equipment: string
//}

//let inline filter (dtFrom: DateTime) (dtTo: DateTime) (e: 'e when 'e: (member Start: DateTime) and 'e: (member End: DateTime)) = 
//    ( ^e: (member Start: DateTime) e) >= dtFrom && ( ^e: (member End: DateTime) e) <= dtTo
