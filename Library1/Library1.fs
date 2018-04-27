module Mod1

open FSharp.Data.Sql

[<Literal>]
let private connectionString = @"Data Source=203.15.179.12;Integrated Security=False;User ID=ncf;Password=ncf1234;Connect Timeout=30"
type private dbSchema = SqlDataProvider<ConnectionString = connectionString>
let private db = dbSchema.GetDataContext()

