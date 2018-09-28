namespace NCF.Data

#nowarn "3060"   

open System
open Ocnarf.Ampla
open FSharp.Data.Sql
open FSharp.Data.Sql.Common
open System.Linq
open System.Collections.Generic
open System.ComponentModel

//type private proAmpla = Ocnarf.TypeProviders.AmplaProvider<"172.40.2.10">

module Ampla = 
    open Time

    module private Internals = 
        let [<Literal>] dbVendor = Common.DatabaseProviderTypes.MSSQLSERVER
        let [<Literal>] schemaConnString = @"Data Source=172.40.2.10;Initial Catalog=NCFAmpla; Persist Security Info=True;User ID=ncfUser;Password=ncfPassword"
        let [<Literal>] useOptTypes = false
        let [<Literal>] contextSchemaPath = @"C:\Root\Project\NCF\NCF.Data\ampla.schema"
        let [<Literal>] tableNames = "Downtimes"
        let connStringFormat = Printf.StringFormat<string->string>(@"Data Source=%s;Initial Catalog=NCFAmpla; Persist Security Info=True;User ID=ncfUser;Password=ncfPassword")
        type Schema = SqlDataProvider<dbVendor, schemaConnString, ContextSchemaPath = contextSchemaPath>
        type DowntimeEntity = Schema.dataContext.``dbo.DowntimesEntity``
        type DowntimeQuery = IQueryable<DowntimeEntity>
        type AmplaServer(server: string) =
            //inherit proAmpla()
            let db = Schema.GetDataContext(sprintf connStringFormat server)
            let tables = db.Dbo
            member x.Downtimes = tables.Downtimes
        let ampla = AmplaServer("172.40.2.10")
        let filterFrom (dt: DateTime) (q: DowntimeQuery) = query {for d in q do where (d.StartDateTime >= dt); select d}
        let filterTo (dt: DateTime) (q: DowntimeQuery) = query {for d in q do where (d.StartDateTime <= dt); select d}
        let filterEquip (eq: string) (q: DowntimeQuery) = query {for d in q do where (d.Equipment.Contains(eq)); select d}
        let filterTimeRange (tr: TimeRange) (q: DowntimeQuery) = let dtFrom, dtTo = tr in query {for d in q do where (d.StartDateTime >= dtFrom && d.StartDateTime <= dtTo); select d}
        let filterTimeSpec (ts: TimeSpec) (q: DowntimeQuery) = filterTimeRange (toTimeRange ts) q
        let filter q f = 
            match box f with
            | :? TimeSpec as ts -> filterTimeSpec ts q
            | :? string as equip -> filterEquip equip q
            | _ -> q

    open Internals

    [<EditorBrowsable(EditorBrowsableState.Never)>]
    type Downtime = {
        [<MappedColumn("StartDateTime")>] Start: DateTime
        [<MappedColumn("EndDateTime")>] End: DateTime
        Equipment: string
    } with
        override this.ToString() = sprintf "[%A .. %A] -- %A" this.Start this.End this.Equipment 
        static member internal FromEntity(entity: SqlEntity) = entity.MapTo<Downtime>()

    type Downtimes private (?q:DowntimeQuery) = 
        inherit Query<DowntimeEntity, Downtime>((defaultArg q (ampla.Downtimes :> DowntimeQuery)), filter, Downtime.FromEntity)
        new(timeSpec:TimeSpec) = Downtimes(filter ampla.Downtimes timeSpec)
        new(equip:string) = Downtimes(filter ampla.Downtimes equip)

    //type Downtimes private (?q:DowntimeQuery)  = 
    //    let query = defaultArg q (ampla.Downtimes :> DowntimeQuery)
    //    let filter selector = Downtimes(filter query selector)
    //    //member this.Query = query
    //    member this.Seq = query |> Seq.map Downtime.FromEntity
    //    member this.Array = Array.ofSeq this.Seq
    //    member this.Item(i: int) = this.Seq |> Seq.toList |> List.item i
    //    member this.Filter(timeSpec:TimeSpec) = filter timeSpec
    //    member this.Filter(equip:string) = filter equip
    //    interface IQueriable<DowntimeEntity> with
    //        member this.Query: IQueryable<DowntimeEntity> = query
    //    member this.Print() = query |> Seq.iteri (fun i d -> printfn "[%O] %O" i d)
    //    static member (.*) (d: IQueriable<DowntimeEntity>, f) = filter d.Query f
    //    static member All = Downtimes()
    //    new(timeSpec:TimeSpec) = Downtimes(filter ampla.Downtimes timeSpec)
    //    new(equip:string) = Downtimes(filter ampla.Downtimes equip)
    //    interface IEnumerable<Downtime> with
    //        member this.GetEnumerator(): IEnumerator<Downtime> = this.Seq.GetEnumerator()
    //        member this.GetEnumerator(): Collections.IEnumerator = (this :> IEnumerable<Downtime>).GetEnumerator() :> Collections.IEnumerator

    //module Downtimes = 
    //    let get selector = Downtimes() .* selector


