#r @"C:\Root\Project\Ocnarf\packages\Deedle.1.2.5\lib\net40\Deedle.dll"
#r @"C:\Root\Project\Ocnarf\packages\XPlot.Plotly.1.4.5\lib\net45\XPlot.Plotly.dll"

open System
open Deedle
open XPlot.Plotly

let dates  = 
  [ DateTime(2013,1,1); 
    DateTime(2013,1,4); 
    DateTime(2013,1,8) ]
let values = 
  [ 10.0; 20.0; 30.0 ]
let first = Series(dates, values)

Scatter(x = first.Keys, y = first.Values) |> Chart.Plot |> Chart.Show
