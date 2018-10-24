﻿CREATE TABLE [dbo].[GdprPDF]
(
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_GdprPDF_Id] DEFAULT NEWID() NOT NULL PRIMARY KEY NONCLUSTERED, 
    [TableName] NVARCHAR(50) NULL, 
    [FieldName] NVARCHAR(50) NULL, 
    [Value] INT UNIQUE NOT NULL, 
    [Combine] INT CONSTRAINT [DF_GdprPDF_Combine] DEFAULT 0 NOT NULL, 
    [Description] NVARCHAR(MAX) NULL
)
