use master
go

use QUANLYPHONGKHAM
go
-- (Khach hang)
-- sp_kiemTraSDTKH: Kiem tra so dien thoai (tai khoan) ton tai chua va pk co role la KHACH HANG
-- input: @SDT char(10)
-- output: bit (1: ton tai, 0: khong ton tai)
CREATE OR ALTER PROCEDURE sp_kiemTraSDTKH @SDT char(10)
AS
BEGIN
	if (not exists (SELECT * FROM NGUOI_DUNG ND WHERE DIENTHOAI = @SDT AND VAITRO = 'KHACH HANG'))
	begin
		return 0
	end

	return 1
END
GO



--- (Khach hang)
--- sp_thongTinKhachHang: Lay thong tin khach hang
--- input: @SDT char(10)
--- output: list data
CREATE OR ALTER PROC sp_thongTinKhachHang @SDT char(10)
AS
BEGIN
	-- check role is KHACH HANG
	DECLARE @TEST bit
	EXEC @TEST = sp_kiemTraSDTKH  @SDT
	
	--ok -> select data
	if (@TEST = 1)
	BEGIN
		SELECT ND.DIENTHOAI, KH.HOTEN, KH.NGAYSINH, KH.DIACHI FROM NGUOI_DUNG ND, KHACH_HANG KH WHERE ND.ID_USER = KH.ID_KH AND ND.DIENTHOAI = @SDT
	END
	else begin 	rollback end
END
GO



--- (Khach hang)
--- sp_capNhatThongTin: cap nhat thong tin khach hang
--- input: @SDT char(10)
--- output: list data

CREATE OR ALTER PROC sp_CapNhatThongTin 
	@DIENTHOAI CHAR(10),@HOTEN NVARCHAR(50),@NGAYSINH DATE,@DIACHI NVARCHAR(50)
AS
BEGIN
	-- is user
	DECLARE @TEST bit
	EXEC @TEST = sp_kiemTraSDTKH  @DIENTHOAI

	if (@TEST = 0)begin raiserror(N'Chỉ khách hàng mới được cập nhật thông tin', 16, 1) return 0 end
	
	if (TRIM(@HOTEN) = ' ' or @HOTEN = '')
	begin
		raiserror(N'Họ tên và mật khẩu không được rổng', 16, 1)
		return 0
	end

	if (len(@DIENTHOAI) != 10)begin raiserror(N'Điện thoại phải có 10 số', 16, 1)return 0 end


	declare @id_kh nvarchar(5)
	SELECT @id_kh = ID_USER FROM NGUOI_DUNG WHERE DIENTHOAI = @DIENTHOAI
	UPDATE KHACH_HANG SET HOTEN = @HOTEN, NGAYSINH = @NGAYSINH, DIACHI = @DIACHI WHERE ID_KH = @id_kh 
	return 1
END
GO




--- (Khach hang)
--- sp_thongTinKhachHang: lay chi tiet benh an cua khach hang (SDT)
--- input: @id_bn nchar(5)
--- output:
CREATE OR ALTER PROC sp_chiTietBenhAn_Thuoc @id_bn nchar(5), @ngay_kham Date
AS
BEGIN
	SELECT t.TENTHUOC, t.CHIDINH, dt.SOLUONG, t.DONGIA 
	FROM DON_THUOC dt, THUOC t 
	WHERE dt.ID_BN = @id_bn and t.ID_THUOC=dt.ID_THUOC and NGAYKHAM = @ngay_kham
END
GO



CREATE OR ALTER PROC sp_chiTietBenhAn_DV @id_bn nchar(5), @ngay_kham Date
AS
BEGIN
	SELECT dv.TENDV, ct.SOLUONG, dv.DONGIA
	FROM DICH_VU dv, CHI_TIET_DV ct
	WHERE dv.ID_DV = ct.ID_DV and ct.ID_BN = @id_bn and NGAYKHAM = @ngay_kham
END
GO



CREATE OR ALTER PROC sp_chiTietBenhAn_HD @id_bn nchar(5), @ngay_kham Date
AS
BEGIN
	SELECT * FROM HOA_DON WHERE ID_BN = @id_bn and NGAYKHAM = @ngay_kham
END
GO

--- (Khach hang)
--- sp_timLichRanh: tim danh sach bac si ranh trong ngay va thoi gian (SDT)
--- input: @SDT char(10)
--- output: list data

--- (Khach hang)
--- sp_xemLichKham: lay chi tiet benh an cua khach hang 
--- input: @id_kh nvarchar(5)
--- output: list data
CREATE or ALTER PROC sp_xemLichKham @id_kh nvarchar(5)
AS
BEGIN
	-- get id kh
	SELECT NGAYHEN, GIO_BD, GIO_KT, ns.HOTEN, ns.ID_NS  FROM LICH_KHAM lk, NHA_SI ns
	WHERE ID_KH = @id_kh and lk.ID_NS = ns.ID_NS
	and cast(NGAYHEN as datetime) + cast(GIO_KT as datetime) >= GETDATE()
