/* =====================================================================================
   CSDL: QuanLyLopTiengAnh
   File tong hop (gop qlTienganh.sql + sql_thong_bao.sql + sql_tin_nhan.sql)

   THAY DOI SO VOI BAN GOC:
   1) Bo cac thuoc tinh TRUNG LAP giua [nguoi_dung] voi [hoc_vien] va [nguoi_dung] voi
      [giao_vien]: ho_ten, so_dien_thoai, email, dang_hoat_dong CHI con luu duy nhat
      trong bang [nguoi_dung] (thong tin dang nhap + thong tin lien he chinh).
      -> [hoc_vien] va [giao_vien] gio bat buoc phai co ma_nguoi_dung (NOT NULL, UNIQUE)
         va chi con luu them cac thuoc tinh RIENG cua ho so hoc vien / giao vien.
   2) Bo sung bang moi:
      - [bai_tap]  : bai tap giao viec giao cho lop / buoi hoc
      - [diem]     : diem cham bai tap cua hoc vien (tuong tu cap ky_thi/diem_thi)
   3) Gop them 2 bang [thong_bao] va [tin_nhan] tu 2 file rieng.

   Moi bang duoc nap SAN 10 ban ghi mau, du lieu tieng Viet, lien ket dung khoa ngoai
   voi nhau xuyen suot cac bang.
   ===================================================================================== */

IF DB_ID(N'QuanLyLopTiengAnh') IS NULL
BEGIN
    CREATE DATABASE [QuanLyLopTiengAnh];
END;
GO

USE [QuanLyLopTiengAnh];
GO

/* =====================================================================================
   1. XOA BANG CU (neu co) - xoa theo thu tu NGUOC voi thu tu phu thuoc
   ===================================================================================== */
IF OBJECT_ID(N'dbo.tin_nhan', N'U')      IS NOT NULL DROP TABLE [dbo].[tin_nhan];
IF OBJECT_ID(N'dbo.thong_bao', N'U')     IS NOT NULL DROP TABLE [dbo].[thong_bao];
IF OBJECT_ID(N'dbo.thanh_toan', N'U')    IS NOT NULL DROP TABLE [dbo].[thanh_toan];
IF OBJECT_ID(N'dbo.hoa_don', N'U')       IS NOT NULL DROP TABLE [dbo].[hoa_don];
IF OBJECT_ID(N'dbo.diem', N'U')          IS NOT NULL DROP TABLE [dbo].[diem];
IF OBJECT_ID(N'dbo.diem_thi', N'U')      IS NOT NULL DROP TABLE [dbo].[diem_thi];
IF OBJECT_ID(N'dbo.ky_thi', N'U')        IS NOT NULL DROP TABLE [dbo].[ky_thi];
IF OBJECT_ID(N'dbo.diem_danh', N'U')     IS NOT NULL DROP TABLE [dbo].[diem_danh];
IF OBJECT_ID(N'dbo.dang_ky_hoc', N'U')   IS NOT NULL DROP TABLE [dbo].[dang_ky_hoc];
IF OBJECT_ID(N'dbo.bai_tap', N'U')       IS NOT NULL DROP TABLE [dbo].[bai_tap];
IF OBJECT_ID(N'dbo.buoi_hoc', N'U')      IS NOT NULL DROP TABLE [dbo].[buoi_hoc];
IF OBJECT_ID(N'dbo.lich_hoc', N'U')      IS NOT NULL DROP TABLE [dbo].[lich_hoc];
IF OBJECT_ID(N'dbo.lop_hoc', N'U')       IS NOT NULL DROP TABLE [dbo].[lop_hoc];
IF OBJECT_ID(N'dbo.phong_hoc', N'U')     IS NOT NULL DROP TABLE [dbo].[phong_hoc];
IF OBJECT_ID(N'dbo.khoa_hoc', N'U')      IS NOT NULL DROP TABLE [dbo].[khoa_hoc];
IF OBJECT_ID(N'dbo.hoc_vien', N'U')      IS NOT NULL DROP TABLE [dbo].[hoc_vien];
IF OBJECT_ID(N'dbo.giao_vien', N'U')     IS NOT NULL DROP TABLE [dbo].[giao_vien];
IF OBJECT_ID(N'dbo.nguoi_dung', N'U')    IS NOT NULL DROP TABLE [dbo].[nguoi_dung];
IF OBJECT_ID(N'dbo.vai_tro', N'U')       IS NOT NULL DROP TABLE [dbo].[vai_tro];
GO

/* =====================================================================================
   2. TAO BANG - theo dung thu tu phu thuoc
   ===================================================================================== */

-- 2.1 vai_tro -------------------------------------------------------------------------
CREATE TABLE [dbo].[vai_tro]
(
    [ma_vai_tro]   INT IDENTITY(1,1) NOT NULL,
    [ten_vai_tro]  NVARCHAR(50)  NOT NULL,
    CONSTRAINT [PK_vai_tro] PRIMARY KEY ([ma_vai_tro]),
    CONSTRAINT [UQ_vai_tro_ten] UNIQUE ([ten_vai_tro])
);
GO

-- 2.2 nguoi_dung ------------------------------------------------------------------------
-- Noi luu DUY NHAT thong tin dinh danh / lien he: ho_ten, email, so_dien_thoai,
-- dang_hoat_dong. hoc_vien va giao_vien KHONG duoc lap lai cac cot nay nua.
CREATE TABLE [dbo].[nguoi_dung]
(
    [ma_nguoi_dung]   INT IDENTITY(1,1) NOT NULL,
    [ten_dang_nhap]   NVARCHAR(50)   NOT NULL,
    [mat_khau_hash]   NVARCHAR(255)  NOT NULL,
    [ho_ten]          NVARCHAR(100)  NOT NULL,
    [email]           NVARCHAR(100)  NULL,
    [so_dien_thoai]   NVARCHAR(20)   NULL,
    [ma_vai_tro]      INT NOT NULL,
    [avatar_url]      NVARCHAR(500)  NULL,
    [dang_hoat_dong]  BIT NOT NULL CONSTRAINT [df_nguoidung_hoatdong] DEFAULT (1),
    [ngay_tao]        DATETIME2(7)   NOT NULL CONSTRAINT [df_nguoidung_ngaytao] DEFAULT (SYSDATETIME()),
    CONSTRAINT [PK_nguoi_dung] PRIMARY KEY ([ma_nguoi_dung]),
    CONSTRAINT [UQ_nguoidung_tendangnhap] UNIQUE ([ten_dang_nhap]),
    CONSTRAINT [UQ_nguoidung_email] UNIQUE ([email]),
    CONSTRAINT [fk_nguoidung_vaitro] FOREIGN KEY ([ma_vai_tro]) REFERENCES [dbo].[vai_tro] ([ma_vai_tro])
);
GO
CREATE INDEX [idx_nguoidung_vaitro] ON [dbo].[nguoi_dung] ([ma_vai_tro]);
GO

-- 2.3 giao_vien ---------------------------------------------------------------------
-- Chi con cac thuoc tinh RIENG cua ho so giao vien (khong lap lai ho_ten/sdt/email/
-- trang thai hoat dong da co trong nguoi_dung).
CREATE TABLE [dbo].[giao_vien]
(
    [ma_giao_vien]     INT IDENTITY(1,1) NOT NULL,
    [ma_nguoi_dung]    INT NOT NULL,
    [gioi_tinh]        NVARCHAR(10)  NULL,
    [ngay_sinh]        DATE NULL,
    [chuyen_mon]       NVARCHAR(100) NULL,
    [ngay_vao_lam]     DATE NULL,
    [luong_theo_gio]   DECIMAL(10,2) NULL,
    CONSTRAINT [PK_giao_vien] PRIMARY KEY ([ma_giao_vien]),
    CONSTRAINT [UQ_giaovien_nguoidung] UNIQUE ([ma_nguoi_dung]),
    CONSTRAINT [fk_giaovien_nguoidung] FOREIGN KEY ([ma_nguoi_dung]) REFERENCES [dbo].[nguoi_dung] ([ma_nguoi_dung]) ON DELETE CASCADE,
    CONSTRAINT [ck_giaovien_gioitinh] CHECK ([gioi_tinh] IN (N'Nam', N'Nu', N'Khac'))
);
GO

-- 2.4 hoc_vien ------------------------------------------------------------------------
-- Chi con cac thuoc tinh RIENG cua ho so hoc vien (khong lap lai ho_ten/sdt/email/
-- trang thai hoat dong da co trong nguoi_dung).
CREATE TABLE [dbo].[hoc_vien]
(
    [ma_hoc_vien]      INT IDENTITY(1,1) NOT NULL,
    [ma_nguoi_dung]    INT NOT NULL,
    [gioi_tinh]        NVARCHAR(10)  NULL,
    [ngay_sinh]        DATE NULL,
    [dia_chi]          NVARCHAR(255) NULL,
    [ten_phu_huynh]    NVARCHAR(100) NULL,
    [sdt_phu_huynh]    NVARCHAR(20)  NULL,
    [ngay_nhap_hoc]    DATE NOT NULL CONSTRAINT [df_hocvien_ngaynhaphoc] DEFAULT (CONVERT(date, GETDATE())),
    CONSTRAINT [PK_hoc_vien] PRIMARY KEY ([ma_hoc_vien]),
    CONSTRAINT [UQ_hocvien_nguoidung] UNIQUE ([ma_nguoi_dung]),
    CONSTRAINT [fk_hocvien_nguoidung] FOREIGN KEY ([ma_nguoi_dung]) REFERENCES [dbo].[nguoi_dung] ([ma_nguoi_dung]) ON DELETE CASCADE,
    CONSTRAINT [ck_hocvien_gioitinh] CHECK ([gioi_tinh] IN (N'Nam', N'Nu', N'Khac'))
);
GO

