USE [master]
GO
/****** Object:  Database [BaseDatos]    Script Date: 16/5/2022 23:19:30 ******/
CREATE DATABASE [BaseDatos]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BaseDatos', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLSERVERATLAS\MSSQL\DATA\BaseDatos.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BaseDatos_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLSERVERATLAS\MSSQL\DATA\BaseDatos_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [BaseDatos] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BaseDatos].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BaseDatos] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BaseDatos] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BaseDatos] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BaseDatos] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BaseDatos] SET ARITHABORT OFF 
GO
ALTER DATABASE [BaseDatos] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BaseDatos] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BaseDatos] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BaseDatos] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BaseDatos] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BaseDatos] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BaseDatos] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BaseDatos] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BaseDatos] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BaseDatos] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BaseDatos] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BaseDatos] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BaseDatos] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BaseDatos] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BaseDatos] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BaseDatos] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BaseDatos] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BaseDatos] SET RECOVERY FULL 
GO
ALTER DATABASE [BaseDatos] SET  MULTI_USER 
GO
ALTER DATABASE [BaseDatos] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BaseDatos] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BaseDatos] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BaseDatos] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BaseDatos] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'BaseDatos', N'ON'
GO
ALTER DATABASE [BaseDatos] SET QUERY_STORE = OFF
GO
USE [BaseDatos]
GO
/****** Object:  Table [dbo].[cliente]    Script Date: 16/5/2022 23:19:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cliente](
	[cli_id_cliente] [int] IDENTITY(1,1) NOT NULL,
	[cli_identificacion] [varchar](30) NOT NULL,
	[cli_contrasenia] [varchar](30) NOT NULL,
	[cli_estado] [bit] NOT NULL,
	[cli_nombre] [varchar](30) NOT NULL,
	[cli_genero] [varchar](30) NOT NULL,
	[cli_edad] [int] NOT NULL,
	[cli_direccion] [varchar](50) NULL,
	[cli_telefono] [varchar](20) NULL,
 CONSTRAINT [PK_cliente] PRIMARY KEY CLUSTERED 
(
	[cli_id_cliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cuenta]    Script Date: 16/5/2022 23:19:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cuenta](
	[cue_numero] [varchar](30) NOT NULL,
	[cli_id_cliente] [int] NOT NULL,
	[cue_tipo] [varchar](30) NOT NULL,
	[cue_estado] [bit] NOT NULL,
	[cue_saldo] [money] NOT NULL,
 CONSTRAINT [PK__cuenta__5138EEC71FAE4CF6] PRIMARY KEY CLUSTERED 
(
	[cue_numero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[movimiento]    Script Date: 16/5/2022 23:19:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[movimiento](
	[mov_id_movimiento] [int] IDENTITY(1,1) NOT NULL,
	[cue_numero] [varchar](30) NOT NULL,
	[mov_fecha] [datetime] NOT NULL,
	[mov_tipo] [varchar](30) NOT NULL,
	[mov_saldo_inicial] [money] NOT NULL,
	[mov_movimiento_valor] [money] NOT NULL,
	[mov_saldo_actual] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[mov_id_movimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[cuenta]  WITH CHECK ADD  CONSTRAINT [FK_cuenta_cuenta] FOREIGN KEY([cli_id_cliente])
REFERENCES [dbo].[cliente] ([cli_id_cliente])
GO
ALTER TABLE [dbo].[cuenta] CHECK CONSTRAINT [FK_cuenta_cuenta]
GO
ALTER TABLE [dbo].[movimiento]  WITH CHECK ADD  CONSTRAINT [FK_movimiento_cuenta] FOREIGN KEY([cue_numero])
REFERENCES [dbo].[cuenta] ([cue_numero])
GO
ALTER TABLE [dbo].[movimiento] CHECK CONSTRAINT [FK_movimiento_cuenta]
GO
USE [master]
GO
ALTER DATABASE [BaseDatos] SET  READ_WRITE 
GO