END
GO




--- (Khach hang)
--- sp_xemHoSoBN: lay ho so benh nhan
--- input: @id_kh nvarchar(5)
--- output: list data
CREATE or ALTER proc sp_xemHoSoBN @id_kh nvarchar(5)
AS
BEGIN
	SELECT ct.*, ns.HOTEN FROM CHI_TIET_HS ct, HS_BENH_NHAN hs, NHA_SI ns WHERE hs.ID_KH = @id_kh and ct.ID_BN = hs.ID_BN and ns.ID_NS = ct.NGUOIKHAM 
END
GO


create or alter proc sp_tatcaLHNS 
as
begin
   select *  from LICH_NHA_SI
end
go


create or alter proc sp_DATLICHKHAM @id_ns nchar(5), @id_kh nchar(5), @id_nv nchar(5), @ngayhen date, @gio_bd time, @gio_kt time
as
BEGIN TRAN
   -- Kiem tra ID_NS co ton tai khong
     if not exists(select * from NHA_SI where ID_NS = @id_ns)
      begin
        print N'ID nha sĩ không tồn tại'
	    ROLLBACK TRAN 
	    return 1
     end
   -- Kiem tra ID_KH co ton tai khong
      if not exists(select * from KHACH_HANG where ID_KH = @id_kh)
        begin
          print N'ID khách hàng không tồn tại'
	      ROLLBACK TRAN 
	      return 1
      end
   -- Neu nguoi dung dat lich kham thi ID_NV bang null
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
         print N'khung giờ hẹn có bị kẹt bởi lịch khác của nha sĩ'
	     ROLLBACK TRAN 
	     return 1
	   end
   -- cap nhat lai lich hen cua nha si
   insert into LICH_NHA_SI (ID_NS, NGAYHEN, GIO_BD, GIO_KT, CHITIET) values (@id_ns, @ngayhen, @gio_bd, @gio_kt, NULL)
   WAITFOR DELAY '0:0:05'
   if @gio_bd between '11:00:00' and '13:30:00'
     begin
		--RAISEERROR('Time is not valid', 16, 1);
		--THROW 50001, 'This is an error message.', 1;
		ROLLBACK TRAN 
	    raiserror(N'Thời gian không hợp lệ',16,	1)
		RETURN 1;
	 end
   -- cap nhat lai trong lich kham cua benh nhan
   -- Tao ID lich hen tu dong tang
    declare @id_lichhen nchar(5)
	set @id_lichhen = (select max(ID_LICHHEN) from LICH_KHAM)
	if @id_lichhen is null
	  begin  
	     set @id_lichhen = 'LH001'
	  end
	else
	  begin 
		 set @id_lichhen = (select 'LH'  + REPLACE(str((CAST(substring(max(@id_lichhen),3,3) as int) + 1) ,3), ' ', '0')
                     from LICH_KHAM)
      end
   insert into LICH_KHAM (ID_LICHHEN, ID_NS, ID_KH, ID_NV,NGAYHEN, GIO_BD, GIO_KT) values (@id_lichhen, @id_ns, @id_kh, @id_nv, @ngayhen, @gio_bd, @gio_kt)
COMMIT TRAN
GO




go



--- (Quan tri vien)
--- sp_addNewAccount: them tai khoan moi
--- input: @DIENTHOAI CHAR(10),	@MATKHAU NCHAR(20), @VAITRO NCHAR(20), @HOTEN NVARCHAR(50)
--- output: 

