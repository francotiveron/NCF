module File2

type T1() =
    member x.Start with get() = System.DateTime.Now

type T2() =
    member x.Start with get() = System.DateTime.Now.AddDays(1.0)

let inline f1< ^e when ^e: (member Start: System.DateTime)> (e: 'e) : System.DateTime =
    (^e : (member Start: System.DateTime) e)

let t1Start = f1 (T1())
let t2Start = f1 (T2())


open System.Runtime.InteropServices

type Default6 = class end
type Default5 = class inherit Default6 end
type Default4 = class inherit Default5 end
type Default3 = class inherit Default4 end
type Default2 = class inherit Default3 end
type Default1 = class inherit Default2 end

type DivRem =
    inherit Default1
    static member inline DivRem (x:^t when ^t: null and ^t: struct, y:^t, thisClass:DivRem) = (x, y)
    static member inline DivRem (D:'T, d:'T, [<Optional>]impl:Default1) = let q = D / d in q,  D - q * d
    static member inline DivRem (D:'T, d:'T, [<Optional>]impl:DivRem) =
        let mutable r = Unchecked.defaultof<'T>
        (^T: (static member DivRem: _ * _ -> _ -> _) (D, d, &r)), r
    static member inline Invoke (D:'T) (d:'T) :'T*'T =
        let inline call_3 (a:^a, b:^b, c:^c) = ((^a or ^b or ^c) : (static member DivRem: _*_*_ -> _) b, c, a)
        let inline call (a:'a, b:'b, c:'c) = call_3 (a, b, c)
        call (Unchecked.defaultof<DivRem>, D, d)    

let inline divRem (D:'T) (d:'T) :'T*'T = DivRem.Invoke D d
//let inline divRem (D:^T) (d:^T): ^T * ^T = let q = D / d in q,  D - q * d

let qr0  = divRem 7  3
let qr1  = divRem 7I 3I
let qr2  = divRem 7. 3.

