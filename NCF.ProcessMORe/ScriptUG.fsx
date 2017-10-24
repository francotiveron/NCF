#r @"System.Data.Linq"
#r @"C:\Root\Project\NCF\packages\FSharp.Data.TypeProviders.5.0.0.2\lib\net40\FSharp.Data.TypeProviders.DLL"

open FSharp.Data.TypeProviders
open System.Collections
open System.Linq

type tEqClass = {
    id: int
    name: string
} with override this.ToString() =
        sprintf "%d, %s" this.id this.name

type tEq = {
    id: int
    name: string
    parent: int
    desc: string
    cls: int
    area: int
} with override this.ToString() =
        sprintf "%d, %s, %d, %s, %d, %d" this.id this.name this.parent this.desc this.cls this.area

type tFc = {
    id: int
    name: string
    cat: string
    prob: string
    cause: string
    parent: int
    desc: string
} with override this.ToString() =
        sprintf "%d, %s, %s, %s, %s, %d, %s" this.id this.name this.cat this.prob this.cause this.parent this.desc

type tEqClassFc = {
    cls: int
    fc: int
} with override this.ToString() =
        sprintf "%d, %d" this.cls this.fc

type tList<'a> = {
    list: 'a list
} with override this.ToString() =
        this.list |> List.fold (sprintf "%s%O\r\n") ""

type tModel = {
    eqClasses: tList<tEqClass>
    equipments: tList<tEq>
    categories: tList<string>
    problems: tList<string>
    causes: tList<string>
    failureCodes: tList<tFc>
    eqClassFcs: tList<tEqClassFc>
} with override this.ToString() =
        sprintf 
            "** EQUIPMENT CLASS {id, name} **\r\n%O** EQUIPMENT {id, name, parent, description, class, area}**\r\n%O** FAILURE CODE CATEGORY **\r\n%O** FAILURE CODE PROBLEM **\r\n%O** FAILURE CODE CAUSE **\r\n%O** FAILURE CODE {id, name, category, problem, cause, parent, description}**\r\n%O** EQUIPMENT CLASS FAILURE CODE (class, failure code}**\r\n%O" 
            this.eqClasses this.equipments this.categories this.problems this.causes this.failureCodes this.eqClassFcs       

type dbSchema = SqlDataConnection<"Data Source=203.15.179.32;Initial Catalog=UG_ProcessMORe;Integrated Security=False;Connect Timeout=30;user=downtime;password=downtime;ApplicationIntent=ReadWrite">
let db = dbSchema.GetDataContext()
let model = 
    let equipClasses = 
        query {
            for c in db.TBL_EQUIP_CLASS do
            select {
                id = (int c.CLASS_ID_EQC)
                name = c.EQ_CLASS_EQC
                }
            }
    let equipments = 
        query {
            for e in db.TBL_EQUIPMENT do
            select {
                id = (int e.EQNUM_ID_EQM)
                name = e.EQNUM_EQM
                parent = (if e.PARENT_EQM.HasValue then (int e.PARENT_EQM.Value) else -1)
                desc = e.DESCRIPTION_EQM
                cls = (int e.CLASS_ID_EQM)
                area = (if e.DTM_AREA_ID_EQM.HasValue then e.DTM_AREA_ID_EQM.Value else -1)
                }
            }
    let categories = 
        query {
            for c in db.TBL_DOWNTIME_CATEGORY do
            select c.DT_CATEGORY_CAT
            }
    let problems = 
        query {
            for p in db.TBL_PROBLEMS do
            select p.PROBLEM_PRO
            }
    let causes = 
        query {
            for c in db.TBL_CAUSES do
            select c.CAUSE_CAU
            }
    let failureCodes = 
        query {
            for f in db.TBL_FAILURE_CODES do
            where (not (f.CAUSE_FAC = ""))
            select {
                id = (int f.FAILURE_CODE_ID_FAC)
                name = f.FAILURE_CODE_FAC
                cat = f.DT_CATEGORY_FAC
                prob = f.PROBLEM_FAC
                cause = f.CAUSE_FAC
                parent = (if f.PARENT_FAC.HasValue then (int f.PARENT_FAC.Value) else -1)
                desc = f.DESCRIPTION_FAC
                }
            }
    let eqClassFcs = 
        let fcIdList = failureCodes |> Seq.map (fun fc -> fc.id)
        query {
            for t in db.TBL_EQUIP_CLASS_FAILURE_CODE do
            where (fcIdList.Contains(int t.FAILURE_CODE_ID_EQF))
            select {
                cls = (int t.CLASS_ID_EQF)
                fc = (int t.FAILURE_CODE_ID_EQF)
                }
            }
    {
    eqClasses = {list = equipClasses |> List.ofSeq}
    equipments = {list = equipments |> List.ofSeq}
    categories = {list = categories |> List.ofSeq} 
    problems = {list = problems |> List.ofSeq} 
    causes = {list = causes |> List.ofSeq} 
    failureCodes = {list = failureCodes |> List.ofSeq}
    eqClassFcs = {list = eqClassFcs |> List.ofSeq}
    }

