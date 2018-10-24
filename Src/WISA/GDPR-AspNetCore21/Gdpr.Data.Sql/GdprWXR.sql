﻿CREATE TABLE [dbo].[GdprWXR]
(
	[Id] UNIQUEIDENTIFIER CONSTRAINT [DF_GdprWXR_Id] DEFAULT NEWID() NOT NULL PRIMARY KEY NONCLUSTERED, 
    [UrdId] UNIQUEIDENTIFIER CONSTRAINT [FK_GdprWXR_UrdId] FOREIGN KEY REFERENCES [dbo].[GdprURD] (Id) ON DELETE CASCADE NOT NULL,  
    [WstId] UNIQUEIDENTIFIER CONSTRAINT [FK_GdprWXR_WstId] FOREIGN KEY REFERENCES [dbo].[GdprWST] (Id) ON DELETE CASCADE  NOT NULL, 
)
GO;
CREATE UNIQUE NONCLUSTERED INDEX [IX_GdprWXR_UrdId] ON [dbo].[GdprWXR] ([UrdId])
GO;
CREATE UNIQUE NONCLUSTERED INDEX [IX_GdprWXR_WstId] ON [dbo].[GdprWXR] ([WstId])
GO;