-- 2.5 khoa_hoc --------------------------------------------------------------------------
CREATE TABLE [dbo].[khoa_hoc]
(
    [ma_khoa_hoc]     INT IDENTITY(1,1) NOT NULL,
    [ten_khoa_hoc]    NVARCHAR(150) NOT NULL,
    [ma_code]         NVARCHAR(20)  NULL,
    [trinh_do]        NVARCHAR(50)  NULL,
    [mo_ta]           NVARCHAR(MAX) NULL,
    [tong_so_buoi]    INT NULL,
    [so_tuan_hoc]     INT NULL,
    [hoc_phi_chuan]   DECIMAL(12,2) NULL,
    [dang_hoat_dong]  BIT NOT NULL CONSTRAINT [df_khoahoc_hoatdong] DEFAULT (1),
    CONSTRAINT [PK_khoa_hoc] PRIMARY KEY ([ma_khoa_hoc]),
    CONSTRAINT [UQ_khoahoc_macode] UNIQUE ([ma_code])
);
GO

-- 2.6 phong_hoc ---------------------------------------------------------------------
CREATE TABLE [dbo].[phong_hoc]
(
    [ma_phong]   INT IDENTITY(1,1) NOT NULL,
    [ten_phong]  NVARCHAR(50) NOT NULL,
    [suc_chua]   INT NULL,
    [vi_tri]     NVARCHAR(100) NULL,
    CONSTRAINT [PK_phong_hoc] PRIMARY KEY ([ma_phong])
);
GO

-- 2.7 lop_hoc -------------------------------------------------------------------------
CREATE TABLE [dbo].[lop_hoc]
(
    [ma_lop]         INT IDENTITY(1,1) NOT NULL,
    [ma_lop_code]    NVARCHAR(30)  NOT NULL,
    [ten_lop]        NVARCHAR(150) NOT NULL,
    [ma_khoa_hoc]    INT NOT NULL,
    [ma_giao_vien]   INT NULL,
    [ma_phong]       INT NULL,
    [avatar_url]     NVARCHAR(500) NULL,
    [ngay_bat_dau]   DATE NULL,
    [ngay_ket_thuc]  DATE NULL,
    [si_so_toi_da]   INT NOT NULL CONSTRAINT [df_lophoc_sisotoida] DEFAULT (20),
    [trang_thai]     NVARCHAR(20) NOT NULL CONSTRAINT [df_lophoc_trangthai] DEFAULT (N'SapKhaiGiang'),
    CONSTRAINT [PK_lop_hoc] PRIMARY KEY ([ma_lop]),
    CONSTRAINT [UQ_lophoc_macode] UNIQUE ([ma_lop_code]),
    CONSTRAINT [fk_lophoc_khoahoc] FOREIGN KEY ([ma_khoa_hoc]) REFERENCES [dbo].[khoa_hoc] ([ma_khoa_hoc]),
    CONSTRAINT [fk_lophoc_giaovien] FOREIGN KEY ([ma_giao_vien]) REFERENCES [dbo].[giao_vien] ([ma_giao_vien]),
    CONSTRAINT [fk_lophoc_phong] FOREIGN KEY ([ma_phong]) REFERENCES [dbo].[phong_hoc] ([ma_phong]),
    CONSTRAINT [ck_lophoc_trangthai] CHECK ([trang_thai] IN (N'SapKhaiGiang', N'DangHoc', N'DaKetThuc', N'Huy'))
);
GO
CREATE INDEX [idx_lophoc_khoahoc]  ON [dbo].[lop_hoc] ([ma_khoa_hoc]);
CREATE INDEX [idx_lophoc_giaovien] ON [dbo].[lop_hoc] ([ma_giao_vien]);
GO

-- 2.8 lich_hoc ------------------------------------------------------------------------
CREATE TABLE [dbo].[lich_hoc]
(
    [ma_lich]          INT IDENTITY(1,1) NOT NULL,
    [ma_lop]           INT NOT NULL,
    [thu_trong_tuan]   NVARCHAR(5) NOT NULL,
    [gio_bat_dau]      TIME(7) NOT NULL,
    [gio_ket_thuc]     TIME(7) NOT NULL,
    CONSTRAINT [PK_lich_hoc] PRIMARY KEY ([ma_lich]),
    CONSTRAINT [fk_lichhoc_lop] FOREIGN KEY ([ma_lop]) REFERENCES [dbo].[lop_hoc] ([ma_lop]) ON DELETE CASCADE,
    CONSTRAINT [ck_lichhoc_thu] CHECK ([thu_trong_tuan] IN (N'T2',N'T3',N'T4',N'T5',N'T6',N'T7',N'CN'))
);
GO
CREATE INDEX [idx_lichhoc_lop] ON [dbo].[lich_hoc] ([ma_lop]);
GO

-- 2.9 buoi_hoc ------------------------------------------------------------------------
CREATE TABLE [dbo].[buoi_hoc]
(
    [ma_buoi]        INT IDENTITY(1,1) NOT NULL,
    [ma_lop]         INT NOT NULL,
    [ngay_hoc]       DATE NOT NULL,
    [gio_bat_dau]    TIME(7) NULL,
    [gio_ket_thuc]   TIME(7) NULL,
    [noi_dung]       NVARCHAR(255) NULL,
    [bi_huy]         BIT NOT NULL CONSTRAINT [df_buoihoc_bihuy] DEFAULT (0),
    CONSTRAINT [PK_buoi_hoc] PRIMARY KEY ([ma_buoi]),
    CONSTRAINT [fk_buoihoc_lop] FOREIGN KEY ([ma_lop]) REFERENCES [dbo].[lop_hoc] ([ma_lop]) ON DELETE CASCADE
);
GO
CREATE INDEX [idx_buoihoc_lop] ON [dbo].[buoi_hoc] ([ma_lop]);
GO

-- 2.10 bai_tap (BANG MOI) --------------------------------------------------------------
CREATE TABLE [dbo].[bai_tap]
(
    [ma_bai_tap]      INT IDENTITY(1,1) NOT NULL,
    [ma_lop]          INT NOT NULL,
    [ma_buoi]         INT NULL,
    [ma_giao_vien]    INT NOT NULL,
    [tieu_de]         NVARCHAR(200) NOT NULL,
    [mo_ta]           NVARCHAR(MAX) NULL,
    [ngay_giao]       DATE NOT NULL CONSTRAINT [df_baitap_ngaygiao] DEFAULT (CONVERT(date, GETDATE())),
    [han_nop]         DATE NULL,
    [file_dinh_kem]   NVARCHAR(500) NULL,
    CONSTRAINT [PK_bai_tap] PRIMARY KEY ([ma_bai_tap]),
    CONSTRAINT [fk_baitap_lop] FOREIGN KEY ([ma_lop]) REFERENCES [dbo].[lop_hoc] ([ma_lop]) ON DELETE CASCADE,
    CONSTRAINT [fk_baitap_buoi] FOREIGN KEY ([ma_buoi]) REFERENCES [dbo].[buoi_hoc] ([ma_buoi]),
    CONSTRAINT [fk_baitap_giaovien] FOREIGN KEY ([ma_giao_vien]) REFERENCES [dbo].[giao_vien] ([ma_giao_vien])
);
GO
CREATE INDEX [idx_baitap_lop]  ON [dbo].[bai_tap] ([ma_lop]);
CREATE INDEX [idx_baitap_buoi] ON [dbo].[bai_tap] ([ma_buoi]);
GO

-- 2.11 dang_ky_hoc ----------------------------------------------------------------------
CREATE TABLE [dbo].[dang_ky_hoc]
(
    [ma_dang_ky]     INT IDENTITY(1,1) NOT NULL,
    [ma_hoc_vien]    INT NOT NULL,
    [ma_lop]         INT NOT NULL,
    [ngay_dang_ky]   DATE NOT NULL CONSTRAINT [df_dangky_ngay] DEFAULT (CONVERT(date, GETDATE())),
    [trang_thai]     NVARCHAR(20) NOT NULL CONSTRAINT [df_dangky_trangthai] DEFAULT (N'DangHoc'),
    CONSTRAINT [PK_dang_ky_hoc] PRIMARY KEY ([ma_dang_ky]),
    CONSTRAINT [uq_hocvien_lop] UNIQUE ([ma_hoc_vien], [ma_lop]),
    CONSTRAINT [fk_dangky_hocvien] FOREIGN KEY ([ma_hoc_vien]) REFERENCES [dbo].[hoc_vien] ([ma_hoc_vien]),
    CONSTRAINT [fk_dangky_lop] FOREIGN KEY ([ma_lop]) REFERENCES [dbo].[lop_hoc] ([ma_lop]),
    CONSTRAINT [ck_dangky_trangthai] CHECK ([trang_thai] IN (N'DangHoc', N'BaoLuu', N'DaHoanThanh', N'Huy'))
);
GO

-- 2.12 diem_danh ------------------------------------------------------------------------
CREATE TABLE [dbo].[diem_danh]
(
    [ma_diem_danh]   INT IDENTITY(1,1) NOT NULL,
    [ma_buoi]        INT NOT NULL,
    [ma_hoc_vien]    INT NOT NULL,
    [trang_thai]     NVARCHAR(20) NOT NULL CONSTRAINT [df_diemdanh_trangthai] DEFAULT (N'CoMat'),
    [ghi_chu]        NVARCHAR(255) NULL,
    CONSTRAINT [PK_diem_danh] PRIMARY KEY ([ma_diem_danh]),
    CONSTRAINT [uq_buoi_hocvien] UNIQUE ([ma_buoi], [ma_hoc_vien]),
    CONSTRAINT [fk_diemdanh_buoi] FOREIGN KEY ([ma_buoi]) REFERENCES [dbo].[buoi_hoc] ([ma_buoi]) ON DELETE CASCADE,
    CONSTRAINT [fk_diemdanh_hocvien] FOREIGN KEY ([ma_hoc_vien]) REFERENCES [dbo].[hoc_vien] ([ma_hoc_vien]),
    CONSTRAINT [ck_diemdanh_trangthai] CHECK ([trang_thai] IN (N'CoMat', N'Vang', N'TreCoPhep', N'VangCoPhep'))
);
GO

