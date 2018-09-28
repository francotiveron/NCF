namespace NCF.Data

open NCF.Data.UI
open Deedle
open XPlot.Plotly
open System
open System.Reflection
open System.Collections
open System.Collections.Generic

module IO = 
    //let print (o: IPrintable) = o.Print()
    //let rec print o = 
    //    match box o with
    //    | :? IPrintable as p -> p.Print()
    //    | :? IEnumerable as sq -> sq |> Seq.cast<obj> |> Seq.iter print
    //    | p -> printfn "%A" p
    let rec print o = 
        let mia = o.GetType().FindMembers(MemberTypes.Method, BindingFlags.Public ||| BindingFlags.Instance, MemberFilter(fun mi _ -> mi.Name = "Print"), null)
        match mia with
        | [|mi|] -> let method = mi :?> MethodInfo in method.Invoke(o, null) |> ignore
        | _ ->
            match box o with
            | :? IEnumerable as sq -> sq |> Seq.cast<obj> |> Seq.iter print
            | p -> printfn "%O" p

    //let inline pick (db: IReifyable<_, _>) = 
    //    UI.SelectFromList(db.AsObjArray) |> db.Unbox;
    let inline pick (seq: IEnumerable<'a>) = 
        UI.SelectFromList(seq) |> Seq.cast<'a>


    let chart data = 
        let toTrace (series : ObjectSeries<DateTime>) = 
            Scatter(x = series.Keys, y = series.Values, mode = "lines") :> Trace
        let chrt (frame: Frame<DateTime, string>) = 
            frame.Columns.Values |> Seq.map toTrace |> Chart.Plot
        match box data with
        | :? Frame<DateTime, string> as frame -> frame |> chrt |> Chart.Show
        | :? seq<Frame<DateTime, string>> as frames -> frames |> Seq.map chrt |> Chart.ShowAll
        | _-> failwith "Can't chart this object"

    let excel data = data |> Excel.send