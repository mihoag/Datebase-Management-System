use QUANLYPHONGKHAM
go
-- Sp đặt lịch khám
select * from NHA_SI
select * from KHACH_HANG
go
-- drop proc  sp_DATLICHKHAM
create proc sp_DATLICHKHAM @id_ns nchar(5), @id_kh nchar(5), @id_nv nchar(5), @ngayhen date, @gio_bd time, @gio_kt time
as
begin
SET TRAN ISOLATION LEVEL READ COMMITTED
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
   if @gio_bd between '11:00:00' and '13:30:00'
     begin
	    print N'Khung giờ khám không hợp lệ'
	    ROLLBACK TRAN 
		RETURN 1
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
RETURN 2
end
GO
-- TEST
declare @check int
exec @check =  sp_DATLICHKHAM  'NS002', 'BN002',  'NV001', '2020-9-11', '8:30:00', '9:00:00'
print @check
go
---
select * from LICH_KHAM
delete from LICH_KHAM where ID_LICHHEN = 'LK2'
delete from LICH_NHA_SI
-- store procedure xem thuốc
create proc sp_xemthuoc 
as
SET TRAN ISOLATION LEVEL READ COMMITTED
BEGIN TRAN
   select * from THUOC
COMMIT TRAN
go

exec sp_xemthuoc  
go


--- sp xem thuoc theo ten
create proc sp_timthuoctheoten @tenthuoc nvarchar(50)
as
begin tran
   select * from THUOC where TENTHUOC = @tenthuoc
commit tran
go
  exec sp_timthuoctheoten 'aspirin'
go

-- sp xem ho so benh nhan
create proc sp_HSBN  
as
SET TRAN ISOLATION LEVEL READ COMMITTED
begin tran
select hs.ID_BN, k.ID_KH , k.HOTEN from HS_BENH_NHAN  hs , KHACH_HANG k, NGUOI_DUNG n
where  hs.ID_KH = k.ID_KH and n.ID_USER = k.ID_KH 
commit tran
go

-- Tesst 
exec sp_HSBN

----------------------------------------------------------------
-- Tao store procedure tim HSBN theo so dien thoai
go


create proc sp_HSBN_DT @dt CHAR(10) 
as
SET TRAN ISOLATION LEVEL READ COMMITTED
begin tran
select hs.ID_BN, k.ID_KH , k.HOTEN from HS_BENH_NHAN  hs , KHACH_HANG k, NGUOI_DUNG n
where  hs.ID_KH = k.ID_KH and n.ID_USER = k.ID_KH and n.DIENTHOAI = @dt
commit tran
go
-- test :
exec sp_HSBN_DT '098765111'
go

-- Store proc lap hoa don
-- drop proc sp_laphoadon
create proc sp_laphoadon  @id_bn nchar(5), @ngaykham date, @phikham int, 
@id_nv nchar(5), @id_kh nchar(5)
as
begin tran 
	-- Kiem tra xem ID benh nhan va ngay kham co ton tai hya khong
	if(not exists (select * from CHI_TIET_HS where @id_bn = ID_BN and NGAYKHAM = @ngaykham))
	begin 
	    rollback
		return 1
	end
	---- Tong thanh tien = phi kham + tong tien thuoc + tong tien dich vu
	declare @tienDV int, @tienThuoc int, @thanhTien int
	-- Tinh toan tong tien thuoc
	set @tienThuoc = ( select sum(t.DONGIA*dt.SOLUONG )  from THUOC t, DON_THUOC dt 
	  where t.ID_THUOC  = dt.ID_THUOC and dt.ID_BN = @id_bn and dt.NGAYKHAM = @ngaykham)
    -- Tinh toan tong tien dich vu
	set @tienDV =  (select  sum(d.DONGIA*ct.SOLUONG)  from DICH_VU d, CHI_TIET_DV ct
	  where d.ID_DV  = ct.ID_DV and  ct.ID_BN  = @id_bn and  ct.NGAYKHAM = @ngaykham)

	-- Kiem tra tien dich vu co null hay khong
    if @tienDV is null
	begin 
	    set @tienDV = 0
	end
	-- Kiem tra tong tien thuoc co null hay khong
    if @tienThuoc is null
	begin 
	    set @tienThuoc = 0
	end

	--- Tinh tong tien thuoc
	set @thanhTien = @tienDV + @tienThuoc + @phikham
    
	--- Tao ID hoa don tang dan
	declare @id_hoadon nchar(5)
	set @id_hoadon = (select max(ID_HOADON) from HOA_DON)
	if @id_hoadon is null
	  begin  
	     set @id_hoadon = 'HD001'
	  end
	else
	  begin 
         declare @stt int
		 set @id_hoadon = (select 'HD'  + REPLACE(str((CAST(substring(max(@id_hoadon),3,3) as int) + 1) ,3), ' ', '0')
                     from HOA_DON)
      end
	insert into HOA_DON values (@id_hoadon,@id_bn,@ngaykham,@phikham,@thanhTien,@id_nv,@id_kh)
commit tran
go

-- Test
exec sp_laphoadon  'BN001' , '2023-11-22', '100000' , 'NV001' ,'BN001' 
select * from NHAN_VIEN
delete from HOA_DON where ID_HOADON = 'HD012'
select * from HOA_DON

------test

select * from LICH_KHAM
select * from LICH_NHA_SI
select * from THUOC
select * from NHA_SI
select * from HS_BENH_NHAN
select * from KHACH_HANG
select * from NGUOI_DUNG


go
--- Sp chi tiet ho so benh nhan theo IDBN
create proc sp_chitietHSBN @id_bn nchar(5)
as
begin tran
    if not exists(select * from HS_BENH_NHAN where @id_bn = ID_BN)
	begin
	  rollback
	end
	select * from CHI_TIET_HS where @id_bn = ID_BN
commit tran
go
-- Test 
exec sp_chitietHSBN 'BN001'


go
---Store procedure xem tat ca lich hen cua cac nha si
create proc sp_tatcaLHNS 
as
begin tran
   select *  from LICH_NHA_SI
commit tran
go
exec sp_tatcaLHNS

-- Store proc xem lich nha si theo ID nha si
go

-- Lich hen nha si theo ID
create proc sp_LHNS_ID @ID_NS NCHAR(5)
as
begin tran
   select *  from LICH_NHA_SI where @ID_NS = ID_NS
commit tran
go

-- test
exec sp_LHNS_ID 'NS001'
-- 

exec p_DATLICHKHAM 'LH006', 'NS004', 'BN004', null, '2023-11-11', '8:30:00', '9:00:00'

select * from LICH_KHAM


--store procedure xoá 1 hoá đơn
CREATE OR ALTER PROC sp_XOAHOADON
	@id_hoadon NCHAR(5)
AS
BEGIN TRAN
	-- Kiểm tra id_hoadon có tồn tại hay không
	IF NOT EXISTS(SELECT * FROM HOA_DON WHERE ID_HOADON = @id_hoadon)
		BEGIN
			RAISERROR(N'ID hoá đơn này không tồn tại', 16, 1)
			ROLLBACK TRAN 
			RETURN 1
		END

    -- Xóa hoá đơn
	DELETE FROM HOA_DON WHERE ID_HOADON = @id_hoadon
	PRINT N'Xoá hoá đơn thành công'
COMMIT TRAN
RETURN 0
GO

insert into HOA_DON values('HD004', 'HS001', '2023-12-20', 100000, 800000, 'NV002', 'BN001')
select * from HOA_DON