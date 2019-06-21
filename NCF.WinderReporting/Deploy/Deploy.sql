/* MUST BE RUN IN THE TARGET SQL SERVER INSTANCE, DATABASE CitectAlarms MUST EXIST
   Files must be copied first (deploy.cmd) */

/*Dependencies (once)*/
sp_configure 'clr enabled', 1
go
reconfigure
go
select * from sys.dm_clr_properties
alter database WinderReporting set trustworthy on

use WinderReporting

CREATE ASSEMBLY [FSharp.Core] FROM 'C:\NCF\FSharp.Core.dll' WITH PERMISSION_SET = UNSAFE
GO
--CREATE ASSEMBLY [System.Messaging] FROM 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\System.Messaging.dll' WITH PERMISSION_SET = UNSAFE
CREATE ASSEMBLY [smdiagnostics] FROM 'C:\NCF\smdiagnostics.dll' WITH PERMISSION_SET = UNSAFE
CREATE ASSEMBLY [system.windows.forms] FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\system.windows.forms.dll' WITH PERMISSION_SET = UNSAFE
CREATE ASSEMBLY [system.data.datasetextensions] FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\system.data.datasetextensions.dll' WITH PERMISSION_SET = UNSAFE
CREATE ASSEMBLY [System.Messaging] FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Messaging.dll' WITH PERMISSION_SET = UNSAFE
GO
CREATE ASSEMBLY [System.Data.Linq] FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll' WITH PERMISSION_SET = UNSAFE
GO
CREATE ASSEMBLY [FSharp.Data.SqlProvider] FROM 'C:\NCF\FSharp.Data.SqlProvider.dll' WITH PERMISSION_SET = UNSAFE
GO

/* Package (after any change) */
use WinderReporting

IF  EXISTS (SELECT * FROM sys.procedures procs WHERE procs.name = N'ProcessWinderReportingMessages')
    DROP PROCEDURE ProcessWinderReportingMessages
GO

IF  EXISTS (SELECT * FROM sys.assemblies asms WHERE asms.name = N'NCF.WinderReporting')
    DROP ASSEMBLY [NCF.WinderReporting]
GO

CREATE ASSEMBLY [NCF.WinderReporting] FROM 'C:\NCF\NCF.WinderReporting.dll' WITH PERMISSION_SET = UNSAFE
GO
CREATE PROCEDURE ProcessWinderReportingMessages
AS
EXTERNAL NAME [NCF.WinderReporting].[NCF.WinderReporting.Exports].[ProcessWinderReportingMessages]
GO
