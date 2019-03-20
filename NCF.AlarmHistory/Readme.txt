This project builds a .NET DLL containing a CLR stored procedure.
Once deployed to the target SQL server instance, a sql agent job must be manually created to run this SP every minute.
This project uses FSharp.Data.SQLProvider (the original used FSharp.Data.TypeProviders). 
 To be able to register the assembly in the local SQL server, the F# targeted version needs to be downgraded to 3.1; issue raised in SQLProvider repo
 (https://github.com/fsprojects/SQLProvider/issues/541) and StackOverflow (https://stackoverflow.com/questions/50055163/issue-when-registering-into-sql-server-a-sql-clr-assembly-referencing-fsharp-dat)
To force use of specific version of FSharp.Core, explicitly reference the wanted version in Nuget for the project