-- 2.13 ky_thi -------------------------------------------------------------------------
CREATE TABLE [dbo].[ky_thi]
(
    [ma_ky_thi]     INT IDENTITY(1,1) NOT NULL,
    [ma_lop]        INT NOT NULL,
    [ten_ky_thi]    NVARCHAR(150) NOT NULL,
    [ngay_thi]      DATE NULL,
    [loai_ky_thi]   NVARCHAR(20) NOT NULL CONSTRAINT [df_kythi_loai] DEFAULT (N'QuizNgan'),
    [diem_toi_da]   DECIMAL(5,2) NOT NULL CONSTRAINT [df_kythi_diemtoida] DEFAULT (10),
    CONSTRAINT [PK_ky_thi] PRIMARY KEY ([ma_ky_thi]),
    CONSTRAINT [fk_kythi_lop] FOREIGN KEY ([ma_lop]) REFERENCES [dbo].[lop_hoc] ([ma_lop]),
    CONSTRAINT [ck_kythi_loai] CHECK ([loai_ky_thi] IN (N'QuizNgan', N'GiuaKy', N'CuoiKy', N'Mock'))
);
GO

-- 2.14 diem_thi -----------------------------------------------------------------------
CREATE TABLE [dbo].[diem_thi]
(
    [ma_diem_thi]   INT IDENTITY(1,1) NOT NULL,
    [ma_ky_thi]     INT NOT NULL,
    [ma_hoc_vien]   INT NOT NULL,
    [diem_nghe]     DECIMAL(5,2) NULL,
    [diem_noi]      DECIMAL(5,2) NULL,
    [diem_doc]      DECIMAL(5,2) NULL,
    [diem_viet]     DECIMAL(5,2) NULL,
    [tong_diem]     DECIMAL(5,2) NULL,
    [nhan_xet]      NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_diem_thi] PRIMARY KEY ([ma_diem_thi]),
    CONSTRAINT [uq_kythi_hocvien] UNIQUE ([ma_ky_thi], [ma_hoc_vien]),
    CONSTRAINT [fk_diemthi_kythi] FOREIGN KEY ([ma_ky_thi]) REFERENCES [dbo].[ky_thi] ([ma_ky_thi]) ON DELETE CASCADE,
    CONSTRAINT [fk_diemthi_hocvien] FOREIGN KEY ([ma_hoc_vien]) REFERENCES [dbo].[hoc_vien] ([ma_hoc_vien])
);
GO

-- 2.15 diem (BANG MOI - diem bai tap) --------------------------------------------------
CREATE TABLE [dbo].[diem]
(
    [ma_diem]          INT IDENTITY(1,1) NOT NULL,
    [ma_bai_tap]       INT NOT NULL,
    [ma_hoc_vien]      INT NOT NULL,
    [diem_so]          DECIMAL(5,2) NULL,
    [trang_thai_nop]   NVARCHAR(20) NOT NULL CONSTRAINT [df_diem_trangthainop] DEFAULT (N'ChuaNop'),
    [ngay_nop]         DATETIME2(7) NULL,
    [nhan_xet]         NVARCHAR(500) NULL,
    CONSTRAINT [PK_diem] PRIMARY KEY ([ma_diem]),
    CONSTRAINT [uq_baitap_hocvien] UNIQUE ([ma_bai_tap], [ma_hoc_vien]),
    CONSTRAINT [fk_diem_baitap] FOREIGN KEY ([ma_bai_tap]) REFERENCES [dbo].[bai_tap] ([ma_bai_tap]) ON DELETE CASCADE,
    CONSTRAINT [fk_diem_hocvien] FOREIGN KEY ([ma_hoc_vien]) REFERENCES [dbo].[hoc_vien] ([ma_hoc_vien]),
    CONSTRAINT [ck_diem_trangthainop] CHECK ([trang_thai_nop] IN (N'ChuaNop', N'DaNop', N'NopTre'))
);
GO
CREATE INDEX [idx_diem_baitap]  ON [dbo].[diem] ([ma_bai_tap]);
CREATE INDEX [idx_diem_hocvien] ON [dbo].[diem] ([ma_hoc_vien]);
GO

-- 2.16 hoa_don ------------------------------------------------------------------------
CREATE TABLE [dbo].[hoa_don]
(
    [ma_hoa_don]       INT IDENTITY(1,1) NOT NULL,
    [ma_hoc_vien]      INT NOT NULL,
    [ma_lop]           INT NOT NULL,
    [tong_tien]        DECIMAL(12,2) NOT NULL,
    [giam_gia]         DECIMAL(12,2) NOT NULL CONSTRAINT [df_hoadon_giamgia] DEFAULT (0),
    [thanh_tien]       AS ([tong_tien] - [giam_gia]) PERSISTED,
    [ngay_lap]         DATE NOT NULL CONSTRAINT [df_hoadon_ngaylap] DEFAULT (CONVERT(date, GETDATE())),
    [han_thanh_toan]   DATE NULL,
    [trang_thai]       NVARCHAR(20) NOT NULL CONSTRAINT [df_hoadon_trangthai] DEFAULT (N'ChuaThanhToan'),
    CONSTRAINT [PK_hoa_don] PRIMARY KEY ([ma_hoa_don]),
    CONSTRAINT [fk_hoadon_hocvien] FOREIGN KEY ([ma_hoc_vien]) REFERENCES [dbo].[hoc_vien] ([ma_hoc_vien]),
    CONSTRAINT [fk_hoadon_lop] FOREIGN KEY ([ma_lop]) REFERENCES [dbo].[lop_hoc] ([ma_lop]),
    CONSTRAINT [ck_hoadon_trangthai] CHECK ([trang_thai] IN (N'ChuaThanhToan', N'DaThanhToan', N'ThanhToanMotPhan'))
);
GO
CREATE INDEX [idx_hoadon_hocvien] ON [dbo].[hoa_don] ([ma_hoc_vien]);
GO

-- 2.17 thanh_toan ---------------------------------------------------------------------
CREATE TABLE [dbo].[thanh_toan]
(
    [ma_thanh_toan]      INT IDENTITY(1,1) NOT NULL,
    [ma_hoa_don]         INT NOT NULL,
    [so_tien_da_tra]     DECIMAL(12,2) NOT NULL,
    [ngay_thanh_toan]    DATETIME2(7) NOT NULL CONSTRAINT [df_thanhtoan_ngay] DEFAULT (SYSDATETIME()),
    [hinh_thuc]          NVARCHAR(20) NOT NULL CONSTRAINT [df_thanhtoan_hinhthuc] DEFAULT (N'TienMat'),
    [ghi_chu]            NVARCHAR(255) NULL,
    CONSTRAINT [PK_thanh_toan] PRIMARY KEY ([ma_thanh_toan]),
    CONSTRAINT [fk_thanhtoan_hoadon] FOREIGN KEY ([ma_hoa_don]) REFERENCES [dbo].[hoa_don] ([ma_hoa_don]),
    CONSTRAINT [ck_thanhtoan_hinhthuc] CHECK ([hinh_thuc] IN (N'TienMat', N'ChuyenKhoan', N'The', N'ViDienTu'))
);
GO
CREATE INDEX [idx_thanhtoan_hoadon] ON [dbo].[thanh_toan] ([ma_hoa_don]);
GO

-- 2.18 thong_bao ------------------------------------------------------------------------
CREATE TABLE [dbo].[thong_bao]
(
    [ma_thong_bao]    INT IDENTITY(1,1) NOT NULL,
    [ma_nguoi_dung]   INT NOT NULL,
    [loai_thong_bao]  NVARCHAR(20) NOT NULL,
    [tieu_de]         NVARCHAR(255) NOT NULL,
    [noi_dung]        NVARCHAR(2000) NOT NULL,
    [ma_hoc_vien]     INT NULL,
    [ma_lop]          INT NULL,
    [ma_hoa_don]      INT NULL,
    [thoi_gian_tao]   DATETIME2(7) NOT NULL CONSTRAINT [df_thongbao_thoigiantao] DEFAULT (SYSDATETIME()),
    [da_doc]          BIT NOT NULL CONSTRAINT [df_thongbao_dadoc] DEFAULT (0),
    CONSTRAINT [PK_thong_bao] PRIMARY KEY ([ma_thong_bao]),
    CONSTRAINT [ck_thongbao_loai] CHECK ([loai_thong_bao] IN (N'diem_danh', N'diem_so', N'lich_hoc', N'hoc_phi', N'tin_nhan', N'he_thong')),
    CONSTRAINT [fk_thongbao_nguoidung] FOREIGN KEY ([ma_nguoi_dung]) REFERENCES [dbo].[nguoi_dung] ([ma_nguoi_dung]),
    CONSTRAINT [fk_thongbao_hocvien] FOREIGN KEY ([ma_hoc_vien]) REFERENCES [dbo].[hoc_vien] ([ma_hoc_vien]),
    CONSTRAINT [fk_thongbao_lop] FOREIGN KEY ([ma_lop]) REFERENCES [dbo].[lop_hoc] ([ma_lop]),
    CONSTRAINT [fk_thongbao_hoadon] FOREIGN KEY ([ma_hoa_don]) REFERENCES [dbo].[hoa_don] ([ma_hoa_don])
);
GO
CREATE INDEX [idx_thongbao_nguoidung] ON [dbo].[thong_bao] ([ma_nguoi_dung]);
CREATE INDEX [idx_thongbao_loai]      ON [dbo].[thong_bao] ([loai_thong_bao]);
CREATE INDEX [idx_thongbao_dadoc]     ON [dbo].[thong_bao] ([da_doc]);
GO

