module NCF.Messenger.Exports

[<DllExport("NCFx_Send_AlarmEvent")>]
let ncfxSendAlarmEvent(event:string) : int = 
    try 
        Sender.sendMessage event
        0
    with x -> -1
