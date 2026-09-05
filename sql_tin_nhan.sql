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