-- 2.19 tin_nhan -----------------------------------------------------------------------
CREATE TABLE [dbo].[tin_nhan]
(
    [ma_tin_nhan]      INT IDENTITY(1,1) NOT NULL,
    [ma_nguoi_gui]     INT NOT NULL,
    [ma_nguoi_nhan]    INT NOT NULL,
    [ma_hoc_vien]      INT NOT NULL,
    [noi_dung]         NVARCHAR(2000) NOT NULL,
    [thoi_gian_gui]    DATETIME2(7) NOT NULL CONSTRAINT [df_tinnhan_thoigiangui] DEFAULT (SYSDATETIME()),
    [da_doc]           BIT NOT NULL CONSTRAINT [df_tinnhan_dadoc] DEFAULT (0),
    CONSTRAINT [PK_tin_nhan] PRIMARY KEY ([ma_tin_nhan]),
    CONSTRAINT [fk_tinnhan_nguoigui] FOREIGN KEY ([ma_nguoi_gui]) REFERENCES [dbo].[nguoi_dung] ([ma_nguoi_dung]),
    CONSTRAINT [fk_tinnhan_nguoinhan] FOREIGN KEY ([ma_nguoi_nhan]) REFERENCES [dbo].[nguoi_dung] ([ma_nguoi_dung]),
    CONSTRAINT [fk_tinnhan_hocvien] FOREIGN KEY ([ma_hoc_vien]) REFERENCES [dbo].[hoc_vien] ([ma_hoc_vien])
);
GO
CREATE INDEX [idx_tinnhan_nguoigui]  ON [dbo].[tin_nhan] ([ma_nguoi_gui]);
CREATE INDEX [idx_tinnhan_nguoinhan] ON [dbo].[tin_nhan] ([ma_nguoi_nhan]);
CREATE INDEX [idx_tinnhan_hocvien]   ON [dbo].[tin_nhan] ([ma_hoc_vien]);
GO

/* =====================================================================================
   3. DU LIEU MAU - 10 ban ghi / bang
   Ghi chu: [nguoi_dung] can nhieu hon 10 dong vi day la bang duy nhat luu ho_ten/sdt/
   email cho CA 10 giao_vien lan 10 hoc_vien (moi giao_vien/hoc_vien bat buoc gan voi
   1 nguoi_dung rieng, khong dung chung) + vai tai khoan quan tri/le tan/ke toan.
   ===================================================================================== */

-- 3.1 vai_tro (10) ----------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[vai_tro] ON;
INSERT [dbo].[vai_tro] ([ma_vai_tro],[ten_vai_tro]) VALUES
(1, N'Admin'), (2, N'GiaoVien'), (3, N'HocVien'), (4, N'LeTan'), (5, N'KeToan'),
(6, N'QuanLy'), (7, N'TroGiang'), (8, N'CSKH'), (9, N'Marketing'), (10, N'PhuHuynh');
SET IDENTITY_INSERT [dbo].[vai_tro] OFF;
GO

-- 3.2 nguoi_dung (24 dong: 4 nhan vien + 10 giao vien + 10 hoc vien) --------------------
SET IDENTITY_INSERT [dbo].[nguoi_dung] ON;
INSERT [dbo].[nguoi_dung] ([ma_nguoi_dung],[ten_dang_nhap],[mat_khau_hash],[ho_ten],[email],[so_dien_thoai],[ma_vai_tro],[avatar_url],[dang_hoat_dong],[ngay_tao]) VALUES
(1,  N'admin01',      N'hashed_password_123', N'Quản Trị Viên',       N'admin@trungtam.com',       N'0999888777', 1, NULL, 1, CAST(N'2026-07-14T08:00:00' AS DATETIME2)),
(2,  N'letan01',      N'hashed_password_123', N'Đỗ Thị Hằng',          N'hangdt@trungtam.com',      N'0988111222', 4, NULL, 1, CAST(N'2026-07-14T08:05:00' AS DATETIME2)),
(3,  N'ketoan01',     N'hashed_password_123', N'Vũ Thị Lan',           N'lanvt@trungtam.com',       N'0977222333', 5, NULL, 1, CAST(N'2026-07-14T08:10:00' AS DATETIME2)),
(4,  N'quanly01',     N'hashed_password_123', N'Phạm Văn Hùng',        N'hungpv@trungtam.com',      N'0966333444', 6, NULL, 1, CAST(N'2026-07-14T08:15:00' AS DATETIME2)),
(5,  N'gv_thutran',   N'hashed_password_123', N'Trần Thị Thu',         N'thutran@trungtam.com',     N'0987654321', 2, NULL, 1, CAST(N'2026-07-14T16:05:48' AS DATETIME2)),
(6,  N'gv_hoangnam',  N'hashed_password_123', N'Nguyễn Hoàng Nam',     N'namnguyen@trungtam.com',   N'0977665544', 2, NULL, 1, CAST(N'2026-07-14T16:06:10' AS DATETIME2)),
(7,  N'gv_thanhha',   N'hashed_password_123', N'Lê Thanh Hà',          N'hale@trungtam.com',        N'0966554433', 2, NULL, 1, CAST(N'2026-07-15T09:00:00' AS DATETIME2)),
(8,  N'gv_minhtuan',  N'hashed_password_123', N'Phạm Minh Tuấn',       N'tuanpm@trungtam.com',      N'0955443322', 2, NULL, 1, CAST(N'2026-07-15T09:10:00' AS DATETIME2)),
(9,  N'gv_thuylinh',  N'hashed_password_123', N'Đặng Thùy Linh',       N'linhdang@trungtam.com',    N'0944332211', 2, NULL, 1, CAST(N'2026-07-16T10:00:00' AS DATETIME2)),
(10, N'gv_quocbao',   N'hashed_password_123', N'Ngô Quốc Bảo',         N'baongo@trungtam.com',      N'0933221100', 2, NULL, 1, CAST(N'2026-07-16T10:15:00' AS DATETIME2)),
(11, N'gv_kimngan',   N'hashed_password_123', N'Hoàng Kim Ngân',       N'nganhoang@trungtam.com',   N'0922110099', 2, NULL, 1, CAST(N'2026-07-17T11:00:00' AS DATETIME2)),
(12, N'gv_vandat',    N'hashed_password_123', N'Bùi Văn Đạt',          N'datbui@trungtam.com',      N'0911009988', 2, NULL, 1, CAST(N'2026-07-17T11:20:00' AS DATETIME2)),
(13, N'gv_ngocanh',   N'hashed_password_123', N'Trịnh Ngọc Anh',       N'anhtrinh@trungtam.com',    N'0900998877', 2, NULL, 1, CAST(N'2026-07-18T13:00:00' AS DATETIME2)),
(14, N'gv_thephong',  N'hashed_password_123', N'Lý Thế Phong',         N'phongly@trungtam.com',     N'0899887766', 2, NULL, 1, CAST(N'2026-07-18T13:30:00' AS DATETIME2)),
(15, N'hv_khachieu',  N'123456',              N'Lê Khắc Hiếu',         N'hieule@gmail.com',         N'0911222333', 3, NULL, 1, CAST(N'2026-08-16T08:20:00' AS DATETIME2)),
(16, N'hv_quynhanh',  N'123456',              N'Phạm Quỳnh Anh',       N'quynhanh@gmail.com',       N'0922333444', 3, NULL, 1, CAST(N'2026-08-16T08:25:00' AS DATETIME2)),
(17, N'hv_vanan',     N'123456',              N'Nguyễn Văn An',        N'nguyenvanan@gmail.com',    N'0912345678', 3, NULL, 1, CAST(N'2026-08-16T08:32:00' AS DATETIME2)),
(18, N'hv_thimai',    N'123456',              N'Trần Thị Mai',         N'tranthimai@gmail.com',     N'0923456789', 3, NULL, 1, CAST(N'2026-08-16T08:34:00' AS DATETIME2)),
(19, N'hv_minhduc',   N'123456',              N'Lê Minh Đức',          N'leminhduc@gmail.com',      N'0934567890', 3, NULL, 1, CAST(N'2026-08-16T08:40:00' AS DATETIME2)),
(20, N'hv_thuyduong', N'123456',              N'Phan Thùy Dương',      N'duongphan@gmail.com',      N'0945678901', 3, NULL, 1, CAST(N'2026-08-17T09:00:00' AS DATETIME2)),
(21, N'hv_congminh',  N'123456',              N'Vũ Công Minh',         N'minhvu@gmail.com',         N'0956789012', 3, NULL, 1, CAST(N'2026-08-17T09:10:00' AS DATETIME2)),
(22, N'hv_thanhtam',  N'123456',              N'Đỗ Thanh Tâm',         N'tamdo@gmail.com',          N'0967890123', 3, NULL, 1, CAST(N'2026-08-18T10:00:00' AS DATETIME2)),
(23, N'hv_gianguyen', N'123456',              N'Nguyễn Gia Nguyên',    N'nguyennguyen@gmail.com',   N'0978901234', 3, NULL, 1, CAST(N'2026-08-18T10:15:00' AS DATETIME2)),
(24, N'hv_baochau',   N'123456',              N'Trần Bảo Châu',        N'chautran@gmail.com',       N'0989012345', 3, NULL, 1, CAST(N'2026-08-19T11:00:00' AS DATETIME2));
SET IDENTITY_INSERT [dbo].[nguoi_dung] OFF;
GO

