#load "NCF.Data.fsx"

open NCF.Data

let tags = [Pi.Tags.SCS_122CV004_AT_P80; Pi.Tags.SCS_123CV006_AT_P80]

let chartP80BeforeLas24HWinderDowntimes() =
    let getData (d: Downtime) = Pi.Data(tags, (Before d.Start))

    let b1 = Downtimes .* Last24H .* "Winder"
    let bdata = b1.Array |> Seq.map getData
    bdata |> chart

chartP80BeforeLas24HWinderDowntimes();