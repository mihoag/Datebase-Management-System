﻿USE MASTER
GO

IF DB_ID('QUANLYPHONGKHAM') IS NOT NULL DROP DATABASE QUANLYPHONGKHAM
GO

CREATE DATABASE QUANLYPHONGKHAM
GO

USE QUANLYPHONGKHAM

CREATE TABLE NGUOI_DUNG
(
	ID_USER NCHAR(5),
	DIENTHOAI CHAR(10),
	MATKHAU NCHAR(20),
	VAITRO NCHAR(20),
	ACTIVE bit default 1,
	PRIMARY KEY (ID_USER)
)

CREATE TABLE KHACH_HANG
(
	ID_KH NCHAR(5),
	HOTEN NVARCHAR(50),
	NGAYSINH DATE,
	DIACHI NVARCHAR(50),
	PRIMARY KEY (ID_KH)
)


CREATE TABLE QUAN_TRI_VIEN
(
	ID_QTV NCHAR(5),
	HOTEN NVARCHAR(50),
	PRIMARY KEY(ID_QTV)
)

CREATE TABLE NHAN_VIEN
(
	ID_NV NCHAR(5),
	HOTEN NVARCHAR(50),
	PRIMARY KEY (ID_NV),
)

CREATE TABLE NHA_SI
(
	ID_NS NCHAR(5),
	HOTEN NVARCHAR(50),
	PRIMARY KEY (ID_NS)
)

CREATE TABLE LICH_KHAM
(
	ID_LICHHEN NCHAR(5),
	ID_NS NCHAR(5),
	ID_KH NCHAR(5),
	ID_NV NCHAR(5),
	NGAYHEN DATE,
	GIO_BD TIME,
	GIO_KT TIME,
	PRIMARY KEY (ID_LICHHEN)
)

CREATE TABLE LICH_NHA_SI
(
	ID_NS NCHAR(5),
	NGAYHEN DATE,
	GIO_BD TIME,
	GIO_KT TIME,
	CHITIET NVARCHAR(50),
	PRIMARY KEY (ID_NS, NGAYHEN, GIO_BD)
)

CREATE TABLE HS_BENH_NHAN
(
	ID_BN NCHAR(5),
	ID_KH NCHAR(5),
	PRIMARY KEY (ID_BN)
)

CREATE TABLE CHI_TIET_HS
(
	ID_BN NCHAR(5),
	NGAYKHAM DATE,
	TINHTRANG NVARCHAR(50),
	NGUOIKHAM NCHAR(5)
	PRIMARY KEY (ID_BN, NGAYKHAM)
)

CREATE TABLE DON_THUOC
(
	ID_THUOC NCHAR(5),
	ID_BN NCHAR(5),
	NGAYKHAM DATE,
	SOLUONG INT,
	PRIMARY KEY (ID_THUOC, ID_BN,  NGAYKHAM)
)

CREATE TABLE THUOC
(
	ID_THUOC NCHAR(5),
	TENTHUOC NVARCHAR(50),
	DONVITINH NCHAR(10),
	CHIDINH NVARCHAR(50),
	TONKHO INT,
	NGAYHETHAN DATE,
	DONGIA INT,
	ID_QTV NCHAR(5)
	PRIMARY KEY (ID_THUOC)
)

CREATE TABLE DICH_VU
(
	ID_DV NCHAR(5),
	TENDV NVARCHAR(50),
	DONGIA INT,
	PRIMARY KEY (ID_DV)
)

CREATE TABLE CHI_TIET_DV
(
	ID_DV NCHAR(5),
	ID_BN NCHAR(5),
	NGAYKHAM DATE,
	SOLUONG INT,
	PRIMARY KEY (ID_BN, ID_DV, NGAYKHAM)
)

CREATE TABLE HOA_DON
(
	ID_HOADON NCHAR(5),
	ID_BN NCHAR(5),
	NGAYKHAM DATE,
	PHIKHAM INT,
	THANHTIEN INT,
	ID_NV NCHAR(5),
	ID_KH NCHAR(5),
	PRIMARY KEY (ID_HOADON, ID_BN, NGAYKHAM)
)

ALTER TABLE KHACH_HANG
ADD CONSTRAINT FK_KH_ND
FOREIGN KEY (ID_KH)
REFERENCES NGUOI_DUNG (ID_USER);

ALTER TABLE NHAN_VIEN
ADD CONSTRAINT FK_NV_ND
FOREIGN KEY (ID_NV)
REFERENCES NGUOI_DUNG (ID_USER);