-- 3.3 giao_vien (10) ---------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[giao_vien] ON;
INSERT [dbo].[giao_vien] ([ma_giao_vien],[ma_nguoi_dung],[gioi_tinh],[ngay_sinh],[chuyen_mon],[ngay_vao_lam],[luong_theo_gio]) VALUES
(1,  5,  N'Nu',  CAST(N'1990-03-12' AS DATE), N'Luyện thi IELTS',            CAST(N'2021-06-01' AS DATE), 250000.00),
(2,  6,  N'Nam', CAST(N'1988-11-05' AS DATE), N'TOEIC',                     CAST(N'2020-01-15' AS DATE), 230000.00),
(3,  7,  N'Nu',  CAST(N'1992-07-20' AS DATE), N'Giao tiếp',                 CAST(N'2022-03-10' AS DATE), 220000.00),
(4,  8,  N'Nam', CAST(N'1985-02-18' AS DATE), N'Ngữ pháp - Luyện thi IELTS', CAST(N'2019-09-01' AS DATE), 260000.00),
(5,  9,  N'Nu',  CAST(N'1993-09-09' AS DATE), N'Tiếng Anh thiếu nhi',       CAST(N'2022-08-20' AS DATE), 200000.00),
(6,  10, N'Nam', CAST(N'1991-01-25' AS DATE), N'TOEIC - Giao tiếp',         CAST(N'2021-11-05' AS DATE), 230000.00),
(7,  11, N'Nu',  CAST(N'1994-04-14' AS DATE), N'IELTS Speaking',            CAST(N'2023-02-01' AS DATE), 240000.00),
(8,  12, N'Nam', CAST(N'1987-12-30' AS DATE), N'Ngữ pháp',                  CAST(N'2018-05-15' AS DATE), 250000.00),
(9,  13, N'Nu',  CAST(N'1995-06-22' AS DATE), N'Luyện thi VSTEP',           CAST(N'2023-07-01' AS DATE), 210000.00),
(10, 14, N'Nam', CAST(N'1989-10-08' AS DATE), N'IELTS Writing',             CAST(N'2020-04-10' AS DATE), 260000.00);
SET IDENTITY_INSERT [dbo].[giao_vien] OFF;
GO

-- 3.4 hoc_vien (10) -----------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[hoc_vien] ON;
INSERT [dbo].[hoc_vien] ([ma_hoc_vien],[ma_nguoi_dung],[gioi_tinh],[ngay_sinh],[dia_chi],[ten_phu_huynh],[sdt_phu_huynh],[ngay_nhap_hoc]) VALUES
(1,  15, N'Nam', CAST(N'2004-01-10' AS DATE), N'Hưng Yên',   N'Lê Văn Sơn',      N'0911222000', CAST(N'2026-07-01' AS DATE)),
(2,  16, N'Nu',  CAST(N'2005-05-15' AS DATE), N'Hà Nội',      N'Phạm Văn Long',   N'0922333000', CAST(N'2026-07-05' AS DATE)),
(3,  17, N'Nam', CAST(N'2005-05-15' AS DATE), N'Hưng Yên',   N'Nguyễn Văn Bình', N'0987654321', CAST(N'2026-08-01' AS DATE)),
(4,  18, N'Nu',  CAST(N'2006-08-20' AS DATE), N'Hà Nội',      N'Trần Văn Minh',   N'0978123456', CAST(N'2026-08-02' AS DATE)),
(5,  19, N'Nam', CAST(N'2005-12-10' AS DATE), N'Hải Dương',   N'Lê Văn Nam',      N'0967123456', CAST(N'2026-08-03' AS DATE)),
(6,  20, N'Nu',  CAST(N'2006-02-14' AS DATE), N'Bắc Ninh',    N'Phan Văn Đông',   N'0945000111', CAST(N'2026-08-05' AS DATE)),
(7,  21, N'Nam', CAST(N'2004-11-30' AS DATE), N'Hưng Yên',   N'Vũ Văn Kiên',     N'0956000222', CAST(N'2026-08-06' AS DATE)),
(8,  22, N'Nu',  CAST(N'2005-09-18' AS DATE), N'Hà Nội',      N'Đỗ Văn Phúc',     N'0967000333', CAST(N'2026-08-07' AS DATE)),
(9,  23, N'Nam', CAST(N'2006-06-25' AS DATE), N'Hải Phòng',   N'Nguyễn Văn Toàn', N'0978000444', CAST(N'2026-08-08' AS DATE)),
(10, 24, N'Nu',  CAST(N'2005-03-03' AS DATE), N'Hưng Yên',   N'Trần Văn Quang',  N'0989000555', CAST(N'2026-08-09' AS DATE));
SET IDENTITY_INSERT [dbo].[hoc_vien] OFF;
GO

-- 3.5 khoa_hoc (10) -----------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[khoa_hoc] ON;
INSERT [dbo].[khoa_hoc] ([ma_khoa_hoc],[ten_khoa_hoc],[ma_code],[trinh_do],[mo_ta],[tong_so_buoi],[so_tuan_hoc],[hoc_phi_chuan],[dang_hoat_dong]) VALUES
(1,  N'IELTS Foundation',        N'IELTS-FD',  N'IELTS 4.0-5.0', N'Khóa học IELTS nền tảng cho người mới bắt đầu', 40, 10, 6000000.00, 1),
(2,  N'IELTS Intermediate',      N'IELTS-INT', N'IELTS 5.0-6.0', N'Khóa nâng cao kỹ năng 4 kỹ năng IELTS',          40, 10, 6500000.00, 1),
(3,  N'IELTS Advanced',          N'IELTS-ADV', N'IELTS 6.5+',    N'Khóa luyện thi IELTS band điểm cao',             36, 9,  7500000.00, 1),
(4,  N'TOEIC 450+',              N'TOEIC-450', N'TOEIC Beginner',N'Khóa luyện thi TOEIC cho người mới',             30, 8,  4000000.00, 1),
(5,  N'TOEIC 650+',              N'TOEIC-650', N'TOEIC Intermediate', N'Khóa luyện thi TOEIC nâng cao',             30, 8,  4500000.00, 1),
(6,  N'Giao tiếp cơ bản',        N'GT-CB',     N'A1-A2',         N'Khóa giao tiếp tiếng Anh cơ bản hàng ngày',      24, 6,  3000000.00, 1),
(7,  N'Giao tiếp nâng cao',      N'GT-NC',     N'B1-B2',         N'Khóa giao tiếp phản xạ nâng cao',                24, 6,  3500000.00, 1),
(8,  N'Ngữ pháp căn bản',        N'NP-CB',     N'A1-B1',         N'Hệ thống hóa ngữ pháp tiếng Anh cơ bản',         20, 5,  2500000.00, 1),
(9,  N'Luyện thi VSTEP',         N'VSTEP-01',  N'B1-B2',         N'Khóa luyện thi chứng chỉ VSTEP',                 30, 8,  4800000.00, 1),
(10, N'Tiếng Anh thiếu nhi',     N'TA-TN',     N'6-11 tuổi',     N'Khóa tiếng Anh dành cho trẻ em',                 32, 8,  3200000.00, 1);
SET IDENTITY_INSERT [dbo].[khoa_hoc] OFF;
GO

-- 3.6 phong_hoc (10) ----------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[phong_hoc] ON;
INSERT [dbo].[phong_hoc] ([ma_phong],[ten_phong],[suc_chua],[vi_tri]) VALUES
(1,  N'P101 - IELTS',    22, N'Tầng 1'),
(2,  N'P102',            25, N'Tầng 1'),
(3,  N'P103',            20, N'Tầng 1'),
(4,  N'P201',            30, N'Tầng 2'),
(5,  N'P202',            20, N'Tầng 2'),
(6,  N'P203',            18, N'Tầng 2'),
(7,  N'Lab IELTS 01',    15, N'Tầng 3'),
(8,  N'Lab TOEIC 01',    15, N'Tầng 3'),
(9,  N'Phòng VIP 01',    10, N'Tầng 4'),
(10, N'Hội trường',      50, N'Tầng 1');
SET IDENTITY_INSERT [dbo].[phong_hoc] OFF;
GO

