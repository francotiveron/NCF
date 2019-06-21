USE [CitectAlarms]
GO
/****** Object:  StoredProcedure [dbo].[TransferCitectAlarmMessages]    Script Date: 18/06/2019 2:05:53 PM ******/
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
	declare @dt datetime2, @st bit
	declare @cyStart datetime2, @cyEnd datetime2, @cyPayload int, @cyLoadMs int, @cyUnloadMs int, @cyFlags smallint


	select @idMax = max(id) from NCFEventSink

	declare @body as nvarchar(255);
 
	declare @stCursor as cursor
	set @stCursor = cursor for
		select [body] from NCFEventSink sink
		where sink.id <= @idMax and sink.[type] = 1
 
	declare @cyCursor as cursor
	set @cyCursor = cursor for
		select [body] from NCFEventSink sink
		where sink.id <= @idMax and sink.[type] = 2

 	begin transaction;

		open @stCursor;
		fetch next from @stCursor into @body;
 
		while @@FETCH_STATUS = 0
		begin
			select @dt=dt, @st=st from [dbo].[WRRParseStaMsg](@body)

			if exists(select id from NcfWrrStates where id = @dt)
			begin
				update NcfWrrStates
				set [status] = @st
				where id = @dt
			end
			else
			begin
				insert into NcfWrrStates([id], [status])
				values (@dt, @st)
			end
			fetch next from @stCursor into @body;
		end
 
		close @stCursor;
		deallocate @stCursor;

		open @cyCursor;
		fetch next from @cyCursor into @body;
 
		while @@FETCH_STATUS = 0
		begin
			select @cyStart=[start], @cyEnd=[end], @cyPayload=payload, @cyLoadMs=loadMs, @cyUnloadMs=unloadMs, @cyFlags=flags
			from [dbo].[WRRParseCycMsg](@body)

			if exists(select id from NcfWrrCycles where [start] = @cyStart)
			begin
				update NcfWrrCycles
				set [end] = @cyEnd, payload = @cyPayload, load_ms = @cyLoadMs, unload_ms = @cyUnloadMs, flags = @cyFlags
				where [start] = @cyStart
			end
			else
			begin
				insert into NcfWrrStates([start], [end], payload, load_ms, unload_ms, flags)
				values (@cyStart, @cyEnd, @cyPayload, @cyLoadMs, @cyUnloadMs, @cyFlags)
			end
			fetch next from @cyCursor into @body;
		end
 
		close @cyCursor;
		deallocate @cyCursor;

		delete from NCFEventSink where id <= @idMax

	commit;
END
