﻿CREATE TABLE [dbo].[GdprWST]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT [DF_GdprWST_Id] DEFAULT NEWID() NOT NULL PRIMARY KEY NONCLUSTERED, 
    [Title] NVARCHAR(80) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Created] DATETIME2 CONSTRAINT [DF_GdprWST_Created] DEFAULT SYSUTCDATETIME() NOT NULL,  
    [Published] DATETIME2 NULL,
    [Updated] DATETIME2 NULL,
    [Status] INT CONSTRAINT [DF_GdprWST_Status] DEFAULT 0 NOT NULL, 
    [Link] NVARCHAR(MAX) NULL, 
    [Hash] NVARCHAR(250) NULL, 
)
GO;
CREATE UNIQUE CLUSTERED INDEX [IX_GdprWST_Title] ON [dbo].[GdprWST] ([Title])
GO;
