﻿USE QUANLYPHONGKHAM
GO

/* UNREPEATABLE READ */
CREATE OR ALTER PROCEDURE USP_THONG_TIN_THUOC_FIX @ID_THUOC NCHAR(5)
AS
SET TRAN ISOLATION LEVEL REPEATABLE READ
BEGIN TRAN
	IF NOT EXISTS(SELECT * FROM THUOC WHERE ID_THUOC= @ID_THUOC)
	BEGIN
		PRINT N'Mã thuốc không tồn tại'
        ROLLBACK TRAN
	END

	DECLARE @TEN NVARCHAR(50)
	SELECT @TEN = TENTHUOC FROM THUOC WHERE ID_THUOC = @ID_THUOC

	WAITFOR DELAY '00:00:05'

	DECLARE @TEN2 NVARCHAR(50)

	SELECT @TEN2=TENTHUOC
	FROM THUOC WHERE ID_THUOC = @ID_THUOC
	SELECT * FROM THUOC WHERE ID_THUOC =@ID_THUOC

	IF (@TEN != @TEN2)
	BEGIN
		RAISERROR(N'Tên thuốc hai lần đọc khác nhau',16, 1)
	END
COMMIT TRAN
GO
---------------------------------------------------------------------------
/* UNREPEATABLE READ */
CREATE OR ALTER PROCEDURE USP_UPDATE_TEN_THUOC_FIX @ID_THUOC NCHAR(5), @TENTHUOC NVARCHAR(50)
AS
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
BEGIN TRAN
		IF NOT EXISTS(SELECT * FROM THUOC WHERE ID_THUOC= @ID_THUOC)
		BEGIN
			PRINT N'Không tồn tại thuốc này'
			ROLLBACK TRAN
		END

		UPDATE THUOC
		SET TENTHUOC = @TENTHUOC
		WHERE ID_THUOC = @ID_THUOC
COMMIT TRAN
GO