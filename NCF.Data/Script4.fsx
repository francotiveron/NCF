#load "NCF.Data.fsx"

[<Measure>] type s
[<Measure>] type min = s*s

let ddd (a:int<'u>) = a / 1<s>

let m1 = 1<min>

let t1 = ddd m1

