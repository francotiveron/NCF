#r "System.Messaging"
open System.Messaging

let q = new MessageQueue(@".\private$\NCFCitectAlarmHistory")
let msg = "OPD~FM1_WTP_LL          ~Flow Meter 1                                      ~Low Low Level                                     ~OFF       ~0  ~5  ~16/08/2017~01:57:00 PM~16/08/2017~01:56:09 PM~16/08/2017~01:57:00 PM~16/08/2017~01:56:58 PM"
q.Send(msg)

//let stp = "STOP"
//q.Send(stp)