ALTER TABLE NHA_SI
ADD CONSTRAINT FK_NS_ND
FOREIGN KEY (ID_NS)
REFERENCES NGUOI_DUNG (ID_USER);

ALTER TABLE QUAN_TRI_VIEN
ADD CONSTRAINT FK_QTV_ND
FOREIGN KEY (ID_QTV)
REFERENCES NGUOI_DUNG (ID_USER);

ALTER TABLE LICH_KHAM
ADD CONSTRAINT FK_LK_KH
FOREIGN KEY (ID_KH)
REFERENCES KHACH_HANG (ID_KH);

ALTER TABLE LICH_KHAM
ADD CONSTRAINT FK_LK_NS
FOREIGN KEY (ID_NS)
REFERENCES NHA_SI (ID_NS);

ALTER TABLE LICH_KHAM
ADD CONSTRAINT FK_LK_NV
FOREIGN KEY (ID_NV)
REFERENCES NHAN_VIEN (ID_NV);

ALTER TABLE LICH_NHA_SI
ADD CONSTRAINT FK_LNS_NS
FOREIGN KEY (ID_NS)
REFERENCES NHA_SI (ID_NS);

ALTER TABLE HS_BENH_NHAN
ADD CONSTRAINT FK_HSBN_KH
FOREIGN KEY (ID_KH)
REFERENCES KHACH_HANG (ID_KH);

ALTER TABLE CHI_TIET_HS
ADD CONSTRAINT FK_CTHS_HSBN
FOREIGN KEY (ID_BN)
REFERENCES HS_BENH_NHAN (ID_BN);

ALTER TABLE CHI_TIET_HS
ADD CONSTRAINT FK_CTHS_NS
FOREIGN KEY (NGUOIKHAM)
REFERENCES NHA_SI (ID_NS);

ALTER TABLE THUOC
ADD CONSTRAINT FK_THUOC_QTV
FOREIGN KEY (ID_QTV)
REFERENCES QUAN_TRI_VIEN (ID_QTV);

ALTER TABLE DON_THUOC
ADD CONSTRAINT FK_DT_HSBN
FOREIGN KEY (ID_BN, NGAYKHAM)
REFERENCES CHI_TIET_HS (ID_BN, NGAYKHAM);

ALTER TABLE DON_THUOC
ADD CONSTRAINT FK_DT_THUOC
FOREIGN KEY (ID_THUOC)
REFERENCES THUOC (ID_THUOC);

ALTER TABLE CHI_TIET_DV
ADD CONSTRAINT FK_CTDV_DV
FOREIGN KEY (ID_DV)
REFERENCES DICH_VU (ID_DV);

ALTER TABLE CHI_TIET_DV
ADD CONSTRAINT FK_CTDV_CTHS
FOREIGN KEY (ID_BN, NGAYKHAM)
REFERENCES CHI_TIET_HS (ID_BN, NGAYKHAM);

ALTER TABLE HOA_DON
ADD CONSTRAINT FK_HD_CTHS
FOREIGN KEY (ID_BN, NGAYKHAM)
REFERENCES CHI_TIET_HS (ID_BN, NGAYKHAM);

ALTER TABLE HOA_DON
ADD CONSTRAINT FK_HD_NV
FOREIGN KEY (ID_NV)
REFERENCES NHAN_VIEN (ID_NV);

ALTER TABLE HOA_DON
ADD CONSTRAINT FK_HD_KH
FOREIGN KEY (ID_KH)
REFERENCES KHACH_HANG (ID_KH);

ALTER TABLE NGUOI_DUNG
ADD CONSTRAINT CHECK_ROLE_USER
CHECK (VAITRO IN ('KHACH HANG', 'NHA SI', 'NHAN VIEN', 'QUAN TRI VIEN')); 

-------------------------------- insert du lieu
-- Bang nguoi dung
INSERT INTO NGUOI_DUNG(ID_USER, DIENTHOAI, MATKHAU, VAITRO) VALUES
('BN001', '0123456789', '123456', 'KHACH HANG'),
('BN002', '0123456799', '123456', 'KHACH HANG'),
('NS001', '0123456788', '123456', 'NHA SI'),
('NS002', '0123456798', '123456', 'NHA SI'),
('NV001', '0123456889', '123456', 'NHAN VIEN'),
('NV002', '0123456888', '123456', 'NHAN VIEN'),
('QTV01', '0123456189', '123456', 'QUAN TRI VIEN'),
('QTV02', '0123456188', '123456', 'QUAN TRI VIEN');

