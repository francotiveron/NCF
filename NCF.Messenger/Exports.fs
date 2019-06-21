module NCF.Messenger.Exports

open System

[<Literal>]
let private dtFormat = "dd MMM yyyy HH:mm:ss.fff"

[<DllExport("NCFx_Send_AlarmEvent")>]
let ncfxSendAlarmEvent(event:string) : int = 
    try 
        Sender.sendMessage event
        0
    with x -> -1

[<DllExport("NCFx_Send_WRRStaEvent")>]
let ncfxSendWRRStaEvent(sec1980:uint32, mss:uint16) : int = 
    try 
        let ms = mss / 2us;
        let status = mss - 2us * ms;
        let origin = new DateTime(1980, 1, 1, 0, 0, 0)
        let dt = origin.AddSeconds((float)sec1980).AddMilliseconds((float)ms)
        let msg = sprintf "DT=%s,ST=%d" (dt.ToString(dtFormat)) status
        Sender.sendWRRStaMessage msg
        0
    with x -> -1
    
[<DllExport("NCFx_Send_WRRCycEvent")>]
let ncfxSendWRRCycEvent(
                        secStart:uint32, 
                        msStart:uint16, 
                        secEnd:uint32,
                        msEnd:uint16,
                        payload:uint32,
                        msLoad:uint32,
                        msUnload:uint32,
                        flags:int16
                        ) : int = 
    try 
        let origin = new DateTime(1980, 1, 1, 0, 0, 0)
        let dtStart = origin.AddSeconds((float)secStart).AddMilliseconds((float)msStart)
        let dtEnd = origin.AddSeconds((float)secEnd).AddMilliseconds((float)msEnd)
        let msg = sprintf "START=%s,END=%s,PAYLOAD=%d,LOADMS=%d,UNLOADMS=%d,FLAGS=%d" 
                    (dtStart.ToString(dtFormat)) 
                    (dtEnd.ToString(dtFormat)) 
                    payload
                    msLoad
                    msUnload
                    flags
        Sender.sendWRRCycMessage msg
        0
    with x -> -1
    
