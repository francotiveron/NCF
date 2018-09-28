#load "NCF.Data.fsx"
open NCF.Data.Pi
open Ocnarf.Pi
open System.Collections.Generic
open Microsoft.FSharp.Quotations

type QueryBuilder() =
    member __.For(x: seq<'T>, f: 'T -> seq<'R>) : seq<'R> = Seq.collect f x
    member __.Yield(t: 'T) : seq<'T> = seq { yield t }
    [<CustomOperation("op1", MaintainsVariableSpace = true)>]
    member __.Op1(x: seq<'T>, [<ProjectionParameter>] f: 'T -> bool): seq<'T> = Seq.filter f x
    [<CustomOperation("op2")>]
    member __.Op2(x: seq<'T>, [<ProjectionParameter>] f: 'T -> 'R): seq<'R> = Seq.map f x
    [<CustomOperation("op3")>]
    member __.Op3(x: seq<'T>, n: int): seq<'T> = Seq.take n x
    //member __.Quote(e:Expr<_>) = e

let ncfqry = QueryBuilder()

let q = ncfqry {
    for tag in pi.Tags do
    op1 (tag.Name.Contains("Pressure"))
    op2 tag.Name
    //op3 5
    } 

printf "%A" (q |> List.ofSeq)
