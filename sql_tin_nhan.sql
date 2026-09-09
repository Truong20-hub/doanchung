USE [QuanLyLopTiengAnh];
GO

IF OBJECT_ID(N'dbo.tin_nhan', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tin_nhan]
    (
        [ma_tin_nhan] INT IDENTITY(1,1) NOT NULL,
        [ma_nguoi_gui] INT NOT NULL,
        [ma_nguoi_nhan] INT NOT NULL,
        [ma_hoc_vien] INT NOT NULL,
        [noi_dung] NVARCHAR(2000) NOT NULL,
        [thoi_gian_gui] DATETIME2 NOT NULL CONSTRAINT [df_tinnhan_thoi_gian_gui] DEFAULT (SYSDATETIME()),
        [da_doc] BIT NOT NULL CONSTRAINT [df_tinnhan_da_doc] DEFAULT (0),
        CONSTRAINT [PK__tin_nhan] PRIMARY KEY ([ma_tin_nhan]),
        CONSTRAINT [fk_tinnhan_nguoi_gui] FOREIGN KEY ([ma_nguoi_gui]) REFERENCES [dbo].[nguoi_dung] ([ma_nguoi_dung]),
        CONSTRAINT [fk_tinnhan_nguoi_nhan] FOREIGN KEY ([ma_nguoi_nhan]) REFERENCES [dbo].[nguoi_dung] ([ma_nguoi_dung]),
        CONSTRAINT [fk_tinnhan_hoc_vien] FOREIGN KEY ([ma_hoc_vien]) REFERENCES [dbo].[hoc_vien] ([ma_hoc_vien])
    );

    CREATE INDEX [idx_tinnhan_nguoi_gui] ON [dbo].[tin_nhan] ([ma_nguoi_gui]);
    CREATE INDEX [idx_tinnhan_nguoi_nhan] ON [dbo].[tin_nhan] ([ma_nguoi_nhan]);
    CREATE INDEX [idx_tinnhan_hoc_vien] ON [dbo].[tin_nhan] ([ma_hoc_vien]);
END;
GO

USE [QuanLyLopTiengAnh];
GO

-- Xóa dữ liệu cũ nếu có để tránh trùng lặp
DELETE FROM [dbo].[tin_nhan];
GO

SET IDENTITY_INSERT [dbo].[tin_nhan] ON;
GO

-- ===================================================================================
-- HỘI THOẠI 1: Học viên Phạm Quỳnh Anh (ma_hoc_vien = 5, ma_nguoi_gui = 7) ↔ Admin (ma_nguoi_nhan = 3)
-- ===================================================================================
INSERT [dbo].[tin_nhan] ([ma_tin_nhan], [ma_nguoi_gui], [ma_nguoi_nhan], [ma_hoc_vien], [noi_dung], [thoi_gian_gui], [da_doc]) 
VALUES (1, 7, 3, 5, N'Chào trung tâm, em muốn hỏi lớp IELTS Basic của em bao giờ khai giảng ạ?', CAST(N'2026-08-25T08:30:00' AS DateTime2), 1);

INSERT [dbo].[tin_nhan] ([ma_tin_nhan], [ma_nguoi_gui], [ma_nguoi_nhan], [ma_hoc_vien], [noi_dung], [thoi_gian_gui], [da_doc]) 
VALUES (2, 3, 7, 5, N'Chào Quỳnh Anh, lớp IELTS Basic (IELTS-BASIC-01) của em sẽ khai giảng vào ngày 01/09/2026 nhé.', CAST(N'2026-08-25T08:35:00' AS DateTime2), 1);

INSERT [dbo].[tin_nhan] ([ma_tin_nhan], [ma_nguoi_gui], [ma_nguoi_nhan], [ma_hoc_vien], [noi_dung], [thoi_gian_gui], [da_doc]) 
VALUES (3, 7, 3, 5, N'Dạ vâng ạ, học phí em đã đóng đủ chưa ạ?', CAST(N'2026-08-25T08:40:00' AS DateTime2), 1);

INSERT [dbo].[tin_nhan] ([ma_tin_nhan], [ma_nguoi_gui], [ma_nguoi_nhan], [ma_hoc_vien], [noi_dung], [thoi_gian_gui], [da_doc]) 
VALUES (4, 3, 7, 5, N'Học phí của em đã hoàn tất đầy đủ rồi nhé. Hẹn gặp em vào buổi học đầu tiên!', CAST(N'2026-08-25T08:45:00' AS DateTime2), 0);


-- ===================================================================================
-- HỘI THOẠI 2: Học viên Nguyễn Văn An (ma_hoc_vien = 6, ma_nguoi_gui = 11) ↔ GV Trần Thị Thu (ma_nguoi_nhan = 4)
-- ===================================================================================
INSERT [dbo].[tin_nhan] ([ma_tin_nhan], [ma_nguoi_gui], [ma_nguoi_nhan], [ma_hoc_vien], [noi_dung], [thoi_gian_gui], [da_doc]) 
VALUES (5, 11, 4, 6, N'Em chào cô Thu, em xin phép nghỉ học buổi hôm nay vì bị ốm đột xuất ạ.', CAST(N'2026-08-26T15:00:00' AS DateTime2), 1);

INSERT [dbo].[tin_nhan] ([ma_tin_nhan], [ma_nguoi_gui], [ma_nguoi_nhan], [ma_hoc_vien], [noi_dung], [thoi_gian_gui], [da_doc]) 
VALUES (6, 4, 11, 6, N'Chào An, cô đã nhận được thông tin. Em nhớ nghỉ ngơi và mượn vở các bạn ghi bài nhé.', CAST(N'2026-08-26T15:15:00' AS DateTime2), 0);


-- ===================================================================================
-- HỘI THOẠI 3: Học viên Lê Minh Đức (ma_hoc_vien = 8, ma_nguoi_gui = 13) ↔ Admin (ma_nguoi_nhan = 3)
-- ===================================================================================
INSERT [dbo].[tin_nhan] ([ma_tin_nhan], [ma_nguoi_gui], [ma_nguoi_nhan], [ma_hoc_vien], [noi_dung], [thoi_gian_gui], [da_doc]) 
VALUES (7, 13, 3, 8, N'Admin kiểm tra giúp em hóa đơn học phí tháng này với ạ, em thấy ghi chưa thanh toán.', CAST(N'2026-08-27T09:10:00' AS DateTime2), 1);

INSERT [dbo].[tin_nhan] ([ma_tin_nhan], [ma_nguoi_gui], [ma_nguoi_nhan], [ma_hoc_vien], [noi_dung], [thoi_gian_gui], [da_doc]) 
VALUES (8, 3, 13, 8, N'Chào Đức, hóa đơn của em đang chờ ngân hàng đối soát khoản chuyển khoản, lát nữa sẽ cập nhật trạng thái nhé.', CAST(N'2026-08-27T09:30:00' AS DateTime2), 0);

GO
SET IDENTITY_INSERT [dbo].[tin_nhan] OFF;
GO

-- Kiểm tra lại dữ liệu đã nạp thành công
SELECT * FROM [dbo].[tin_nhan];