-- 3.7 lop_hoc (10) ------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[lop_hoc] ON;
INSERT [dbo].[lop_hoc] ([ma_lop],[ma_lop_code],[ten_lop],[ma_khoa_hoc],[ma_giao_vien],[ma_phong],[avatar_url],[ngay_bat_dau],[ngay_ket_thuc],[si_so_toi_da],[trang_thai]) VALUES
(1,  N'IELTS-FD-01',   N'IELTS Foundation 01',      1,  1,  1,  N'https://example.com/images/ielts-fd-01.jpg', CAST(N'2026-09-01' AS DATE), CAST(N'2026-11-30' AS DATE), 20, N'DangHoc'),
(2,  N'IELTS-INT-01',  N'IELTS Intermediate 01',    2,  4,  2,  NULL, CAST(N'2026-09-05' AS DATE), CAST(N'2026-11-25' AS DATE), 18, N'DangHoc'),
(3,  N'IELTS-ADV-01',  N'IELTS Advanced 01',        3,  7,  7,  N'https://example.com/images/ielts-adv-01.jpg', CAST(N'2026-09-10' AS DATE), CAST(N'2026-12-10' AS DATE), 15, N'SapKhaiGiang'),
(4,  N'TOEIC-450-01',  N'TOEIC 450+ 01',            4,  2,  8,  NULL, CAST(N'2026-09-02' AS DATE), CAST(N'2026-10-28' AS DATE), 20, N'DangHoc'),
(5,  N'TOEIC-650-01',  N'TOEIC 650+ 01',            5,  6,  4,  NULL, CAST(N'2026-09-15' AS DATE), CAST(N'2026-11-10' AS DATE), 18, N'SapKhaiGiang'),
(6,  N'GT-CB-01',      N'Giao tiếp cơ bản 01',      6,  3,  3,  NULL, CAST(N'2026-08-20' AS DATE), CAST(N'2026-10-01' AS DATE), 20, N'DangHoc'),
(7,  N'GT-NC-01',      N'Giao tiếp nâng cao 01',    7,  3,  5,  NULL, CAST(N'2026-09-20' AS DATE), CAST(N'2026-11-01' AS DATE), 18, N'SapKhaiGiang'),
(8,  N'NP-CB-01',      N'Ngữ pháp căn bản 01',      8,  8,  6,  NULL, CAST(N'2026-08-15' AS DATE), CAST(N'2026-09-19' AS DATE), 25, N'DangHoc'),
(9,  N'VSTEP-01-A',    N'Luyện thi VSTEP 01',       9,  9,  9,  NULL, CAST(N'2026-09-25' AS DATE), CAST(N'2026-12-01' AS DATE), 12, N'SapKhaiGiang'),
(10, N'TA-TN-01',      N'Tiếng Anh thiếu nhi 01',   10, 5,  10, N'https://example.com/images/ta-tn-01.jpg', CAST(N'2026-09-07' AS DATE), CAST(N'2026-11-30' AS DATE), 25, N'DangHoc');
SET IDENTITY_INSERT [dbo].[lop_hoc] OFF;
GO

-- 3.8 lich_hoc (10) -----------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[lich_hoc] ON;
INSERT [dbo].[lich_hoc] ([ma_lich],[ma_lop],[thu_trong_tuan],[gio_bat_dau],[gio_ket_thuc]) VALUES
(1,  1,  N'T2', CAST(N'18:00:00' AS TIME), CAST(N'20:00:00' AS TIME)),
(2,  1,  N'T4', CAST(N'18:00:00' AS TIME), CAST(N'20:00:00' AS TIME)),
(3,  2,  N'T3', CAST(N'18:00:00' AS TIME), CAST(N'20:00:00' AS TIME)),
(4,  2,  N'T5', CAST(N'18:00:00' AS TIME), CAST(N'20:00:00' AS TIME)),
(5,  3,  N'T7', CAST(N'09:00:00' AS TIME), CAST(N'11:00:00' AS TIME)),
(6,  4,  N'T2', CAST(N'19:00:00' AS TIME), CAST(N'21:00:00' AS TIME)),
(7,  5,  N'T4', CAST(N'19:00:00' AS TIME), CAST(N'21:00:00' AS TIME)),
(8,  6,  N'CN', CAST(N'08:00:00' AS TIME), CAST(N'10:00:00' AS TIME)),
(9,  8,  N'T3', CAST(N'17:30:00' AS TIME), CAST(N'19:30:00' AS TIME)),
(10, 10, N'T7', CAST(N'08:00:00' AS TIME), CAST(N'09:30:00' AS TIME));
SET IDENTITY_INSERT [dbo].[lich_hoc] OFF;
GO

-- 3.9 buoi_hoc (10) -----------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[buoi_hoc] ON;
INSERT [dbo].[buoi_hoc] ([ma_buoi],[ma_lop],[ngay_hoc],[gio_bat_dau],[gio_ket_thuc],[noi_dung],[bi_huy]) VALUES
(1,  1,  CAST(N'2026-09-01' AS DATE), CAST(N'18:00:00' AS TIME), CAST(N'20:00:00' AS TIME), N'Unit 1: Làm quen & Kiểm tra đầu vào', 0),
(2,  1,  CAST(N'2026-09-08' AS DATE), CAST(N'18:00:00' AS TIME), CAST(N'20:00:00' AS TIME), N'Unit 2: Listening Basics', 0),
(3,  2,  CAST(N'2026-09-05' AS DATE), CAST(N'18:00:00' AS TIME), CAST(N'20:00:00' AS TIME), N'Unit 1: Reading Skills', 0),
(4,  2,  CAST(N'2026-09-12' AS DATE), CAST(N'18:00:00' AS TIME), CAST(N'20:00:00' AS TIME), N'Unit 2: Grammar Review', 0),
(5,  4,  CAST(N'2026-09-02' AS DATE), CAST(N'19:00:00' AS TIME), CAST(N'21:00:00' AS TIME), N'Unit 1: Từ vựng TOEIC Part 5', 0),
(6,  4,  CAST(N'2026-09-09' AS DATE), CAST(N'19:00:00' AS TIME), CAST(N'21:00:00' AS TIME), N'Unit 2: Listening Part 1', 1),
(7,  6,  CAST(N'2026-08-20' AS DATE), CAST(N'08:00:00' AS TIME), CAST(N'10:00:00' AS TIME), N'Buổi 1: Làm quen & giới thiệu bản thân', 0),
(8,  8,  CAST(N'2026-08-15' AS DATE), CAST(N'17:30:00' AS TIME), CAST(N'19:30:00' AS TIME), N'Buổi 1: Thì hiện tại đơn', 0),
(9,  8,  CAST(N'2026-08-22' AS DATE), CAST(N'17:30:00' AS TIME), CAST(N'19:30:00' AS TIME), N'Buổi 2: Thì hiện tại tiếp diễn', 0),
(10, 10, CAST(N'2026-09-07' AS DATE), CAST(N'08:00:00' AS TIME), CAST(N'09:30:00' AS TIME), N'Buổi 1: Làm quen bảng chữ cái', 0);
SET IDENTITY_INSERT [dbo].[buoi_hoc] OFF;
GO

-- 3.10 bai_tap (10 - BANG MOI) ------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[bai_tap] ON;
INSERT [dbo].[bai_tap] ([ma_bai_tap],[ma_lop],[ma_buoi],[ma_giao_vien],[tieu_de],[mo_ta],[ngay_giao],[han_nop],[file_dinh_kem]) VALUES
(1,  1, 1,    1, N'Bài tập Unit 1 - Từ vựng',            N'Hoàn thành 20 câu điền từ vựng chủ đề gia đình.',       CAST(N'2026-09-01' AS DATE), CAST(N'2026-09-07' AS DATE), NULL),
(2,  1, 2,    1, N'Bài tập Unit 2 - Nghe hiểu',           N'Nghe file đính kèm và trả lời 10 câu hỏi.',             CAST(N'2026-09-08' AS DATE), CAST(N'2026-09-14' AS DATE), N'https://example.com/files/listening-unit2.mp3'),
(3,  2, 3,    4, N'Bài tập đọc hiểu - Reading Passage 1', N'Đọc đoạn văn và trả lời câu hỏi bên dưới.',              CAST(N'2026-09-05' AS DATE), CAST(N'2026-09-10' AS DATE), NULL),
(4,  2, 4,    4, N'Ôn tập ngữ pháp thì hiện tại',         N'Làm 30 câu bài tập ngữ pháp.',                          CAST(N'2026-09-12' AS DATE), CAST(N'2026-09-18' AS DATE), NULL),
(5,  4, 5,    2, N'Bài tập từ vựng TOEIC Part 5',         N'Học thuộc 50 từ vựng và làm bài kiểm tra nhanh.',       CAST(N'2026-09-02' AS DATE), CAST(N'2026-09-08' AS DATE), NULL),
(6,  4, NULL, 2, N'Luyện đề TOEIC mini test',             N'Làm đề thi thử 50 câu trong file đính kèm.',            CAST(N'2026-09-10' AS DATE), CAST(N'2026-09-16' AS DATE), N'https://example.com/files/toeic-minitest.pdf'),
(7,  6, 7,    3, N'Luyện nói theo chủ đề',                N'Ghi âm bài nói giới thiệu bản thân dài 2 phút.',        CAST(N'2026-08-20' AS DATE), CAST(N'2026-08-27' AS DATE), NULL),
(8,  8, 8,    8, N'Bài tập thì hiện tại đơn',             N'Hoàn thành phiếu bài tập thì hiện tại đơn.',            CAST(N'2026-08-15' AS DATE), CAST(N'2026-08-20' AS DATE), NULL),
(9,  8, 9,    8, N'Bài tập thì hiện tại tiếp diễn',       N'Viết 10 câu sử dụng thì hiện tại tiếp diễn.',           CAST(N'2026-08-22' AS DATE), CAST(N'2026-08-29' AS DATE), NULL),
(10, 10, 10,  5, N'Tô màu và viết chữ cái',               N'Hoàn thành phiếu tô màu bảng chữ cái tiếng Anh.',       CAST(N'2026-09-07' AS DATE), CAST(N'2026-09-14' AS DATE), NULL);
SET IDENTITY_INSERT [dbo].[bai_tap] OFF;
GO

-- 3.11 dang_ky_hoc (10) -------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[dang_ky_hoc] ON;
INSERT [dbo].[dang_ky_hoc] ([ma_dang_ky],[ma_hoc_vien],[ma_lop],[ngay_dang_ky],[trang_thai]) VALUES
(1,  1,  1,  CAST(N'2026-08-25' AS DATE), N'DangHoc'),
(2,  2,  1,  CAST(N'2026-08-25' AS DATE), N'DangHoc'),
(3,  3,  1,  CAST(N'2026-08-26' AS DATE), N'DangHoc'),
(4,  4,  2,  CAST(N'2026-08-27' AS DATE), N'DangHoc'),
(5,  5,  4,  CAST(N'2026-08-20' AS DATE), N'DangHoc'),
(6,  6,  4,  CAST(N'2026-08-21' AS DATE), N'DangHoc'),
(7,  7,  6,  CAST(N'2026-08-15' AS DATE), N'DangHoc'),
(8,  8,  8,  CAST(N'2026-08-10' AS DATE), N'DaHoanThanh'),
(9,  9,  9,  CAST(N'2026-08-28' AS DATE), N'DangHoc'),
(10, 10, 10, CAST(N'2026-08-30' AS DATE), N'DangHoc');
SET IDENTITY_INSERT [dbo].[dang_ky_hoc] OFF;
GO

