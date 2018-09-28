namespace NCF.Data

open System
open FSharp.Data.Sql
open FSharp.Data.Sql.Common
open System.Linq
open System.ComponentModel
open System.Collections.Generic

module Scada = 
    open Time

    module private Internals =
        let [<Literal>] dbVendor = Common.DatabaseProviderTypes.MSSQLSERVER
        let [<Literal>] schemaConnString = @"Data Source=203.15.179.12;Initial Catalog=CitectAlarms; Persist Security Info=True;User ID=ncfUser;Password=ncfPassword"
        let [<Literal>] useOptTypes  = false
        let [<Literal>] contextSchemaPath = @"C:\Root\Project\NCF\NCF.Data\alarm.schema"
        let [<Literal>] tableNames = "NCFEvent"
        let connStringFormat = Printf.StringFormat<string->string>(@"Data Source=%s;Initial Catalog=CitectAlarms; Persist Security Info=True;User ID=ncfUser;Password=ncfPassword")
        type internal Schema = SqlDataProvider<dbVendor, schemaConnString, ContextSchemaPath = contextSchemaPath>
        //type private Schema = SqlDataProvider<dbVendor, connString, UseOptionTypes= useOptTypes, ContextSchemaPath = contextSchemaPath, TableNames = tableNames>
        type AlarmEntity = Schema.dataContext.``dbo.NCFEventEntity``
        type AlarmQuery = IQueryable<AlarmEntity>   
        type internal AlarmServer(server: string) =
            let db = Schema.GetDataContext(sprintf connStringFormat server)
            let tables = db.Dbo
            member x.Alarms = tables.NcfEvent
        let internal alarms = AlarmServer("203.15.179.12") 
        let internal filterFrom (dt: DateTime) (q: AlarmQuery) = query {for a in q do where (a.TimeOn >= dt); select a}
        let internal filterTo (dt: DateTime) (q: AlarmQuery) = query {for a in q do where (a.TimeOn <= dt); select a}
        let internal filterName (name: string) (q: AlarmQuery) = query {for a in q do where (a.Name.Contains(name)); select a}
        let internal filterTimeRange (tr: TimeRange) (q: AlarmQuery) = let dtFrom, dtTo = tr in query {for a in q do where (a.TimeOn >= dtFrom && a.TimeOn <= dtTo); select a}
        let internal filterTimeSpec (ts: TimeSpec) (q: AlarmQuery) = filterTimeRange (toTimeRange ts) q
        let filter q f = 
            match box f with
            | :? TimeSpec as ts -> filterTimeSpec ts q
            | :? string as name -> filterName name q
            | _ -> q

    open Internals

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    type Alarm = {
        Name: string
        [<MappedColumn("TimeOn")>] Start: DateTime
        [<MappedColumn("TimeOff")>] End: DateTime
    } with
        override this.ToString() = sprintf "[%A .. %A] -- %A" this.Start this.End this.Name 
        static member internal FromEntity(entity: SqlEntity) = entity.MapTo<Alarm>()

    //type Alarms private (?q:AlarmQuery) = 
    //    let query = defaultArg q (alarms.Alarms :> AlarmQuery)
    //    let filter selector = Alarms(filter query selector)
    //    interface IQueryable<AlarmEntity> with
    //        member this.ElementType: Type = query.ElementType
    //        member this.Expression: Expressions.Expression = query.Expression
    //        member this.GetEnumerator(): Collections.IEnumerator = query.GetEnumerator() :> Collections.IEnumerator
    //        member this.GetEnumerator(): IEnumerator<AlarmEntity> = query.GetEnumerator()
    //        member this.Provider: IQueryProvider = query.Provider

        //member this.Query = query
        //member this.Seq = this.Query |> Seq.map Alarm.FromEntity
        //member this.Array = Array.ofSeq this.Seq
        //member this.Item(i: int) = this.Array.[i]
        //member this.Filter(timeSpec:TimeSpec) = filter timeSpec
        //member this.Filter(name:string) = filter name
        //interface IQueriable<AlarmEntity> with
        //    member this.Query: IQueryable<AlarmEntity> = query
        //interface IReifyable<AlarmEntity, Alarm> with
        //    member this.AsObjArray = this.Array |> Array.map box
        //    member this.Unbox(boxed: obj[]) = boxed |> Seq.cast<Alarm> |> Seq.toArray
        //interface IPrintable with
        //    member this.Print() = this.Array |> Array.iteri (fun i d -> printfn "[%O] %O" i d)
        //static member (.*) (d: Alarms, f) = filter d.Query f
        //static member All = Alarms()
        //new(timeSpec:TimeSpec) = Alarms(filter alarms.Alarms timeSpec)
        //new(name:string) = Alarms(filter alarms.Alarms name)
        //interface IEnumerable<Alarm> with
        //    member this.GetEnumerator(): IEnumerator<Alarm> = this.Seq.GetEnumerator()
        //    member this.GetEnumerator(): Collections.IEnumerator = (this :> IEnumerable<Alarm>).GetEnumerator() :> Collections.IEnumerator


    //type Alarms private (?q:AlarmQuery)  = 
    //    let query = defaultArg q (alarms.Alarms :> AlarmQuery)
    //    let filter selector = Alarms(filter query selector)
    //    member this.Seq = query |> Seq.map Alarm.FromEntity
    //    member this.Array = Array.ofSeq this.Seq
    //    member this.Item(i: int) = this.Array.[i]
    //    member this.Filter(timeSpec:TimeSpec) = filter timeSpec
    //    member this.Filter(name:string) = filter name
    //    interface IQueriable<AlarmEntity> with
    //        member this.Query: IQueryable<AlarmEntity> = query
    //    member this.Print() = query |> Seq.iteri (fun i d -> printfn "[%O] %O" i d)
    //    static member (.*) (d: IQueriable<AlarmEntity>, f) = filter d.Query f
    //    static member All = Alarms()
    //    new(timeSpec:TimeSpec) = Alarms(filter alarms.Alarms timeSpec)
    //    new(name:string) = Alarms(filter alarms.Alarms name)
    //    interface IEnumerable<Alarm> with
    //        member this.GetEnumerator(): IEnumerator<Alarm> = this.Seq.GetEnumerator()
    //        member this.GetEnumerator(): Collections.IEnumerator = (this :> IEnumerable<Alarm>).GetEnumerator() :> Collections.IEnumerator

    //type Alarms private (?q:AlarmQuery) = 
    //    inherit Query<AlarmEntity, Alarm>((defaultArg q (alarms.Alarms :> AlarmQuery)), filter, Alarm.FromEntity)
    //    new(timeSpec:TimeSpec) = Alarms(filter alarms.Alarms timeSpec)
    //    new(name:string) = Alarms(filter alarms.Alarms name)
    //type Alarms = 
    //    inherit Query<AlarmEntity, Alarm>
    //    new(?q:AlarmQuery) = { inherit Query<AlarmEntity, Alarm>((defaultArg q (alarms.Alarms :> AlarmQuery)), filter, Alarm.FromEntity) }
    //    new(timeSpec:TimeSpec) = Alarms(filter alarms.Alarms timeSpec)
    //    new(name:string) = Alarms(filter alarms.Alarms name)
    type Alarms = 
        inherit Query<AlarmEntity, Alarm>
        new(?q:AlarmQuery) = { inherit Query<AlarmEntity, Alarm>((defaultArg q (alarms.Alarms :> AlarmQuery)), filter, Alarm.FromEntity) }
        new(timeSpec:TimeSpec) = Alarms(filter alarms.Alarms timeSpec)
        new(name:string) = Alarms(filter alarms.Alarms name)
    

    //type Alarms1 = 
    //    val private queryable:IQueryable<Alarm>
    //    new(e:IEnumerable<Alarm>) = { queryable = e.AsQueryable() }
    //    new(q:AlarmQuery) = { queryable = q }
    //    interface IQueryable<Alarm> with
    //        member this.ElementType: Type = this.queryable.ElementType
    //        member this.Expression: Expressions.Expression = this.queryable.Expression
    //        member this.GetEnumerator(): Collections.IEnumerator = this.queryable.GetEnumerator() :> Collections.IEnumerator
    //        member this.GetEnumerator(): IEnumerator<Alarm> = this.queryable.GetEnumerator()
    //        member this.Provider: IQueryProvider = this.queryable.Provider
