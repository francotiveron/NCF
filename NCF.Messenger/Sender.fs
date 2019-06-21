module NCF.Messenger.Sender

open System.Messaging

[<Literal>]
let private alarmHistoryServerIP = "203.15.179.12"
//let private alarmHistoryServerIP = "127.0.0.1"
[<Literal>]
let private alarmHistoryQueueName = "NCFCitectAlarmHistory"
let private alarmHistoryQueuePath = sprintf @"FormatName:Direct=TCP:%s\private$\%s" alarmHistoryServerIP alarmHistoryQueueName

[<Literal>]
let private WRRStaQueueName = "NCFWinderStatusReporting"
let private WRRStaQueuePath = sprintf @"FormatName:Direct=TCP:%s\private$\%s" alarmHistoryServerIP WRRStaQueueName

[<Literal>]
let private WRRCycQueueName = "NCFWinderCycleReporting"
let private WRRCycQueuePath = sprintf @"FormatName:Direct=TCP:%s\private$\%s" alarmHistoryServerIP WRRCycQueueName

let private queue = new MessageQueue(alarmHistoryQueuePath)
queue.DefaultPropertiesToSend.Recoverable <- true

let private WRRStaQueue = new MessageQueue(WRRStaQueuePath)
WRRStaQueue.DefaultPropertiesToSend.Recoverable <- true

let private WRRCycQueue = new MessageQueue(WRRCycQueuePath)
WRRCycQueue.DefaultPropertiesToSend.Recoverable <- true

let sendMessage msg = 
    queue.Send(msg)

let sendWRRStaMessage msg = 
    WRRStaQueue.Send(msg)

let sendWRRCycMessage msg = 
    WRRCycQueue.Send(msg)
