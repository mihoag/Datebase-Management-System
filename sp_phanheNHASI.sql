use QUANLYPHONGKHAM
go


-- (LICH NHA SI)
-- sp_LICHNHASI: lay lich cua 1 nha si
-- input: @id_ns NCHAR(5), @ngayhen DATE
-- output: list data
IF OBJECT_ID('dbo.sp_LICHNHASI', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.sp_LICHNHASI
END
GO

CREATE PROCEDURE sp_LICHNHASI 
    @id_ns NCHAR(5),
	@ngayhen DATE
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED

    BEGIN TRANSACTION
    IF @ngayhen is NULL
        BEGIN
            SELECT * FROM LICH_NHA_SI WHERE ID_NS = @id_ns
        END
    ELSE
        BEGIN
            SELECT * FROM LICH_NHA_SI WHERE ID_NS = @id_ns AND NGAYHEN = @ngayhen
        END

    COMMIT TRANSACTION
END
GO


-- (LICH NHA SI)
-- sp_DATLICHBAN: them lich ban cua nha si
-- input: @id_ns NCHAR(5), @ngayhen DATE, @gio_bd TIME, @gio_kt TIME, @chitiet NVARCHAR(50)
-- output: 
IF OBJECT_ID('dbo.sp_DATLICHBAN', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.sp_DATLICHBAN
END
GO

CREATE PROCEDURE sp_DATLICHBAN
    @id_ns NCHAR(5),
	@ngayhen DATE,
	@gio_bd TIME,
	@gio_kt TIME,
	@chitiet NVARCHAR(50)
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED

    BEGIN TRANSACTION
    -- Kiem tra ID_NS co ton tai khong
     if not exists(select * from NHA_SI where ID_NS = @id_ns)
      begin
        print N'ID nha sĩ không tồn tại'
	    ROLLBACK TRAN 
	    return 1
     end
	 -- Kiểm tra ngày hẹn có hợp lệ hay không
	 if @ngayhen is null
       begin
         print N'Ngày hẹn phải khác NULL'
	     ROLLBACK TRAN 
	     return 1
      end
	  -- Kiem tra gio bat dau, va gio ket thuc co hop le khong
      if @gio_bd > @gio_kt
        begin
         print N'Thời gian hẹn không hợp lệ'
	     ROLLBACK TRAN 
	     return 1
      end
	  -- Kiểm tra xem khung giờ hẹn có bị kẹt bởi lịch khác của nha sĩ hay khôgn
       if exists(select * from LICH_NHA_SI where NGAYHEN = @ngayhen and ((GIO_BD between @gio_bd and @gio_kt) or (GIO_KT between @gio_bd and @gio_kt)) and ID_NS = @id_ns)
	    begin
         print N'khung giờ hẹn có bị kẹt bởi lịch khác'
	     ROLLBACK TRAN 
	     return 1
	   end

	   -- cap nhat lai lich hen cua nha si
		insert into LICH_NHA_SI (ID_NS, NGAYHEN, GIO_BD, GIO_KT, CHITIET) values (@id_ns, @ngayhen, @gio_bd, @gio_kt, @chitiet)
    COMMIT TRANSACTION
END
GO

-- (DON THUOC)
-- sp_KEDON: them don thuoc
-- input: @ID_THUOC NCHAR(5), @ID_BN NCHAR(5), @NGAYKHAM DATE, @SOLUONG INT
-- output:
IF OBJECT_ID('dbo.sp_KEDON', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.sp_KEDON
END
GO

CREATE PROC sp_KEDON @ID_THUOC NCHAR(5), @ID_BN NCHAR(5), @NGAYKHAM DATE, @SOLUONG INT
AS
SET TRAN ISOLATION LEVEL READ COMMITTED
BEGIN TRAN
   BEGIN TRY
    -- KIỂM TRA ID_THUỐC CÓ TỒN TẠI KHÔNG
   IF(NOT EXISTS(SELECT * FROM THUOC WHERE ID_THUOC = @ID_THUOC))
   BEGIN
       PRINT N'MÃ THUỐC KHÔNG TỒN TẠI'
	   ROLLBACK TRAN
	   RETURN 1
   END

    -- KIỂM TRA ID_BỆNH NHÂN CÓ TỒN TẠI KHÔNG
   IF(NOT EXISTS(SELECT * FROM CHI_TIET_HS WHERE ID_BN = @ID_BN))
   BEGIN
       PRINT N'MÃ BỆNH NHÂN KHÔNG TỒN TẠI'
	   ROLLBACK TRAN
	   RETURN 1
   END

   -- KIỂM TRA NGÀY CÓ NULL HAY KHÔNG
   IF(@NGAYKHAM IS NULL)
   BEGIN
	   PRINT N'NGÀY KHÁM KHÔNG ĐƯỢC NULL'
	   ROLLBACK TRAN
	   RETURN 1
   END

   -- KIỂM TRA NGÀY KHÁM CÓ TRONG CHI_TIET_HS HAY KHÔNG
   IF(NOT EXISTS(SELECT * FROM CHI_TIET_HS WHERE ID_BN = @ID_BN AND NGAYKHAM = @NGAYKHAM))
   BEGIN
       PRINT N'KHÔNG TỒN TẠI MÃ BỆNH NHÂN VÀ NGÀY KHÁM TƯƠNG ỨNG TRONG CHI_TIET_HS'
	   ROLLBACK TRAN
	   RETURN 1
   END

   --KIỂM TRA ĐƠN THUỐC CÓ BỊ TRÙNG HAY KHÔNG
   IF(EXISTS(SELECT * FROM DON_THUOC WHERE ID_BN = @ID_BN AND NGAYKHAM = @NGAYKHAM AND @ID_THUOC = ID_THUOC))
   BEGIN
       PRINT N'ĐƠN THUỐC ĐÃ TỒN TẠI'
	   ROLLBACK TRAN
	   RETURN 1
   END

   -- 
   DECLARE @SLT INT
    -- KIỂM TRA SỐ LƯỢNG TỒN CÓ ĐỦ HAY KHÔNG
   SET @SLT = (SELECT TONKHO  FROM THUOC WHERE ID_THUOC = @ID_THUOC)
   IF(@SLT - @SOLUONG < 0)
   BEGIN
       PRINT N'SỐ LƯỢNG TỒN CỦA THUỐC KHÔNG ĐỦ ĐỂ KÊ ĐƠN'
	   ROLLBACK TRAN
	   RETURN 1
   END

	-- UPDATE LẠI SỐ LƯỢNG TỒN CỦA THUỐC
	UPDATE THUOC
	SET TONKHO = @SLT - @SOLUONG
	WHERE ID_THUOC = @ID_THUOC;


   INSERT INTO DON_THUOC(ID_THUOC, ID_BN, NGAYKHAM, SOLUONG) VALUES (@ID_THUOC, @ID_BN, @NGAYKHAM, @SOLUONG)
   
   END TRY

   BEGIN CATCH
		PRINT ERROR_MESSAGE()
		ROLLBACK TRAN
		RETURN 1	
   END CATCH

COMMIT TRAN
RETURN 0
GO

-- (THUOC)
-- sp_DSTHUOC: lay danh muc thuoc
-- input: 
-- output: list data
IF OBJECT_ID('dbo.sp_DSTHUOC', 'P') IS NOT NULL
BEGIN
    DROP PROCEDURE dbo.sp_DSTHUOC
END
GO

CREATE PROC sp_DSTHUOC
AS
SET TRAN ISOLATION LEVEL READ COMMITTED
BEGIN TRAN
	BEGIN TRY

	SELECT * FROM THUOC

	END TRY

	BEGIN CATCH
		PRINT ERROR_MESSAGE()
		ROLLBACK TRAN
		RETURN 1	
   END CATCH
COMMIT TRAN
RETURN 0
GO
