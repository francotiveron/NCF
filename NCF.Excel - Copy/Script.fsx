//#r "bin\Debug\NCF.Excel.dll"
//open NCF.Excel

open System.Text.RegularExpressions

let location = "cmoc/npm/OPD/0"
let ug, opd, winder = true, true, true
Regex.IsMatch(location, sprintf "%s[%s]" @"cmoc/npm/" (match ug, opd, winder with
                                                                            | true, false, false -> "UG"
                                                                            | false, true, false -> "OPD"
                                                                            | false, false, true -> "WINDER"
                                                                            | true, true, false -> "UG|OPD"
                                                                            | true, false, true -> "UG|WINDER"
                                                                            | false, true, true -> "OPD|WINDER"
                                                                            | _ -> "UG|OPD|WINDER")
                                                                            )