-- Bang khach hang
INSERT INTO KHACH_HANG (ID_KH, HOTEN, NGAYSINH,	DIACHI) VALUES
('BN001', N'Nguyễn Nhật Hào', '2003-7-20', N'Gò Công, Tiền Giang'),
('BN002', N'Lê Văn An', '2002-5-10', N'Thanh Xuân, Hà Nội');

-- Bang Quan tri vien
INSERT INTO QUAN_TRI_VIEN (ID_QTV, HOTEN) VALUES
('QTV01', N'Bùi Minh Duy'),
('QTV02', N'Nguyễn Văn Lâm');

-- Bang Nhan Vien
INSERT INTO NHAN_VIEN (ID_NV, HOTEN) VALUES
('NV001', N'Lê Minh Hoàng'),
('NV002', N'Trương Trọng Tài');

-- Bang Nha si
INSERT INTO NHA_SI (ID_NS, HOTEN) VALUES
('NS001', N'Tô Phương Hiếu'),
('NS002', N'Đỗ Trọng Nhân');

-- Bang Lich kham
INSERT INTO LICH_KHAM (ID_LICHHEN,ID_NS,ID_KH,ID_NV,NGAYHEN,GIO_BD,GIO_KT) VALUES
('LH001', 'NS001', 'BN001', 'NV001', '2024-12-18', '08:00:00', '09:00:00'),
('LH002', 'NS001', 'BN002', 'NV002', '2024-12-18', '10:00:00', '10:45:00'),
('LH003', 'NS001', 'BN002', null, '2023-12-20', '08:00:00', '09:00:00'),
('LH004', 'NS002', 'BN001', null, '2023-12-18', '08:00:00', '09:00:00'),
('LH005', 'NS002', 'BN002', 'NV001', '2023-12-28', '14:00:00', '16:00:00');

-- Bang Lich Nha Si
INSERT INTO LICH_NHA_SI(ID_NS,NGAYHEN,GIO_BD,GIO_KT,CHITIET) VALUES
('NS001','2023-12-18','07:00:00','07:40:00',N'Đưa con đi học'),
('NS001','2023-12-18','15:00:00','16:00:00',N'Rướt con đi học về'),
('NS001','2023-12-20','07:00:00','07:40:00',N'Họp ban'),
('NS001','2023-12-21','07:00:00','16:00:00',N'Công tác'),
('NS002','2023-12-18','09:00:00','10:40:00',N'Họp ban'),
('NS002','2023-12-20','07:00:00','16:00:00',N'Công tác'),
('NS001', '2024-12-18','08:00:00', '09:00:00', N'Khám bệnh'),
('NS001', '2024-12-18', '10:00:00', '10:45:00',N'Khám bệnh'),
('NS001', '2023-12-20', '08:00:00', '09:00:00', N'Khám bệnh'),
('NS002', '2023-12-18', '08:00:00', '09:00:00', N'Khám bệnh'),
('NS002', '2023-12-28', '14:00:00', '16:00:00', N'Khám bệnh');

-- Bang Thuoc
INSERT INTO THUOC(ID_THUOC,TENTHUOC,DONVITINH,CHIDINH,TONKHO,NGAYHETHAN,DONGIA,ID_QTV) VALUES 
('T0001','Novocain',N'Hộp',N'Thuốc không dùng với người dị ứng',100,'2024-12-12',100000,'QTV01'),
('T0002','Paracetamol',N'Hộp',N'Thuốc không dùng với người dị ứng',100,'2024-1-12',100000,'QTV02'),
('T0003','Aspirin',N'Chai',N'Thuốc không dùng với người dị ứng',100,'2025-10-12',50000,'QTV01'),
('T0004','Amoxillin',N'Chai',N'Thuốc không dùng với người dị ứng',100,'2025-11-12',30000,'QTV02'),
('T0005','Acyclovir',N'Viên',N'Thuốc không dùng với người dị ứng da',100,'2024-12-30',5000,'QTV01'),
('T0006','Pilocarpin',N'Chai',N'Thuốc không dùng với người dị ứng',100,'2026-02-20',30000,'QTV02');

-- bang Dich vu
INSERT INTO DICH_VU(ID_DV, TENDV, DONGIA) VALUES
('DV001', N'Cạo vôi răng', 100000),
('DV002', N'Nhổ răng', 2000000),
('DV003', N'Tẩy trắng răng', 100000),
('DV004', N'Trồng răng', 3000000),
('DV005', N'Niềng răng', 40000000);

