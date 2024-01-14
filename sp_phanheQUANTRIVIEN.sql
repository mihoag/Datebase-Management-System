use QUANLYPHONGKHAM
go

-- lay chi tiet thong tin thuoc
CREATE OR ALTER PROCEDURE sp_chitietThuoc @ID_THUOC char(10)
AS
BEGIN
	if (not exists (SELECT * FROM THUOC WHERE ID_THUOC = @ID_THUOC))
	begin
		raiserror(N'Không tồn tại thuốc có id này', 16, 1)
		return 0
	end

	SELECT * FROM THUOC WHERE ID_THUOC = @ID_THUOC

END
GO

exec sp_chitietThuoc 'T0001'

--- lay ten admin
CREATE OR ALTER PROCEDURE sp_getNameAdmin @ID_ADMIN char(5)
AS
BEGIN
	if (not exists (SELECT * FROM NGUOI_DUNG WHERE ID_USER = @ID_ADMIN))
	begin
		raiserror(N'Không tồn tại người dùng có id này', 16, 1)
		return 0
	end

	if (not exists (SELECT * FROM QUAN_TRI_VIEN WHERE ID_QTV = @ID_ADMIN))
	begin
		raiserror(N'Không tồn tại quản trị viên có id này', 16, 1)
		return 0
	end

	SELECT * FROM QUAN_TRI_VIEN WHERE ID_QTV = @ID_ADMIN

END
GO

-- QTV1: Thêm loại thuốc
create or alter proc sp_THEMTHUOC 
	@tenthuoc NVARCHAR(50),
	@donvitinh NCHAR(10),
	@chidinh NVARCHAR(50),
	@tonkho INT,
	@ngayhethan DATE,
	@dongia INT,
	@id_qtv NCHAR(5)
as
begin
	-- Kiểm tra id quản trị viên đã tồn tại chưa
	if (not exists (SELECT * FROM QUAN_TRI_VIEN WHERE ID_QTV = @id_qtv))
	begin
		raiserror('ID quản trị viên này không tồn tại', 16, 1)
		return 0
	end

	-- Kiểm tra ngày hết hạn có lớn hơn ngày hiện tại hay không
	if (@ngayhethan <= GETDATE())
    begin
        raiserror('Ngày hết hạn phải lớn hơn ngày hiện tại', 16, 1)
        return 0
    end

	-- Kiểm tra số lượng tồn kho và đơn giá có lớn hơn 0
	if (@tonkho <= 0)
    begin
        raiserror('Số lượng tồn kho phải lớn hơn 0', 16, 1)
        return 0
    end

	if (@dongia <= 0)
    begin
        raiserror('Đơn giá phải lớn hơn 0', 16, 1)
        return 0
    end

	-- Tạo id thuốc tăng dần
	declare	@id_thuoc NCHAR(5)
	set @id_thuoc = (select max(ID_THUOC) from THUOC)
	if @id_thuoc is null
		begin  
			set @id_thuoc = 'T0001'
		end
	else
		begin 
			declare @stt int
			set @id_thuoc = (select 'T' + format((cast(substring(max(@id_thuoc),2,4) as int) + 1), '0000') from THUOC)
		end

	-- Thêm thuốc
	insert into THUOC values (@id_thuoc, @tenthuoc, @donvitinh, @chidinh, @tonkho, @ngayhethan, @dongia, @id_qtv)
end
go

-- QTV2: Xoá loại thuốc
create or alter proc sp_XOATHUOC 
    @id_thuoc char(5)
as
begin
    -- Kiểm tra xem id thuốc có tồn tại không
    if not exists (select * from THUOC where ID_THUOC = @id_thuoc)
    begin
        raiserror('ID thuốc không tồn tại', 16, 1)
        return 0
    end

    -- Xoá thuốc từ bảng THUOC
    delete from THUOC where ID_THUOC = @id_thuoc
    
    -- Cập nhật lại các ID_THUOC có giá trị lớn hơn @id_thuoc 
    update THUOC set ID_THUOC = 'T' + format(cast(substring(ID_THUOC, 2, len(ID_THUOC) - 1) as int) - 1, '0000') where ID_THUOC > @id_thuoc
end
go


--QTV3: Cập nhập lại số lượng tồn
create or alter proc sp_CAPNHAPSOLUONGTON
	@id_thuoc char(5),
	@tonkho int
as
begin
	-- Kiểm tra xem id thuốc có tồn tại không
	if not exists (select * from THUOC where ID_THUOC = @id_thuoc)
	begin
		raiserror('ID thuốc không tồn tại', 16, 1)
		return 0
	end

	-- Kiểm tra số lượng tồn kho có lớn hơn 0
	if (@tonkho <= 0)
    begin
        raiserror('Số lượng tồn kho phải lớn hơn 0', 16, 1)
        return 0
    end
	
	-- Cập nhập số lượng tồn
	UPDATE THUOC SET TONKHO = @tonkho where ID_THUOC = @id_thuoc
end
go

--Test
exec sp_THEMTHUOC 'abc', N'viên', N'2 ngày 1 lần', 100, '1-10-2024', 3000, 'QTV01' 
exec sp_THEMTHUOC 'acd', N'sủi', N'1 ngày 2 lần', 200, '1-12-2024', 3000, 'QTV01' 
exec sp_THEMTHUOC 'a3b', N'tiêm', N'1 ngày 1 lần', 100, '1-14-2024', 10000, 'QTV01' 
select * from THUOC
exec sp_CAPNHAPSOLUONGTON 'T0007', 20
exec sp_CAPNHAPSOLUONGTON 'T0008', 50
select * from THUOC
exec sp_XOATHUOC 'T0007'
select * from THUOC
go
--QTV6: Xem danh sách người dùng - error
CREATE OR ALTER PROC sp_XEMNGUOIDUNG
AS
BEGIN TRAN
SET TRAN ISOLATION LEVEL REPEATABLE READ
	-- Tính tổng số lượng người dùng
	SELECT COUNT(*) AS TONG_NGUOI_DUNG
	FROM NGUOI_DUNG;

	-- Test
	WAITFOR DELAY '0:0:05'

	-- Hiển thị chi tiết danh sách người dùng
	SELECT ND.ID_USER,
		   ND.DIENTHOAI,
		   ND.MATKHAU,
		   ND.VAITRO,
		   COALESCE(KH.HOTEN, QTV.HOTEN, NV.HOTEN, NS.HOTEN) AS HOTEN
	FROM NGUOI_DUNG ND LEFT JOIN KHACH_HANG KH ON ND.ID_USER = KH.ID_KH
					   LEFT JOIN QUAN_TRI_VIEN QTV ON ND.ID_USER = QTV.ID_QTV
					   LEFT JOIN NHAN_VIEN NV ON ND.ID_USER = NV.ID_NV
					   LEFT JOIN NHA_SI NS ON ND.ID_USER = NS.ID_NS
COMMIT TRAN
RETURN 0
GO

--QTV7: Xem danh sách hoá đơn
CREATE OR ALTER PROC sp_XEMHOADON
AS
BEGIN TRAN
	-- Tính tổng số lượng hoá đơn
	SELECT COUNT(*) AS TONG_SO_HD
	FROM HOA_DON;

	-- Test
	WAITFOR DELAY '0:0:05'

	-- Hiển thị danh sách hoá đơn
	SELECT * FROM HOA_DON;

COMMIT TRAN
RETURN 0
