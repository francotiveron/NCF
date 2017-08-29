#r @"C:\Root\Project\NCF\NCF.Messenger\bin\Debug\NCF.Messenger.dll"
open NCF.Messenger

let msg = "tag ~ name~ desc~ OFF ~  4/4/2017~ 21:3:18 ~ 4/4/2017~ 21:3:4 ~  4/4/2017~ 21:3:18~~"
Exports.ncfxSendAlarmEvent msg

//let stp = "STOP"
//Exports.ncfxSendAlarmEvent stp
