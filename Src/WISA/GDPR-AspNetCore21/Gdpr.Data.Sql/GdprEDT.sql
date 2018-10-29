﻿CREATE TABLE [dbo].[GdprEDT]
(
    [Id] UNIQUEIDENTIFIER CONSTRAINT [DF_GdprEDT_Id] DEFAULT NEWID() NOT NULL PRIMARY KEY NONCLUSTERED,
    [Name] NVARCHAR(80) NOT NULL, 
    [Address] NVARCHAR(MAX) NULL, 
    [Country] NVARCHAR(40) NULL, 
    [Details] NVARCHAR(MAX) NULL, 
    [TransferFlag] INT CONSTRAINT [DF_GdprEDT_TransferFlag] DEFAULT 0 NOT NULL, 
    [FpdId] UNIQUEIDENTIFIER CONSTRAINT [FK_GdprEDT_FpdId] FOREIGN KEY REFERENCES [dbo].[GdprFPD] (Id) ON DELETE CASCADE NOT NULL, 
)
GO;
CREATE UNIQUE NONCLUSTERED INDEX [IX_GdprEDT_FpdId] ON [dbo].[GdprEDT] ([FpdId])
GO;
CREATE UNIQUE CLUSTERED INDEX [IX_GdprEDT_Name] ON [dbo].[GdprEDT] ([Name])
GO;
