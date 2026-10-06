-- Run once against an existing database before starting the API.
-- The script is safe to run again.
IF COL_LENGTH(N'dbo.diem', N'ghi_chu_nop') IS NULL
BEGIN
    ALTER TABLE [dbo].[diem]
        ADD [ghi_chu_nop] NVARCHAR(1000) NULL;
END
GO

IF COL_LENGTH(N'dbo.diem', N'tep_nop') IS NULL
BEGIN
    ALTER TABLE [dbo].[diem]
        ADD [tep_nop] NVARCHAR(MAX) NULL;
END
GO
