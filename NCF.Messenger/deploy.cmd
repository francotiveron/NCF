rem 1 - execute deploy.cmd (this file)
rem 2 - restart alarm servers

net use \\203.15.178.6\C$ /user:NPMPROD.ORG\npm.controladmin <password>
xcopy "C:\Root\Project\NCF\NCF.Messenger\bin\Debug\*.*" \\203.15.178.6\C$\Citect\Bin /Y
net use \\203.15.178.7\C$ /user:NPMPROD.ORG\npm.controladmin <password>
xcopy "C:\Root\Project\NCF\NCF.Messenger\bin\Debug\*.*" \\203.15.178.7\C$\Citect\Bin /Y
net use \\203.15.179.6\C$ /user:NPMPROD.ORG\npm.controladmin <password>
xcopy "C:\Root\Project\NCF\NCF.Messenger\bin\Debug\*.*" \\203.15.179.6\C$\Citect\Bin /Y
net use \\203.15.179.7\C$ /user:NPMPROD.ORG\npm.controladmin <password>
xcopy "C:\Root\Project\NCF\NCF.Messenger\bin\Debug\*.*" \\203.15.179.7\C$\Citect\Bin /Y
net use \\203.15.179.4\C$ /user:NPMPROD.ORG\npm.controladmin <password>
xcopy "C:\Root\Project\NCF\NCF.Messenger\bin\Debug\*.*" \\203.15.179.4\C$\Citect\Bin /Y