module NCF.AlarmHistory.CitectAlarmEvent

open System
open System.Security.Policy

//Citect EventFmt = zone~{TAG,20}~{NAME,50}~{DESC,50}~{STATE,10}~{AREA,3}~{PRIORITY,3}~{DATEEXT,10}~{TIME,11}~{ONDATEEXT,10}~{ONTIME,11}~{OFFDATEEXT,10}~{OFFTIME,11}~{ACKDATEEXT,10}~{ACKTIME,11}
type State = On | NotOn
type CitectAlarmEvent = {
    zone : string
    tag : string
    name : string
    desc : string
    state : State
    area : int16
    priority : byte
    time : DateTime
    //timeOn : DateTime
    timeOn : DateTime option
    timeOff : DateTime option
    timeAck : DateTime option
    }

[<Literal>] 
let private separator = '~'

let private split (msg : string) = msg.Split(separator)

//tag, event time and timeOn must exist for a message to be valid
let msg2record (citectMessage : string) : Result<CitectAlarmEvent, string> = 
    let mutable t = DateTime.Now
    let mutable tOn = DateTime.Now
    let parsedt t d = let r, dt = DateTime.TryParse(t + " " + d) in if r then Some dt else None
    
    match split (citectMessage.ToUpper()) |> Array.map (fun s -> s.Trim()) with
    | [|zone; tag; name; desc; state; area; priority; date; time; dateOn; timeOn; dateOff; timeOff; dateAck; timeAck|] when 
            not (String.IsNullOrWhiteSpace(tag))
            && (match (parsedt time date) with | Some dt -> t <- dt; true | None -> false)
            //&& (match (parsedt timeOn dateOn) with | Some dt -> tOn <- dt; true | None -> false)
        -> Ok {
            zone= zone
            tag = tag
            name = name
            desc = desc
            state = match state with | "ON" -> On | _ -> NotOn
            area = System.Int16.Parse(area);
            priority = System.Byte.Parse(priority)
            time = t
            //timeOn = tOn
            timeOn = parsedt timeOn dateOn
            timeOff = parsedt timeOff dateOff
            timeAck = parsedt timeAck dateAck}
    | _ -> Error "Invalid Format"
