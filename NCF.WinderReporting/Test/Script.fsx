#r @"C:\Root\Project\NCF\NCF.WinderReporting\bin\Debug\NCF.WinderReporting.dll"
open NCF.WinderReporting

let msg = "DT=23 aug 2019 23:12:65.342,ST=1"
DB.saveEvent 1uy msg
