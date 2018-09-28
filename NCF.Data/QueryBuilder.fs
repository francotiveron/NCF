namespace NCF.Data

open Microsoft.FSharp.Quotations
open FSharp.Data.Sql.Common
open System
open System.Collections
open System.Collections.Generic
open System.Linq
open System.ComponentModel
open System.Linq.Expressions

type DataSource<'Entity, 'User> = 
    | Entity of IQueryable<'Entity>
    | User of IEnumerable<'User>
type IQuery<'a> = abstract member Query : IQueryable<'a>
type Query<'Entity, 'User> internal (query:IQueryable<'Entity>, filterDelegate:IQueryable<'Entity> -> _ -> IQueryable<'Entity>, fromEntityConstructor:'Entity -> 'User)  = 
    let filter selector = Query(filterDelegate query selector, filterDelegate, fromEntityConstructor)
    member private this._Filter = filter
    member this.Seq = query |> Seq.map fromEntityConstructor
    member this.Array = Array.ofSeq this.Seq
    member this.Item(i: int) = this.Array.[i]
    member this.Filter(timeSpec:Time.TimeSpec) = filter (box timeSpec)
    member this.Filter(name:string) = filter (box name)
    member internal this.Query = query
    member this.Print() = this.Seq |> Seq.iteri (fun i d -> printfn "[%O] %O" i d)
    static member (.*) (q: Query<_,_>, selector) = q._Filter(selector)
    interface IEnumerable<'User> with
        member this.GetEnumerator(): IEnumerator<'User> = this.Seq.GetEnumerator()
        member this.GetEnumerator(): Collections.IEnumerator = (this :> IEnumerable<'User>).GetEnumerator() :> Collections.IEnumerator
    interface IQuery<'Entity> with
        member this.Query: IQueryable<'Entity> = query

[<EditorBrowsable(EditorBrowsableState.Never)>]
module NCFData_QueryBuilder = 
    open System.Linq

    let rec private replace expr = 
      match expr with 
      | Patterns.Call(Some inst, mi, [arg]) when mi.Name = "Source" -> 
          let t, x = 
            match arg with
            | Patterns.PropertyGet(_, tt, _) -> tt.PropertyType, arg
            | Patterns.Coerce (exp, _) ->
                match exp with
                | Patterns.NewObject(tt, _) -> tt.DeclaringType, exp
                | Patterns.PropertyGet(_, ds, _) -> ds.PropertyType, exp
                | _ -> Unchecked.defaultof<Type>, exp
            | _ -> Unchecked.defaultof<Type>, arg
          let sourceMethod =
            typeof<Linq.QueryBuilder>.GetMethods() 
            |> Seq.filter (fun mi -> 
                 mi.Name = "Source" && 
                 mi.GetParameters().[0].ParameterType.Name = "IQueryable`1")
            |> Seq.head
          let sourceMethod = 
            sourceMethod.MakeGenericMethod 
              [| typeof<SqlEntity>; typeof<System.Linq.IQueryable> |]
              //[| typeof<DowntimeEntity>; typeof<System.Linq.IQueryable> |]
          //let queryProp = typeof<T2>.GetProperty("Query")
          let queryProp = t.GetProperty("Query")
          //Expr.Call(inst, sourceMethod, [ Expr.PropertyGet(arg, queryProp) ])
          Expr.Call(inst, sourceMethod, [ Expr.PropertyGet(x, queryProp) ])
      | ExprShape.ShapeCombination(o, args) -> 
          ExprShape.RebuildShapeCombination(o, List.map replace args)
      | ExprShape.ShapeLambda(a, b) -> Expr.Lambda(a, replace b)
      | ExprShape.ShapeVar(v) -> Expr.Var(v)

    type NCFData_QueryBuilder() = 
        inherit Linq.QueryBuilder()
        member __.Source (source: IQuery<_>) = base.Source(source.Query)
        member __.Run(e:Expr<Linq.QuerySource<'a, System.Linq.IQueryable>>) = 
            let e = Expr.Cast<Linq.QuerySource<'a, System.Linq.IQueryable>>(replace e.Raw)
            base.Run(e) 

    let ncdQuery = new NCFData_QueryBuilder()