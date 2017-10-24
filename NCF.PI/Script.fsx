// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.
#load "Library1.fs"
#r "OSIsoft.AFSDK"
open OSIsoft.AF

let pis = new PISystems()
let pi = pis.DefaultPISystem
pi.ConnectWithPrompt(null)
let db = pi.Databases.DefaultDatabase;
let dbs = pi.Databases