USE [DemoDatabase]
GO

/****** Object:  Table [dbo].[CompanyItems]    Script Date: 3/17/2025 5:19:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CompanyItems](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[StockTicker] [nvarchar](30) NOT NULL,
	[Exchange] [nvarchar](30) NOT NULL,
	[Isin] [nvarchar](30) NOT NULL,
	[WebUrl] [nvarchar](30) NULL,
 CONSTRAINT [PK_CompanyItems] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

