rem 1 - execute deploy.cmd (this file)
rem 2 - execute deploy.sql in the target sql server database
net use \\203.15.179.12\C$ /user:NPMPROD.ORG\franco.tiveron.1 <password>
xcopy "C:\Root\Project\NCF\NCF.AlarmHistory\bin\Debug\*.*" \\203.15.179.12\C$\NCF /Y