create or alter proc sp_addNewAccount @DIENTHOAI CHAR(10),@MATKHAU NCHAR(20),@VAITRO NCHAR(20),@HOTEN NVARCHAR(50)
as
begin
	if (LEN(@DIENTHOAI) != 10)
	begin
		raiserror(N'Số điện thoại phải có độ dài là 10', 16, 1);
		return
	end
	if (LEN(@HOTEN) <= 0)
	begin
		raiserror(N'Họ tên không được để trống', 16, 1);
		return
	end
	if (LEN(@MATKHAU) <= 0)
	begin
		raiserror(N'Mật khẩu không được để trống', 16, 1);
		return
	end
	if (exists (select * from NGUOI_DUNG where DIENTHOAI = @DIENTHOAI))
	begin
		raiserror(N'Số điện thoại này đã tồn tại', 16, 1);
		return
	end
	-- tao id nguoi dung
	declare @id_user nchar(5)
	if @VAITRO = 'NHA SI'
	begin
		set @id_user = (select Max(ID_NS) from NHA_SI)
		if @id_user is null
		begin
			set @id_user = 'NS001'
		end
		else
		begin
			 set @id_user = (select 'NS'  + REPLACE(str((CAST(substring(max(@id_user),3,3) as int) + 1) ,3), ' ', '0')
                     from NHA_SI)
		end
		INSERT INTO NGUOI_DUNG(ID_USER, DIENTHOAI, MATKHAU, VAITRO) VALUES
		(@id_user, @DIENTHOAI, @MATKHAU, @VAITRO);
		INSERT INTO NHA_SI (ID_NS, HOTEN) VALUES(@id_user, @HOTEN);
	end
	else
	begin
		set @id_user = (select Max(ID_NV) from NHAN_VIEN)
		if @id_user is null
		begin
			set @id_user = 'NV001'
		end
		else
		begin
			 set @id_user = (select 'NV'  + REPLACE(str((CAST(substring(max(@id_user),3,3) as int) + 1) ,3), ' ', '0')
                     from NHAN_VIEN)
		end
		INSERT INTO NGUOI_DUNG(ID_USER, DIENTHOAI, MATKHAU, VAITRO) VALUES
		(@id_user, @DIENTHOAI, @MATKHAU, @VAITRO);
		INSERT INTO NHAN_VIEN (ID_NV, HOTEN) VALUES	(@id_user, @HOTEN);
	end
end
go


--- (Quan tri vien)
--- sp_getAllAccount: lay tat ca account (nhan vien, nguoi dung, bac si)
--- input: None
--- output: list
create or alter proc sp_getAllAccount 
as
begin
	select ID_USER, DIENTHOAI, HOTEN, VAITRO, ACTIVE from NGUOI_DUNG ng, KHACH_HANG kh WHERE VAITRO != 'QUAN TRI VIEN' and ng.ID_USER = kh.ID_KH
	union
	select ID_USER, DIENTHOAI, HOTEN, VAITRO, ACTIVE from NGUOI_DUNG ng, NHAN_VIEN nv WHERE VAITRO != 'QUAN TRI VIEN' and ng.ID_USER = nv.ID_NV
	union
	select ID_USER, DIENTHOAI, HOTEN, VAITRO, ACTIVE from NGUOI_DUNG ng, NHA_SI ns WHERE VAITRO != 'QUAN TRI VIEN' and ng.ID_USER = ns.ID_NS
end
go


--- (Quan tri vien)
--- sp_searchUser: search nguoi dung
--- input: @search nvarchar(10)
--- output: list
create or alter proc sp_searchUser @search nvarchar(10)
as
begin
	select ID_USER, DIENTHOAI, HOTEN, VAITRO, ACTIVE from NGUOI_DUNG ng, KHACH_HANG kh WHERE VAITRO != 'QUAN TRI VIEN' and ng.ID_USER = kh.ID_KH and DIENTHOAI like ('%'+@search+'%')
	union
	select ID_USER, DIENTHOAI, HOTEN, VAITRO, ACTIVE from NGUOI_DUNG ng, NHAN_VIEN nv WHERE VAITRO != 'QUAN TRI VIEN' and ng.ID_USER = nv.ID_NV and DIENTHOAI like ('%'+@search+'%')
	union
	select ID_USER, DIENTHOAI, HOTEN, VAITRO, ACTIVE from NGUOI_DUNG ng, NHA_SI ns WHERE VAITRO != 'QUAN TRI VIEN' and ng.ID_USER = ns.ID_NS and DIENTHOAI like ('%'+@search+'%')
end
go



--- (Quan tri vien)
--- sp_lockAccount: khoa tai khoan
--- input: @id_user NCHAR(5)
--- output: none
create or alter proc sp_lockAccount @id_user NCHAR(5)
as
BEGIN
	if (not exists (SELECT * FROM NGUOI_DUNG WHERE ID_USER = @id_user))
	begin
		raiserror('Người dùng không tồn tại', 16, 1)
		return 0
	end
	UPDATE NGUOI_DUNG SET ACTIVE = 0 WHERE ID_USER = @id_user
	return 1
END
go



--- (Quan tri vien)
--- sp_activeAccount: mo khoa tai khoan
--- input: @id_user NCHAR(5)
--- output: none
create or alter proc sp_activeAccount @id_user NCHAR(5)
as
BEGIN
	if (not exists (SELECT * FROM NGUOI_DUNG WHERE ID_USER = @id_user))
	begin
		raiserror('Người dùng không tồn tại', 16, 1)
		return 0
	end
	UPDATE NGUOI_DUNG SET ACTIVE = 1 WHERE ID_USER = @id_user
	return 1
END
go


