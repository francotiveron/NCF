module NCF.WinderReporting.Processor

open System.Messaging
open System

[<Literal>]
let WRREventTypeSta = 1uy
[<Literal>]
let private WRRStaQueuePath = @".\private$\NCFWinderStatusReporting"
let private WRRStaQueue = new MessageQueue(WRRStaQueuePath)
WRRStaQueue.Formatter <- new XmlMessageFormatter([|"System.String"|])

[<Literal>]
let WRREventTypeCyc = 2uy
[<Literal>]
let private WRRCycQueuePath = @".\private$\NCFWinderCycleReporting"
let private WRRCycQueue = new MessageQueue(WRRCycQueuePath)
WRRCycQueue.Formatter <- new XmlMessageFormatter([|"System.String"|])

let rec processLoop () : unit = 
    try
        let staMsg = string (WRRStaQueue.Receive(TimeSpan.Zero).Body)
        if staMsg <> null then do DB.saveEvent WRREventTypeSta staMsg
    with | :? MessageQueueException -> ()

    try
        let cycMsg = string (WRRCycQueue.Receive(TimeSpan.Zero).Body)
        if cycMsg <> null then do DB.saveEvent WRREventTypeCyc cycMsg
    with | :? MessageQueueException -> ()

    processLoop()
