/* 
   GDPR-AspNetCore21 - AlterAftertCreateTables.sql - performs alterations after creation of tables 
*/

/* 
   Run only once on the database created by AspNet WebApp with Identity
*/

DROP INDEX [EmailIndex] ON [dbo].[AspNetUsers]
GO

CREATE UNIQUE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers] ([Email])
GO

/* 
   Run after drop/publish 
*/


USE [GdprCore21]
GO

ALTER TABLE [dbo].[GdprRPD]  WITH CHECK ADD  CONSTRAINT [FK_GdprRPD_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[GdprRPD] CHECK CONSTRAINT [FK_GdprRPD_UserId]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_GdprRPD_UserId] ON [dbo].[GdprRPD] ([UserId])
GO

ALTER TABLE [dbo].[GdprURD]  WITH CHECK ADD  CONSTRAINT [FK_GdprURD_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[GdprURD] CHECK CONSTRAINT [FK_GdprURD_RoleId]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_GdprURD_RoleId] ON [dbo].[GdprURD] ([RoleId])
GO


/* 
   This script allows you to make quick changes to the schema. At major releases you would drop all tables, update the table creation SQL, 
   republish and then run this script with the changes listed below removed.

*/