-- 3.12 diem_danh (10) ---------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[diem_danh] ON;
INSERT [dbo].[diem_danh] ([ma_diem_danh],[ma_buoi],[ma_hoc_vien],[trang_thai],[ghi_chu]) VALUES
(1,  1, 1, N'CoMat',      NULL),
(2,  1, 2, N'CoMat',      NULL),
(3,  2, 1, N'VangCoPhep', N'Xin nghỉ có phép'),
(4,  2, 2, N'CoMat',      NULL),
(5,  3, 4, N'TreCoPhep',  N'Đến muộn 15 phút'),
(6,  4, 4, N'CoMat',      NULL),
(7,  5, 5, N'CoMat',      NULL),
(8,  5, 6, N'Vang',       N'Không rõ lý do'),
(9,  8, 8, N'CoMat',      NULL),
(10, 9, 8, N'CoMat',      NULL);
SET IDENTITY_INSERT [dbo].[diem_danh] OFF;
GO

-- 3.13 ky_thi (10) ------------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[ky_thi] ON;
INSERT [dbo].[ky_thi] ([ma_ky_thi],[ma_lop],[ten_ky_thi],[ngay_thi],[loai_ky_thi],[diem_toi_da]) VALUES
(1,  1, N'Mini Test 1',           CAST(N'2026-09-20' AS DATE), N'QuizNgan', 10.00),
(2,  1, N'Giữa kỳ',               CAST(N'2026-10-15' AS DATE), N'GiuaKy',   40.00),
(3,  2, N'Mini Test 1',           CAST(N'2026-09-25' AS DATE), N'QuizNgan', 10.00),
(4,  4, N'TOEIC Mock Test 1',     CAST(N'2026-09-20' AS DATE), N'Mock',     990.00),
(5,  4, N'Giữa kỳ',               CAST(N'2026-10-05' AS DATE), N'GiuaKy',   500.00),
(6,  6, N'Kiểm tra nói',          CAST(N'2026-09-10' AS DATE), N'QuizNgan', 10.00),
(7,  8, N'Kiểm tra ngữ pháp',     CAST(N'2026-09-05' AS DATE), N'QuizNgan', 10.00),
(8,  8, N'Cuối kỳ',               CAST(N'2026-09-19' AS DATE), N'CuoiKy',   100.00),
(9,  10, N'Kiểm tra từ vựng',     CAST(N'2026-09-25' AS DATE), N'QuizNgan', 10.00),
(10, 2, N'Cuối kỳ',               CAST(N'2026-11-20' AS DATE), N'CuoiKy',   40.00);
SET IDENTITY_INSERT [dbo].[ky_thi] OFF;
GO

-- 3.14 diem_thi (10) ----------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[diem_thi] ON;
INSERT [dbo].[diem_thi] ([ma_diem_thi],[ma_ky_thi],[ma_hoc_vien],[diem_nghe],[diem_noi],[diem_doc],[diem_viet],[tong_diem],[nhan_xet]) VALUES
(1,  1, 1, 5.50, 6.00, 5.00, 5.50, 5.50,  N'Cần cải thiện phần đọc'),
(2,  1, 2, 6.00, 6.50, 6.00, 6.00, 6.10,  N'Tiến bộ tốt'),
(3,  3, 4, 6.50, 7.00, 6.50, 6.00, 6.50,  NULL),
(4,  4, 5, NULL, NULL, NULL, NULL, 450.00, N'Điểm bài thi thử TOEIC'),
(5,  4, 6, NULL, NULL, NULL, NULL, 400.00, NULL),
(6,  6, 7, NULL, 7.50, NULL, NULL, 7.50,  N'Phát âm tốt, cần tự tin hơn'),
(7,  7, 8, NULL, NULL, NULL, NULL, 8.50,  N'Nắm chắc ngữ pháp cơ bản'),
(8,  8, 8, NULL, NULL, NULL, NULL, 85.00, N'Hoàn thành tốt bài kiểm tra cuối kỳ'),
(9,  9, 10, NULL, NULL, NULL, NULL, 9.00, N'Nhớ tốt từ vựng'),
(10, 2, 1, 6.00, 6.00, 5.50, 5.50, 5.75,  N'Cần luyện thêm kỹ năng viết');
SET IDENTITY_INSERT [dbo].[diem_thi] OFF;
GO

-- 3.15 diem (10 - BANG MOI, diem bai tap) --------------------------------------------------
SET IDENTITY_INSERT [dbo].[diem] ON;
INSERT [dbo].[diem] ([ma_diem],[ma_bai_tap],[ma_hoc_vien],[diem_so],[trang_thai_nop],[ngay_nop],[nhan_xet]) VALUES
(1,  1, 1, 8.50,  N'DaNop',   CAST(N'2026-09-06T20:00:00' AS DATETIME2), N'Làm tốt'),
(2,  1, 2, 9.00,  N'DaNop',   CAST(N'2026-09-05T19:30:00' AS DATETIME2), N'Xuất sắc'),
(3,  2, 1, 7.00,  N'NopTre',  CAST(N'2026-09-15T08:00:00' AS DATETIME2), N'Nộp trễ 1 ngày'),
(4,  3, 4, 8.00,  N'DaNop',   CAST(N'2026-09-09T21:00:00' AS DATETIME2), NULL),
(5,  4, 4, NULL,  N'ChuaNop', NULL, NULL),
(6,  5, 5, 9.50,  N'DaNop',   CAST(N'2026-09-07T22:00:00' AS DATETIME2), N'Rất tốt'),
(7,  6, 6, 6.50,  N'DaNop',   CAST(N'2026-09-15T10:00:00' AS DATETIME2), N'Cần ôn thêm Part 5'),
(8,  8, 8, 10.00, N'DaNop',   CAST(N'2026-08-19T18:00:00' AS DATETIME2), N'Hoàn hảo'),
(9,  9, 8, 8.00,  N'DaNop',   CAST(N'2026-08-28T19:00:00' AS DATETIME2), NULL),
(10, 10, 10, 7.50, N'DaNop',  CAST(N'2026-09-13T09:00:00' AS DATETIME2), N'Tô màu đẹp');
SET IDENTITY_INSERT [dbo].[diem] OFF;
GO

-- 3.16 hoa_don (10) -----------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[hoa_don] ON;
INSERT [dbo].[hoa_don] ([ma_hoa_don],[ma_hoc_vien],[ma_lop],[tong_tien],[giam_gia],[ngay_lap],[han_thanh_toan],[trang_thai]) VALUES
(1,  1,  1,  6000000.00, 0.00,      CAST(N'2026-08-25' AS DATE), CAST(N'2026-09-25' AS DATE), N'ChuaThanhToan'),
(2,  2,  1,  6000000.00, 300000.00, CAST(N'2026-08-25' AS DATE), CAST(N'2026-09-25' AS DATE), N'DaThanhToan'),
(3,  3,  1,  6000000.00, 0.00,      CAST(N'2026-08-26' AS DATE), CAST(N'2026-09-26' AS DATE), N'ThanhToanMotPhan'),
(4,  4,  2,  6500000.00, 0.00,      CAST(N'2026-08-27' AS DATE), CAST(N'2026-09-27' AS DATE), N'DaThanhToan'),
(5,  5,  4,  4000000.00, 0.00,      CAST(N'2026-08-20' AS DATE), CAST(N'2026-09-20' AS DATE), N'DaThanhToan'),
(6,  6,  4,  4000000.00, 200000.00, CAST(N'2026-08-21' AS DATE), CAST(N'2026-09-21' AS DATE), N'ChuaThanhToan'),
(7,  7,  6,  3000000.00, 0.00,      CAST(N'2026-08-15' AS DATE), CAST(N'2026-09-15' AS DATE), N'DaThanhToan'),
(8,  8,  8,  2500000.00, 0.00,      CAST(N'2026-08-10' AS DATE), CAST(N'2026-09-10' AS DATE), N'DaThanhToan'),
(9,  9,  9,  4800000.00, 0.00,      CAST(N'2026-08-28' AS DATE), CAST(N'2026-09-28' AS DATE), N'ChuaThanhToan'),
(10, 10, 10, 3200000.00, 100000.00, CAST(N'2026-08-30' AS DATE), CAST(N'2026-09-30' AS DATE), N'ThanhToanMotPhan');
SET IDENTITY_INSERT [dbo].[hoa_don] OFF;
GO

