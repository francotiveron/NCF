USE [CitectAlarms]
GO
/****** Object:  StoredProcedure [dbo].[TransferCitectAlarmMessages]    Script Date: 4/04/2018 8:51:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[TransferCitectAlarmMessages]

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