//module Ampla =
//    open System.ComponentModel

//    [<AutoOpen>]
//    module private AmplaDefinitions =
//        let [<Literal>] dbVendor = Common.DatabaseProviderTypes.MSSQLSERVER
//        let [<Literal>] schemaConnString = @"Data Source=172.40.2.10;Initial Catalog=NCFAmpla; Persist Security Info=True;User ID=ncfUser;Password=ncfPassword"
//        let [<Literal>] useOptTypes = false
//        let [<Literal>] contextSchemaPath = @"C:\Root\Project\NCF\NCF.Data\ampla.schema"
//        let [<Literal>] tableNames = "Downtimes"
//        let connStringFormat = Printf.StringFormat<string->string>(@"Data Source=%s;Initial Catalog=NCFAmpla; Persist Security Info=True;User ID=ncfUser;Password=ncfPassword")
//    type private Schema = SqlDataProvider<dbVendor, schemaConnString, ContextSchemaPath = contextSchemaPath>
//    type private DowntimeEntity = Schema.dataContext.``dbo.DowntimesEntity``
//    type private DowntimeQuery = IQueryable<DowntimeEntity>
  
//    type internal AmplaServer(server: string) =
//        //inherit proAmpla()
//        let db = Schema.GetDataContext(sprintf connStringFormat server)
//        let tables = db.Dbo
//        member x.Downtimes = tables.Downtimes

//    let internal ampla = AmplaServer("172.40.2.10")

//    let internal filterFrom (dt: DateTime) (q: DowntimeQuery) = query {for d in q do where (d.StartDateTime >= dt); select d}
//    let internal filterTo (dt: DateTime) (q: DowntimeQuery) = query {for d in q do where (d.StartDateTime <= dt); select d}
//    let internal filterEquip (eq: string) (q: DowntimeQuery) = query {for d in q do where (d.Equipment.Contains(eq)); select d}
//    let internal filterTimeRange (tr: TimeRange) (q: DowntimeQuery) = let dtFrom, dtTo = tr in query {for d in q do where (d.StartDateTime >= dtFrom && d.StartDateTime <= dtTo); select d}
//    let internal filterTimeSpec (ts: TimeSpec) (q: DowntimeQuery) = filterTimeRange (ToTimeRange ts) q
//    let internal filter q f = 
//        match box f with
//        | :? TimeSpec as ts -> filterTimeSpec ts q
//        | :? string as equip -> filterEquip equip q
//        | _ -> q

//    [<EditorBrowsable(EditorBrowsableState.Never)>]
//    type Downtime = {
//        [<MappedColumn("StartDateTime")>] Start: DateTime
//        [<MappedColumn("EndDateTime")>] End: DateTime
//        Equipment: string
//    } with
//        override this.ToString() = sprintf "[%A .. %A] -- %A" this.Start this.End this.Equipment 

//    [<EditorBrowsable(EditorBrowsableState.Never)>]
//    let Downtime (entity: SqlEntity) = entity.MapTo<Downtime>()

//    [<EditorBrowsable(EditorBrowsableState.Never)>]
//    type Downtimes' = Downtimes' of DowntimeQuery with
//        member this.Query = let (Downtimes' q) = this in q
//        member this.Array = this.Query |> Seq.map Downtime |> Seq.toArray
//        member this.Item(i: int) = this.Array.[i]
//        interface IReifyable<DowntimeEntity, Downtime> with
//            member this.AsObjArray = this.Array |> Array.map box
//            member this.Unbox(boxed: obj[]) = boxed |> Seq.cast<Downtime> |> Seq.toArray
//        interface IPrintable with
//            member this.Print() = this.Array |> Array.iteri (fun i d -> printfn "[%O] %O" i d)
//        static member (.*) (Downtimes' q, f) = 
//            match box f with
//            | :? TimeSpec as ts -> filterTimeSpec ts q
//            | :? string as equip -> filterEquip equip q
//            | _ -> q
//            |> Downtimes'
    
//    let Downtimes = Downtimes' ampla.Downtimes 

//    type Downtimes1(?q: DowntimeQuery) = 
//        member this.Query = defaultArg q (ampla.Downtimes :> DowntimeQuery)
//        member this.Array = this.Query |> Seq.map Downtime |> Seq.toArray
//        member this.Item(i: int) = this.Array.[i]
//        interface IReifyable<DowntimeEntity, Downtime> with
//            member this.AsObjArray = this.Array |> Array.map box
//            member this.Unbox(boxed: obj[]) = boxed |> Seq.cast<Downtime> |> Seq.toArray
//        interface IPrintable with
//            member this.Print() = this.Array |> Array.iteri (fun i d -> printfn "[%O] %O" i d)
//        static member (.*) (d: Downtimes1, f) = Downtimes1(filter d.Query f)
//        member this.Filter(f) = this .* f
//     //let Downtimes1 = Downtimes1(ampla.Downtimes)
