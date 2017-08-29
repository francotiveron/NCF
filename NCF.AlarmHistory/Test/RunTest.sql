USE [CitectAlarms]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[ProcessCitectAlarmMessages]

SELECT	@return_value as 'Return Value'

GO