-- 3.17 thanh_toan (10) --------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[thanh_toan] ON;
INSERT [dbo].[thanh_toan] ([ma_thanh_toan],[ma_hoa_don],[so_tien_da_tra],[ngay_thanh_toan],[hinh_thuc],[ghi_chu]) VALUES
(1,  2,  5700000.00, CAST(N'2026-08-26T10:00:00' AS DATETIME2), N'ChuyenKhoan', N'Thanh toán đủ'),
(2,  3,  3000000.00, CAST(N'2026-08-27T09:00:00' AS DATETIME2), N'TienMat',     N'Thanh toán một phần'),
(3,  4,  6500000.00, CAST(N'2026-08-28T14:00:00' AS DATETIME2), N'ChuyenKhoan', NULL),
(4,  5,  4000000.00, CAST(N'2026-08-21T08:30:00' AS DATETIME2), N'The',         NULL),
(5,  7,  3000000.00, CAST(N'2026-08-16T15:00:00' AS DATETIME2), N'TienMat',     NULL),
(6,  8,  2500000.00, CAST(N'2026-08-11T11:00:00' AS DATETIME2), N'ViDienTu',    NULL),
(7,  10, 3100000.00, CAST(N'2026-08-31T16:00:00' AS DATETIME2), N'ChuyenKhoan', N'Thanh toán một phần'),
(8,  1,  3000000.00, CAST(N'2026-09-01T10:00:00' AS DATETIME2), N'TienMat',     N'Đặt cọc lần 1'),
(9,  3,  3000000.00, CAST(N'2026-09-05T10:00:00' AS DATETIME2), N'ChuyenKhoan', N'Thanh toán phần còn lại'),
(10, 9,  2000000.00, CAST(N'2026-09-02T09:00:00' AS DATETIME2), N'The',         N'Đặt cọc');
SET IDENTITY_INSERT [dbo].[thanh_toan] OFF;
GO

-- 3.18 thong_bao (10) ---------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[thong_bao] ON;
INSERT [dbo].[thong_bao] ([ma_thong_bao],[ma_nguoi_dung],[loai_thong_bao],[tieu_de],[noi_dung],[ma_hoc_vien],[ma_lop],[ma_hoa_don],[thoi_gian_tao],[da_doc]) VALUES
(1,  16, N'hoc_phi',   N'Nhắc nhở thanh toán học phí',       N'Hóa đơn học phí lớp IELTS Foundation 01 (6.000.000 VNĐ) của bạn sắp đến hạn ngày 25/09/2026. Vui lòng thanh toán đúng hạn.', 2,  1,  1,    CAST(N'2026-08-26T09:00:00' AS DATETIME2), 0),
(2,  17, N'diem_so',   N'Kết quả thi Mini Test 1',            N'Bạn đã có điểm bài thi Mini Test 1 lớp IELTS Foundation 01. Điểm tổng kết: 5.75.',                                               3,  1,  NULL, CAST(N'2026-09-21T10:00:00' AS DATETIME2), 1),
(3,  18, N'diem_danh', N'Cập nhật trạng thái điểm danh',      N'Buổi học ngày 12/09/2026 lớp IELTS Intermediate 01 bạn được ghi nhận trạng thái: Có mặt.',                                        4,  2,  NULL, CAST(N'2026-09-06T20:00:00' AS DATETIME2), 0),
(4,  19, N'lich_hoc',  N'Thông báo hủy buổi học',             N'Buổi học Unit 2 ngày 09/09/2026 lớp TOEIC 450+ 01 tạm thời bị hủy do sự cố kỹ thuật. Trung tâm sẽ xếp lịch học bù sau.',           5,  4,  NULL, CAST(N'2026-09-09T14:00:00' AS DATETIME2), 0),
(5,  5,  N'tin_nhan',  N'Bạn có tin nhắn mới',                N'Học viên Lê Khắc Hiếu vừa gửi tin nhắn hỏi về lịch khai giảng lớp.',                                                              1,  1,  NULL, CAST(N'2026-08-25T08:31:00' AS DATETIME2), 1),
(6,  1,  N'he_thong',  N'Sao lưu dữ liệu định kỳ thành công', N'Hệ thống vừa hoàn tất quá trình sao lưu tự động cơ sở dữ liệu vào lúc 02:00 AM.',                                                 NULL, NULL, NULL, CAST(N'2026-09-03T02:00:00' AS DATETIME2), 1),
(7,  16, N'he_thong',  N'Lịch bảo trì nâng cấp ứng dụng',     N'Cổng thông tin học viên sẽ tạm dừng bảo trì vào 23:00 ngày 06/09/2026 để nâng cấp tính năng mới.',                                NULL, NULL, NULL, CAST(N'2026-09-04T08:00:00' AS DATETIME2), 0),
(8,  6,  N'lich_hoc',  N'Lịch dạy tuần mới',                  N'Bạn được phân công dạy bổ sung 1 buổi lớp TOEIC 450+ 01 vào tuần tới, vui lòng kiểm tra lịch.',                                    NULL, 4,  NULL, CAST(N'2026-09-05T07:00:00' AS DATETIME2), 0),
(9,  20, N'diem_so',   N'Kết quả bài tập',                    N'Bài tập "Luyện đề TOEIC mini test" của bạn đã được chấm điểm: 6.50.',                                                            6,  4,  NULL, CAST(N'2026-09-16T11:00:00' AS DATETIME2), 0),
(10, 24, N'hoc_phi',   N'Nhắc nhở thanh toán học phí',        N'Hóa đơn học phí lớp Tiếng Anh thiếu nhi 01 (3.200.000 VNĐ) của bạn còn thiếu 100.000 VNĐ, hạn thanh toán 30/09/2026.',              10, 10, 10,   CAST(N'2026-08-31T09:00:00' AS DATETIME2), 0);
SET IDENTITY_INSERT [dbo].[thong_bao] OFF;
GO

-- 3.19 tin_nhan (10) ----------------------------------------------------------------------
SET IDENTITY_INSERT [dbo].[tin_nhan] ON;
INSERT [dbo].[tin_nhan] ([ma_tin_nhan],[ma_nguoi_gui],[ma_nguoi_nhan],[ma_hoc_vien],[noi_dung],[thoi_gian_gui],[da_doc]) VALUES
(1,  15, 1,  1,  N'Chào trung tâm, em muốn hỏi lớp IELTS Foundation 01 của em bao giờ khai giảng ạ?', CAST(N'2026-08-25T08:30:00' AS DATETIME2), 1),
(2,  1,  15, 1,  N'Chào em, lớp IELTS Foundation 01 sẽ khai giảng vào ngày 01/09/2026 nhé.',           CAST(N'2026-08-25T08:35:00' AS DATETIME2), 1),
(3,  17, 1,  3,  N'Dạ, học phí lớp em đã đóng đủ chưa ạ?',                                              CAST(N'2026-08-26T09:10:00' AS DATETIME2), 1),
(4,  1,  17, 3,  N'Lớp em đang ở trạng thái thanh toán một phần, còn thiếu 3.000.000đ nhé.',           CAST(N'2026-08-26T09:20:00' AS DATETIME2), 0),
(5,  19, 6,  5,  N'Em chào thầy, em xin phép nghỉ buổi học hôm nay vì có việc đột xuất ạ.',            CAST(N'2026-09-08T18:50:00' AS DATETIME2), 1),
(6,  6,  19, 5,  N'Thầy đã nhận được thông tin, em nhớ mượn vở bạn ghi bài nhé.',                       CAST(N'2026-09-08T19:05:00' AS DATETIME2), 0),
(7,  22, 1,  8,  N'Admin kiểm tra giúp em hóa đơn học phí với ạ.',                                      CAST(N'2026-08-10T09:00:00' AS DATETIME2), 1),
(8,  1,  22, 8,  N'Hóa đơn của em đã thanh toán đủ rồi nhé.',                                           CAST(N'2026-08-10T09:30:00' AS DATETIME2), 1),
(9,  24, 9,  10, N'Cô ơi bài tập tô màu em nộp muộn được không ạ?',                                    CAST(N'2026-09-13T20:00:00' AS DATETIME2), 1),
(10, 9,  24, 10, N'Được nhé, nhớ nộp trước thứ 6 nha.',                                                 CAST(N'2026-09-13T20:10:00' AS DATETIME2), 0);
SET IDENTITY_INSERT [dbo].[tin_nhan] OFF;
GO

/* =====================================================================================
   4. KIEM TRA NHANH
   ===================================================================================== */
SELECT 'vai_tro' AS bang, COUNT(*) AS so_dong FROM [dbo].[vai_tro]
UNION ALL SELECT 'nguoi_dung', COUNT(*) FROM [dbo].[nguoi_dung]
UNION ALL SELECT 'giao_vien', COUNT(*) FROM [dbo].[giao_vien]
UNION ALL SELECT 'hoc_vien', COUNT(*) FROM [dbo].[hoc_vien]
UNION ALL SELECT 'khoa_hoc', COUNT(*) FROM [dbo].[khoa_hoc]
UNION ALL SELECT 'phong_hoc', COUNT(*) FROM [dbo].[phong_hoc]
UNION ALL SELECT 'lop_hoc', COUNT(*) FROM [dbo].[lop_hoc]
UNION ALL SELECT 'lich_hoc', COUNT(*) FROM [dbo].[lich_hoc]
UNION ALL SELECT 'buoi_hoc', COUNT(*) FROM [dbo].[buoi_hoc]
UNION ALL SELECT 'bai_tap', COUNT(*) FROM [dbo].[bai_tap]
UNION ALL SELECT 'dang_ky_hoc', COUNT(*) FROM [dbo].[dang_ky_hoc]
UNION ALL SELECT 'diem_danh', COUNT(*) FROM [dbo].[diem_danh]
UNION ALL SELECT 'ky_thi', COUNT(*) FROM [dbo].[ky_thi]
UNION ALL SELECT 'diem_thi', COUNT(*) FROM [dbo].[diem_thi]
UNION ALL SELECT 'diem', COUNT(*) FROM [dbo].[diem]
UNION ALL SELECT 'hoa_don', COUNT(*) FROM [dbo].[hoa_don]
UNION ALL SELECT 'thanh_toan', COUNT(*) FROM [dbo].[thanh_toan]
UNION ALL SELECT 'thong_bao', COUNT(*) FROM [dbo].[thong_bao]
UNION ALL SELECT 'tin_nhan', COUNT(*) FROM [dbo].[tin_nhan];
GO