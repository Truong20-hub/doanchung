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
