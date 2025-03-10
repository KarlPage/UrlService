CREATE DATABASE URLService
GO

USE [URLService]
GO

/****** Object:  Table [dbo].[UrlKey]    Script Date: 04/03/2021 15:07:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UrlKey](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UrlKey] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UrlToken] PRIMARY KEY CLUSTERED 
(
	[UrlKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Url]    Script Date: 04/03/2021 15:07:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Url](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UrlKeyId] [int] NOT NULL,
	[Url] [nvarchar](2048) NOT NULL,
 CONSTRAINT [PK_Url] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



/****** Object:  View [dbo].[vUrls]    Script Date: 04/03/2021 15:07:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vUrls]
AS
SELECT t.UrlKey, u.Url
FROM dbo.UrlKey AS t
INNER JOIN dbo.Url AS u ON u.UrlKeyId = t.Id
GO


/****** Object:  Index [NonClusteredIndex-20210304-134512]    Script Date: 04/03/2021 15:07:17 ******/
CREATE UNIQUE NONCLUSTERED INDEX [NonClusteredIndex-20210304-134512] ON [dbo].[Url]
(
	[UrlKeyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO


/****** Object:  StoredProcedure [dbo].[AddUrlKey]    Script Date: 04/03/2021 15:07:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Karl Page
-- Create date: 03/03/2021
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[AddUrlKey] 
  @urlKey nvarchar(50), 
  @url nvarchar(2048)
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRY
    BEGIN TRANSACTION
     
    INSERT INTO UrlKey(UrlKey) VALUES(@urlKey)

	  DECLARE @urlKeyId int
	  SET @urlKeyId = SCOPE_IDENTITY();

	  INSERT INTO Url(UrlKeyId, Url) VALUES(@urlKeyId, @url)
	  COMMIT
	  SELECT '' as Error
  END TRY	
  BEGIN CATCH
    ROLLBACK TRANSACTION
	SELECT ERROR_MESSAGE() as Error
  END CATCH
END
GO


/****** Object:  StoredProcedure [dbo].[ClearTables]    Script Date: 04/03/2021 15:07:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Karl Page
-- Create date: 04/03/2021
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[ClearTables] 
AS
BEGIN
  truncate table Url
  truncate table UrlKey
END
GO


/****** Object:  StoredProcedure [dbo].[GetUrl]    Script Date: 04/03/2021 15:07:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Karl Page
-- Create date: 03/03/03
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetUrl] 
  @urlKey nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @urlKeyId int

	SELECT @urlKeyId = id FROM UrlKey WHERE UrlKey = @urlKey

	IF (@urlKeyId is null)
	BEGIN
	  SELECT 'UrlKey not found' AS Error, ''
	END
	ELSE
	BEGIN
	  DECLARE @url nvarchar(2048);
	  SELECT @url = url FROM Url WHERE urlKeyId = @urlKeyId

	  IF (@url is null)
	  BEGIN	   
			SELECT 'URL not found' AS Error
	  END
	  ELSE
	  BEGIN
	    SELECT '' AS Error, @url AS Url
	  END
	END
END
GO

USE master
GO

CREATE LOGIN UrlServiceUser WITH PASSWORD=N'P@ssw0rd!', DEFAULT_DATABASE=UrlService
GO

ALTER LOGIN UrlServiceUser ENABLE
GO

USE URLService
GO

CREATE USER UrlServiceUser FOR LOGIN UrlServiceUser
EXEC sp_addrolemember 'db_owner', 'UrlServiceUser'
GO