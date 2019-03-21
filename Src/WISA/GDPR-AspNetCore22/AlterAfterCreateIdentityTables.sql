USE [GdprCore22]


DROP INDEX [EmailIndex] ON [dbo].[AspNetUsers]
GO

CREATE UNIQUE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers] ([Email])
GO

