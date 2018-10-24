﻿CREATE TABLE [dbo].[GdprDTD]
(
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_GdprDTD_Id] DEFAULT NEWID() NOT NULL PRIMARY KEY NONCLUSTERED, 
    [DisplayName] NVARCHAR(50) NULL, 
    [Details] NVARCHAR(MAX) NULL
)
