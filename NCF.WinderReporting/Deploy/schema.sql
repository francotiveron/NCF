USE [CitectAlarms]
GO
/****** Object:  Table [dbo].[NCFCitectAreas]    Script Date: 4/04/2018 9:06:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NCFCitectAreas](
	[name] [varchar](50) NULL,
	[code] [varchar](50) NULL,
	[desc] [varchar](50) NULL,
	[zone] [nchar](10) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[NCFEvent]    Script Date: 4/04/2018 9:06:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NCFEvent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[tag] [nvarchar](50) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[desc] [nvarchar](max) NOT NULL,
	[location] [nvarchar](100) NOT NULL CONSTRAINT [DF_NCFEvent_location]  DEFAULT (N'cmoc/npm'),
	[priority] [tinyint] NOT NULL,
	[timeOn] [datetime] NOT NULL,
	[timeOff] [datetime] NOT NULL,
	[duration] [real] NOT NULL,
 CONSTRAINT [PK_NCFEvent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NCFEventSink]    Script Date: 4/04/2018 9:06:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NCFEventSink](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[tag] [nvarchar](50) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[desc] [nvarchar](max) NOT NULL,
	[location] [nvarchar](100) NOT NULL CONSTRAINT [DF_NCFEventSink_location]  DEFAULT (N'cmoc/npm'),
	[priority] [tinyint] NOT NULL,
	[timeOn] [datetime] NOT NULL,
	[timeOff] [datetime] NOT NULL,
	[duration] [real] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NCFQuarantinedMessage]    Script Date: 4/04/2018 9:06:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NCFQuarantinedMessage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[message] [nvarchar](max) NOT NULL,
	[reason] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_NCFQuarantinedMessage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  StoredProcedure [dbo].[ProcessCitectAlarmMessages]    Script Date: 4/04/2018 9:06:38 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ProcessCitectAlarmMessages]
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [NCF.AlarmHistory].[NCF.AlarmHistory.Exports].[ProcessCitectAlarmMessages]
GO
/****** Object:  StoredProcedure [dbo].[TransferCitectAlarmMessages]    Script Date: 4/04/2018 9:06:38 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[TransferCitectAlarmMessages]

AS
BEGIN
	SET NOCOUNT ON;

	declare @idMax int

	select @idMax = max(id) from NCFEventSink

	begin transaction;
		insert into NCFEvent
			([tag]
			  ,[name]
			  ,[desc]
			  ,[location]
			  ,[priority]
			  ,[timeOn]
			  ,[timeOff]
			  ,[duration])
			select max([tag])
			  ,max([name])
			  ,max([desc])
			  ,max([location])
			  ,max([priority])
			  ,max([timeOn])
			  ,max([timeOff])
			  ,max([duration]) from NCFEventSink t1
			  where t1.id <= @idMax and not exists(select tag, timeon, timeoff from NCFEvent t2 where t1.tag = t2.tag and t1.timeOn = t2.timeOn and t1.timeoff = t2.timeoff)
			  group by tag, timeOn, timeOff


		delete from NCFEventSink where id <= @idMax
	commit;
END

GO
