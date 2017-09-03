
USE [CitectAlarms]
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


CREATE UNIQUE NONCLUSTERED INDEX [IX_NCFEvent_tag_timeon_timeoff] ON [dbo].[NCFEvent]
(
	[tag] ASC,
	[timeOn] ASC,
	[timeOff] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

