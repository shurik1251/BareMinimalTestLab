USE [master]
GO

CREATE DATABASE [TheApp]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TheApp', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TheApp.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TheApp_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TheApp_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO


USE [TheApp]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Pupils](
	[PupilId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](64) NOT NULL,
	[MiddleName] [nvarchar](64) NULL,
	[LastName] [nvarchar](64) NOT NULL,
	[BirthDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Pupils] PRIMARY KEY CLUSTERED 
(
	[PupilId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Pupils] ON 
GO

INSERT [dbo].[Pupils] ([PupilId], [FirstName], [MiddleName], [LastName], [BirthDate]) VALUES (1, N'First', N'Middle', N'Last', CAST(N'2018-02-24 00:00:00.000' AS DateTime))
GO

SET IDENTITY_INSERT [dbo].[Pupils] OFF
GO
