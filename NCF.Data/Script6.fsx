let fizzbuzz n s = 
    let rec findFirst b m s  =
        if m >= s then m
        else findFirst b (m + b) s

    let m3 = findFirst 3 3 s
    let m5 = findFirst 5 5 s
    let m15 = findFirst 15 15 s

    let rec loop n m3 m5 m15 = function
        | k when k > n -> printfn "End"
        | k when k = m15 -> printfn "FizzBuzz"; loop n (m3 + 3) (m5 + 5) (m15 + 15) (k + 1)
        | k when k = m5 -> printfn "Buzz"; loop n m3 (m5 + 5) m15 (k + 1)
        | k when k = m3 -> printfn "Fizz"; loop n (m3 + 3) m5 m15 (k + 1)
        | k -> printfn "%A" k; loop n m3 m5 m15 (k + 1)

    loop n m3 m5 m15 s

fizzbuzz 100 1