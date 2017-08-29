module NCF.Messenger.Sender

open System.Messaging

[<Literal>]
let private alarmHistoryServerIP = "203.15.179.12"
[<Literal>]
let private alarmHistoryQueueName = "NCFCitectAlarmHistory"
let private alarmHistoryQueuePath = sprintf @"FormatName:Direct=TCP:%s\private$\%s" alarmHistoryServerIP alarmHistoryQueueName
let private queue = new MessageQueue(alarmHistoryQueuePath)
queue.DefaultPropertiesToSend.Recoverable <- true

let sendMessage msg = 
    queue.Send(msg)
