type QueryBuilder() =
    member __.For(x, f) = seq { for v in x do yield! f v }//f x 
    //member __.Bind(x, f) = f x
    //member __.Return(x) = x
    //member __.Zero() = 123.f
    member __.Yield(x) = seq { yield x }
    [<CustomOperation("duplo")>]
    member __.Duplo(x, [<ProjectionParameter>] ff) = Seq.map ff x

let ncfqry = QueryBuilder()

let q = ncfqry {
    for a in [3.f; 5.4f] do 
    duplo a
    //return a
}

type SeqBuilder() =
    // Standard definition for 'for' and 'yield' in sequences
    member x.For (source, body) = seq { for v in source do yield! body v }//body source
    member x.Yield item =
      seq { yield item }

    // Define an operation 'select' that performs projection
    [<CustomOperation("select")>]
    member x.Select (source : seq<'T>, [<ProjectionParameter>] f: 'T -> 'R) : seq<'R> =
        Seq.map f source

    // Defines an operation 'reverse' that reverses the sequence    
    //[<CustomOperation("reverse", MaintainsVariableSpace = true)>]
    //member x.Expand (source : seq<'T>) =
    //    List.ofSeq source |> List.rev

let mseq = SeqBuilder()

let q1 = mseq { 
    for i in 1 .. 10 do
    select (i + 100)    
}

type StringIntBuilder() =

    member this.Bind(m, f) = 
        let b,i = System.Int32.TryParse(m)
        match b,i with
        | false,_ -> "error"
        | true,i -> f i

    member this.Return(x) = 
        sprintf "%i" x

let stringint = new StringIntBuilder()

let good = 
    stringint {
        let! i = "42"
        let! j = "43"
        return i+j
        }
printfn "good=%s" good

let bad = 
    stringint {
        let! i = "42"
        let! j = "xxx"
        return i+j
        }
printfn "bad=%s" bad