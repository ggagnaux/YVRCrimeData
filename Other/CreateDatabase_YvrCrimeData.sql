USE [master]
GO
/****** Object:  Database [YvrCrimeData]    Script Date: 7/5/2017 5:37:05 PM ******/
CREATE DATABASE [YvrCrimeData]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'YvrCrimeData', FILENAME = N'D:\Program Files (x86)\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\YvrCrimeData.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'YvrCrimeData_log', FILENAME = N'D:\Program Files (x86)\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\YvrCrimeData_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [YvrCrimeData] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [YvrCrimeData].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [YvrCrimeData] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [YvrCrimeData] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [YvrCrimeData] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [YvrCrimeData] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [YvrCrimeData] SET ARITHABORT OFF 
GO
ALTER DATABASE [YvrCrimeData] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [YvrCrimeData] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [YvrCrimeData] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [YvrCrimeData] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [YvrCrimeData] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [YvrCrimeData] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [YvrCrimeData] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [YvrCrimeData] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [YvrCrimeData] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [YvrCrimeData] SET  DISABLE_BROKER 
GO
ALTER DATABASE [YvrCrimeData] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [YvrCrimeData] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [YvrCrimeData] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [YvrCrimeData] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [YvrCrimeData] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [YvrCrimeData] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [YvrCrimeData] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [YvrCrimeData] SET RECOVERY FULL 
GO
ALTER DATABASE [YvrCrimeData] SET  MULTI_USER 
GO
ALTER DATABASE [YvrCrimeData] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [YvrCrimeData] SET DB_CHAINING OFF 
GO
ALTER DATABASE [YvrCrimeData] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [YvrCrimeData] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [YvrCrimeData] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'YvrCrimeData', N'ON'
GO
ALTER DATABASE [YvrCrimeData] SET QUERY_STORE = OFF
GO
USE [YvrCrimeData]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [YvrCrimeData]
GO
/****** Object:  Table [dbo].[Crime]    Script Date: 7/5/2017 5:37:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Crime](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CrimeTypeID] [int] NULL,
	[OffenceDate] [datetime] NULL,
	[Year] [int] NULL,
	[Month] [int] NULL,
	[Day] [int] NULL,
	[Hour] [int] NULL,
	[Minute] [int] NULL,
	[HundredBlock] [nvarchar](80) NULL,
	[NeighbourhoodID] [int] NULL,
	[XCoordinate] [decimal](18, 2) NULL,
	[YCoordinate] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Crime] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CrimeType]    Script Date: 7/5/2017 5:37:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CrimeType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](60) NULL,
 CONSTRAINT [PK_CrimeType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Neighbourhood]    Script Date: 7/5/2017 5:37:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Neighbourhood](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](60) NULL,
 CONSTRAINT [PK_Neighbourhood] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Crime]  WITH CHECK ADD  CONSTRAINT [FK_Crime_CrimeType] FOREIGN KEY([CrimeTypeID])
REFERENCES [dbo].[CrimeType] ([ID])
GO
ALTER TABLE [dbo].[Crime] CHECK CONSTRAINT [FK_Crime_CrimeType]
GO
ALTER TABLE [dbo].[Crime]  WITH CHECK ADD  CONSTRAINT [FK_Crime_Neighbourhood] FOREIGN KEY([NeighbourhoodID])
REFERENCES [dbo].[Neighbourhood] ([ID])
GO
ALTER TABLE [dbo].[Crime] CHECK CONSTRAINT [FK_Crime_Neighbourhood]
GO
USE [master]
GO
ALTER DATABASE [YvrCrimeData] SET  READ_WRITE 
GO
