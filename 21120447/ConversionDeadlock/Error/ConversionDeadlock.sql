USE QUANLYPHONGKHAM
GO
/*CONVERSION DEADLOCK*/
/* T1 */

CREATE OR ALTER PROCEDURE USP_UPDATE_SL_TON_THUOC @ID_THUOC NCHAR(5), @TONKHO INT
AS
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
BEGIN TRAN
	IF NOT EXISTS (SELECT * FROM THUOC WHERE ID_THUOC = @ID_THUOC)
	BEGIN
		PRINT N'Thuốc không tồn tại'
		ROLLBACK TRAN
		RETURN 0
	END

	IF @TONKHO < 0
	BEGIN
		PRINT N'Số lượng tồn kho phải dương'
		ROLLBACK TRAN
		RETURN 0
	END
	
	WAITFOR DELAY '00:00:05'
	
	UPDATE THUOC
	SET TONKHO = @TONKHO
	WHERE ID_THUOC = @ID_THUOC
	
COMMIT TRAN
GO

