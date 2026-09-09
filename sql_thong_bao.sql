USE [QuanLyLopTiengAnh];
GO

IF OBJECT_ID(N'dbo.thong_bao', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[thong_bao]
    (
        [ma_thong_bao] INT IDENTITY(1,1) NOT NULL,
        [ma_nguoi_dung] INT NOT NULL,
        [loai_thong_bao] NVARCHAR(20) NOT NULL,
        [tieu_de] NVARCHAR(255) NOT NULL,
        [noi_dung] NVARCHAR(2000) NOT NULL,
        [ma_hoc_vien] INT NULL,
        [ma_lop] INT NULL,
        [ma_hoa_don] INT NULL,
        [thoi_gian_tao] DATETIME2 NOT NULL CONSTRAINT [df_thongbao_thoi_gian_tao] DEFAULT (SYSDATETIME()),
        [da_doc] BIT NOT NULL CONSTRAINT [df_thongbao_da_doc] DEFAULT (0),
        CONSTRAINT [PK__thong_bao] PRIMARY KEY ([ma_thong_bao]),
        CONSTRAINT [ck_thongbao_loai] CHECK ([loai_thong_bao] IN
            (N'diem_danh', N'diem_so', N'lich_hoc', N'hoc_phi', N'tin_nhan', N'he_thong')),
        CONSTRAINT [fk_thongbao_nguoi_dung] FOREIGN KEY ([ma_nguoi_dung]) REFERENCES [dbo].[nguoi_dung] ([ma_nguoi_dung]),
        CONSTRAINT [fk_thongbao_hoc_vien] FOREIGN KEY ([ma_hoc_vien]) REFERENCES [dbo].[hoc_vien] ([ma_hoc_vien]),
        CONSTRAINT [fk_thongbao_lop] FOREIGN KEY ([ma_lop]) REFERENCES [dbo].[lop_hoc] ([ma_lop]),
        CONSTRAINT [fk_thongbao_hoa_don] FOREIGN KEY ([ma_hoa_don]) REFERENCES [dbo].[hoa_don] ([ma_hoa_don])
    );

    CREATE INDEX [idx_thongbao_nguoi_dung] ON [dbo].[thong_bao] ([ma_nguoi_dung]);
    CREATE INDEX [idx_thongbao_loai] ON [dbo].[thong_bao] ([loai_thong_bao]);
    CREATE INDEX [idx_thongbao_da_doc] ON [dbo].[thong_bao] ([da_doc]);
END;
GO

USE [QuanLyLopTiengAnh];
GO

-- Xóa dữ liệu cũ nếu có
DELETE FROM [dbo].[thong_bao];
GO

SET IDENTITY_INSERT [dbo].[thong_bao] ON;
GO

-- ===================================================================================
-- 1. THÔNG BÁO CHO HỌC VIÊN PHẠM QUỲNH ANH (ma_nguoi_dung = 7, ma_hoc_vien = 5)
-- ===================================================================================
-- Thông báo học phí (liên kết hóa đơn 4, lớp 3)
INSERT [dbo].[thong_bao] ([ma_thong_bao], [ma_nguoi_dung], [loai_thong_bao], [tieu_de], [noi_dung], [ma_hoc_vien], [ma_lop], [ma_hoa_don], [thoi_gian_tao], [da_doc])
VALUES (1, 7, N'hoc_phi', N'Nhắc nhở thanh toán học phí', N'Hóa đơn học phí lớp IELTS Basic (6.000.000 VNĐ) của bạn sắp đến hạn ngày 05/10/2023. Vui lòng thanh toán đúng hạn.', 5, 3, 4, CAST(N'2026-08-20T09:00:00' AS DateTime2), 1);

-- Thông báo điểm số (liên kết lớp 3)
INSERT [dbo].[thong_bao] ([ma_thong_bao], [ma_nguoi_dung], [loai_thong_bao], [tieu_de], [noi_dung], [ma_hoc_vien], [ma_lop], [ma_hoa_don], [thoi_gian_tao], [da_doc])
VALUES (2, 7, N'diem_so', N'Kết quả thi Mini Test 1', N'Bạn đã có điểm bài thi Mini Test 1 lớp IELTS Basic. Điểm tổng kết: 5.50 (Speaking: 6.0, Listening: 5.0).', 5, 3, NULL, CAST(N'2026-08-21T10:30:00' AS DateTime2), 1);

-- Thông báo điểm danh (liên kết lớp 3)
INSERT [dbo].[thong_bao] ([ma_thong_bao], [ma_nguoi_dung], [loai_thong_bao], [tieu_de], [noi_dung], [ma_hoc_vien], [ma_lop], [ma_hoa_don], [thoi_gian_tao], [da_doc])
VALUES (3, 7, N'diem_danh', N'Cập nhật trạng thái điểm danh', N'Buổi học ngày 02/10/2023 lớp IELTS Basic bạn được ghi nhận trạng thái: Vắng có phép.', 5, 3, NULL, CAST(N'2026-08-22T20:15:00' AS DateTime2), 0);

-- ===================================================================================
-- 2. THÔNG BÁO CHO HỌC VIÊN NGUYỄN VĂN AN (ma_nguoi_dung = 11, ma_hoc_vien = 6)
-- ===================================================================================
-- Thông báo lịch học (liên kết lớp 4 - có buổi học bị hủy)
INSERT [dbo].[thong_bao] ([ma_thong_bao], [ma_nguoi_dung], [loai_thong_bao], [tieu_de], [noi_dung], [ma_hoc_vien], [ma_lop], [ma_hoa_don], [thoi_gian_tao], [da_doc])
VALUES (4, 11, N'lich_hoc', N'Thông báo hủy buổi học', N'Buổi học Unit 3 ngày 05/09/2026 lớp TOEIC 500+ tạm thời bị hủy do sự cố kỹ thuật. Trung tâm sẽ xếp lịch học bù sau.', 6, 4, NULL, CAST(N'2026-08-23T14:00:00' AS DateTime2), 0);

-- ===================================================================================
-- 3. THÔNG BÁO CHO GIÁO VIÊN TRẦN THỊ THU (ma_nguoi_dung = 4)
-- ===================================================================================
-- Thông báo tin nhắn mới
INSERT [dbo].[thong_bao] ([ma_thong_bao], [ma_nguoi_dung], [loai_thong_bao], [tieu_de], [noi_dung], [ma_hoc_vien], [ma_lop], [ma_hoa_don], [thoi_gian_tao], [da_doc])
VALUES (5, 4, N'tin_nhan', N'Bạn có tin nhắn mới', N'Học viên Nguyễn Văn An vừa gửi tin nhắn xin phép nghỉ học cho bạn.', 6, 4, NULL, CAST(N'2026-08-26T15:01:00' AS DateTime2), 1);

-- ===================================================================================
-- 4. THÔNG BÁO HỆ THỐNG GỬI CHUNG CHO QUẢN TRỊ VIÊN & TẤT CẢ USER
-- ===================================================================================
-- Thông báo hệ thống gửi Admin (ma_nguoi_dung = 3)
INSERT [dbo].[thong_bao] ([ma_thong_bao], [ma_nguoi_dung], [loai_thong_bao], [tieu_de], [noi_dung], [ma_hoc_vien], [ma_lop], [ma_hoa_don], [thoi_gian_tao], [da_doc])
VALUES (6, 3, N'he_thong', N'Sao lưu dữ liệu định kỳ thành công', N'Hệ thống vừa hoàn tất quá trình sao lưu tự động cơ sở dữ liệu vào lúc 02:00 AM.', NULL, NULL, NULL, CAST(N'2026-08-27T02:00:00' AS DateTime2), 1);

-- Thông báo bảo trì hệ thống gửi cho Học viên Quỳnh Anh (ma_nguoi_dung = 7)
INSERT [dbo].[thong_bao] ([ma_thong_bao], [ma_nguoi_dung], [loai_thong_bao], [tieu_de], [noi_dung], [ma_hoc_vien], [ma_lop], [ma_hoa_don], [thoi_gian_tao], [da_doc])
VALUES (7, 7, N'he_thong', N'Lịch bảo trì nâng cấp ứng dụng', N'Cổng thông tin học viên sẽ tạm dừng bảo trì vào 23:00 ngày 31/08 để nâng cấp tính năng mới.', NULL, NULL, NULL, CAST(N'2026-08-27T08:00:00' AS DateTime2), 0);

GO
SET IDENTITY_INSERT [dbo].[thong_bao] OFF;
GO

-- Kiểm tra kết quả
SELECT * FROM [dbo].[thong_bao];