----------------------------- Kham benh 1-------------------------
-- bang ho so benh nhan
INSERT INTO HS_BENH_NHAN(ID_BN, ID_KH) VALUES ('HS001', 'BN001');
-- bang Chi tiet ho so benh nhan
INSERT INTO CHI_TIET_HS(ID_BN,NGAYKHAM,TINHTRANG,NGUOIKHAM)VALUES('HS001','2023-12-18',N'Đau, nhứt răng','NS001');
-- bang Don thuoc
INSERT INTO DON_THUOC(ID_THUOC,ID_BN,NGAYKHAM,SOLUONG)VALUES('T0001','HS001','2023-12-18',1);
INSERT INTO DON_THUOC(ID_THUOC,ID_BN,NGAYKHAM,SOLUONG)VALUES('T0002','HS001','2023-12-18',2);
-- Bang Chi tiet dich vu
INSERT INTO CHI_TIET_DV(ID_DV,ID_BN,NGAYKHAM,SOLUONG)VALUES('DV001','HS001','2023-12-18',1);
-- Bang Don thuoc
INSERT INTO HOA_DON(ID_HOADON,ID_BN,NGAYKHAM,PHIKHAM,THANHTIEN,ID_NV,ID_KH)VALUES
('HD001', 'HS001', '2023-12-18',50000,450000,'NV001','BN001');
-----------------------------------------------------------------

----------------------------- Kham benh 2-------------------------
-- bang ho so benh nhan
INSERT INTO HS_BENH_NHAN(ID_BN, ID_KH) VALUES ('HS002', 'BN002');
-- bang Chi tiet ho so benh nhan
INSERT INTO CHI_TIET_HS(ID_BN,NGAYKHAM,TINHTRANG,NGUOIKHAM)VALUES('HS002','2023-12-18',N'Đau, nhứt răng','NS002');
-- bang Don thuoc
INSERT INTO DON_THUOC(ID_THUOC,ID_BN,NGAYKHAM,SOLUONG)VALUES('T0001','HS002','2023-12-18',2);
INSERT INTO DON_THUOC(ID_THUOC,ID_BN,NGAYKHAM,SOLUONG)VALUES('T0002','HS002','2023-12-18',2);
-- Bang Chi tiet dich vu
INSERT INTO CHI_TIET_DV(ID_DV,ID_BN,NGAYKHAM,SOLUONG)VALUES('DV001','HS002','2023-12-18',1);
INSERT INTO CHI_TIET_DV(ID_DV,ID_BN,NGAYKHAM,SOLUONG)VALUES('DV002','HS002','2023-12-18',1);
-- Bang Don thuoc
INSERT INTO HOA_DON(ID_HOADON,ID_BN,NGAYKHAM,PHIKHAM,THANHTIEN,ID_NV,ID_KH)VALUES
('HD002', 'HS002', '2023-12-18',100000,800000,'NV002','BN002');
-----------------------------------------------------------------

----------------------------- Kham benh 3-------------------------
-- bang Chi tiet ho so benh nhan
INSERT INTO CHI_TIET_HS(ID_BN,NGAYKHAM,TINHTRANG,NGUOIKHAM)VALUES('HS001','2023-12-20',N'Đau, nhứt răng','NS002');
-- bang Don thuoc
INSERT INTO DON_THUOC(ID_THUOC,ID_BN,NGAYKHAM,SOLUONG)VALUES('T0001','HS001','2023-12-20',2);
INSERT INTO DON_THUOC(ID_THUOC,ID_BN,NGAYKHAM,SOLUONG)VALUES('T0002','HS001','2023-12-20',2);
-- Bang Chi tiet dich vu
INSERT INTO CHI_TIET_DV(ID_DV,ID_BN,NGAYKHAM,SOLUONG)VALUES('DV001','HS001','2023-12-20',1);
INSERT INTO CHI_TIET_DV(ID_DV,ID_BN,NGAYKHAM,SOLUONG)VALUES('DV002','HS001','2023-12-20',1);
-- Bang Don thuoc
INSERT INTO HOA_DON(ID_HOADON,ID_BN,NGAYKHAM,PHIKHAM,THANHTIEN,ID_NV,ID_KH)VALUES
('HD003', 'HS001', '2023-12-20',100000,800000,'NV002','BN001');
-----------------------------------------------------------------