let PrintEqTree() =
    let eqs = Array.ofList model.equipments.list
    let eqGrp = 
        eqs
        |> Array.mapi (fun i eq -> i, eq.id, eq.parent)
        |> Seq.groupBy (fun (_, _, pid) -> pid)
        |> Map.ofSeq

    let rec f2 (m: Map<int, seq<int*int*int>>) (indent:string) (s:string) (i:int, id:int, _) =
        sprintf "%s%s%O\r\n%s" s indent eqs.[i]
            (
            if m.ContainsKey(id) then
                f1 m.[id] (m.Remove(id)) (indent + "-")
            else "")                
    and f1 s m indent =
        s |> Seq.fold (f2 m indent) ""

    f1 eqGrp.[-1] (eqGrp.Remove(-1)) ""
  
let PrintFcTree() =
    let f2 s ((prob:string), (fcCatProb:seq<tFc>)) =
        sprintf "%s-%s\r\n%s" s prob (fcCatProb |> Seq.fold (sprintf "%s--%O\r\n") "")
    let f1 s ((cat:string), (fcCat:seq<tFc>)) =
        sprintf "%s%s\r\n%s" s cat (fcCat |> Seq.groupBy (fun fc -> fc.prob)  |> Seq.fold f2 "")

    (model.failureCodes.list |> Seq.groupBy (fun fc -> fc.cat)) |> Seq.fold f1 ""

let PrintEqFcByEqClass() =
    let eqByEqClass = model.equipments.list |> Seq.groupBy (fun eq -> eq.cls) |> Map.ofSeq
    let fcByEqClass = model.eqClassFcs.list |> Seq.groupBy (fun fc -> fc.cls) |> Map.ofSeq
    let fcMap = model.failureCodes.list |> Seq.map (fun fc -> (fc.id, fc)) |> Map.ofSeq
    let prtEqClassEqs cls = 
        match eqByEqClass.TryFind(cls) with
        | Some eqs -> eqs |> Seq.fold (sprintf "%s-EQ:%O\r\n") ""
        | None -> "0 EQ\r\n"
    let prtEqClassFcs cls = 
        match fcByEqClass.TryFind(cls) with
        | Some fcs -> fcs |> Seq.fold (fun s eqClassFc -> sprintf "%s-FC:%O\r\n" s fcMap.[eqClassFc.fc]) ""
        | None -> "0 FC\r\n"
    let prtEqClass (s:string) (cls:tEqClass) =
        sprintf "%s%O\r\n%s%s" s cls (prtEqClassEqs cls.id) (prtEqClassFcs cls.id)

    model.eqClasses.list |> Seq.fold prtEqClass ""

let printOut file =
    fprintf file "*** PLAIN DATA MODEL ***\r\n%O*** EQUIPMENT HIERARCHY TREE ***\r\n%s*** FAILURE CODE HIERARCHY TREE ***\r\n%s*** EQUIPMENT and FAILURE CODE by EQUIPMENT CLASS***\r\n%s" model (PrintEqTree()) (PrintFcTree()) (PrintEqFcByEqClass())

printOut stdout

//printf "*** PLAIN DATA MODEL ***\r\n%O" model
//printf "*** EQUIPMENT HIERARCHY TREE ***\r\n%s" (PrintEqTree())
//printf "*** FAILURE CODE HIERARCHY TREE ***\r\n%s" (PrintFcTree())
//printf "*** EQUIPMENT and FAILURE CODE by EQUIPMENT CLASS***\r\n%s" (PrintEqFcByEqClass())
