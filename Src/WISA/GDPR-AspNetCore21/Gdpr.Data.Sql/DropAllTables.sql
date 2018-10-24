USE [GdprCore21]
GO

/* Tables without Dependencies */

DROP TABLE [dbo].[GpdrTest]
GO

DROP TABLE [dbo].[GdprPDF] 
GO


/* Tables with foreign keys */

DROP TABLE [dbo].[GdprCXP]  /*ref to FDP, RPD*/
GO

DROP TABLE [dbo].[GdprDXP] /* ref to FDP, PDS*/
GO

DROP TABLE [dbo].[GdprEDT] /* ref to FDP */
GO

/* Tables with dependencies */

DROP TABLE [dbo].[GdprPDS] /* ref to DTD, DCD; ref by DXP */
GO

DROP TABLE [dbo].[GdprDCD] /* ref by PDS */
GO

DROP TABLE [dbo].[GdprDTD] /* ref by PDS */
GO

DROP TABLE [dbo].[GdprRXP] /* ref to FDP, URD*/
GO

DROP TABLE [dbo].[GdprFPD] /* ref by CXP, DXP, EDT, RXP */
GO

DROP TABLE [dbo].[GdprUTA] /* ref to RPD, WST*/
GO

DROP TABLE [dbo].[GdprRPD] /* ref to URD, AspNetUsers; ref by CXP, UTA */
GO

DROP TABLE [dbo].[GdprWXR] /* ref to URD, WST */
GO

DROP TABLE [dbo].[GdprWST] /* ref by WXR, UTA */
GO

DROP TABLE [dbo].[GdprURD] /* ref by RXP, RPD */
GO





