USE [master]
GO
/****** Object:  Database [MailSystem]    Script Date: 10/22/2019 7:05:41 PM ******/
CREATE DATABASE [MailSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MailSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\MailSystem.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MailSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\MailSystem_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [MailSystem] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MailSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MailSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MailSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MailSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MailSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MailSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [MailSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MailSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MailSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MailSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MailSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MailSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MailSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MailSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MailSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MailSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MailSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MailSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MailSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MailSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MailSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MailSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MailSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MailSystem] SET RECOVERY FULL 
GO
ALTER DATABASE [MailSystem] SET  MULTI_USER 
GO
ALTER DATABASE [MailSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MailSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MailSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MailSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MailSystem] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'MailSystem', N'ON'
GO
ALTER DATABASE [MailSystem] SET QUERY_STORE = OFF
GO
USE [MailSystem]
GO
/****** Object:  Table [dbo].[AdTypes]    Script Date: 10/22/2019 7:05:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdTypes](
	[AdTypeId] [bigint] IDENTITY(1,1) NOT NULL,
	[AdTypeName] [nvarchar](max) NULL,
	[Status] [smallint] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
 CONSTRAINT [PK_AdTypes] PRIMARY KEY CLUSTERED 
(
	[AdTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attachments]    Script Date: 10/22/2019 7:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attachments](
	[AttachmentId] [bigint] IDENTITY(1,1) NOT NULL,
	[ConversationId] [bigint] NULL,
	[FileName] [nvarchar](max) NULL,
	[Extension] [nvarchar](max) NULL,
	[ContentFile] [varbinary](max) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [bigint] NULL,
 CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED 
(
	[AttachmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Branches]    Script Date: 10/22/2019 7:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branches](
	[BranchId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Description] [nvarchar](500) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[Status] [smallint] NULL,
	[BranchLevel] [smallint] NULL,
 CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED 
(
	[BranchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Conversations]    Script Date: 10/22/2019 7:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conversations](
	[ConversationID] [bigint] IDENTITY(1,1) NOT NULL,
	[LastSubject] [nvarchar](max) NULL,
	[TimeStamp] [datetime] NULL,
	[Subject] [nvarchar](max) NULL,
	[IsGroup] [bit] NULL,
	[Creator] [bigint] NULL,
	[Body] [nvarchar](max) NULL,
	[Priolti] [nvarchar](max) NULL,
	[AdTypeId] [bigint] NULL,
	[TypeSend] [smallint] NULL,
 CONSTRAINT [PK_Conversations] PRIMARY KEY CLUSTERED 
(
	[ConversationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 10/22/2019 7:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[MessageID] [bigint] IDENTITY(1,1) NOT NULL,
	[ConversationId] [bigint] NULL,
	[AuthorId] [bigint] NULL,
	[DateTime] [datetime] NULL,
	[Subject] [nvarchar](max) NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Participations]    Script Date: 10/22/2019 7:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Participations](
	[ConversationId] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Archive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[IsRead] [bit] NULL,
	[IsDelete] [bit] NULL,
	[IsFavorate] [bit] NULL,
	[IsSendSMS] [bit] NULL,
	[IsSendEmail] [bit] NULL,
	[ExpiryDate] [datetime] NULL,
	[TransactionProccess] [smallint] NULL,
 CONSTRAINT [PK_Participations] PRIMARY KEY CLUSTERED 
(
	[ConversationId] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 10/22/2019 7:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[TransactionID] [bigint] IDENTITY(1,1) NOT NULL,
	[MessageId] [bigint] NULL,
	[UserID] [bigint] NULL,
	[IsRead] [bit] NULL,
	[TimeStamp] [datetime] NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/22/2019 7:05:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[UserType] [smallint] NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Gender] [smallint] NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Phone] [nvarchar](24) NULL,
	[LoginTryAttempts] [smallint] NOT NULL,
	[LoginTryAttemptDate] [datetime] NULL,
	[LastLoginOn] [datetime] NULL,
	[Photo] [varbinary](max) NULL,
	[NationalId] [bigint] NULL,
	[PersonId] [int] NULL,
	[Status] [smallint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[BranchId] [bigint] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AdTypes]  WITH CHECK ADD  CONSTRAINT [FK_AdTypes_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[AdTypes] CHECK CONSTRAINT [FK_AdTypes_Users]
GO
ALTER TABLE [dbo].[AdTypes]  WITH CHECK ADD  CONSTRAINT [FK_AdTypes_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[AdTypes] CHECK CONSTRAINT [FK_AdTypes_Users1]
GO
ALTER TABLE [dbo].[Attachments]  WITH CHECK ADD  CONSTRAINT [FK_Attachments_Conversations] FOREIGN KEY([ConversationId])
REFERENCES [dbo].[Conversations] ([ConversationID])
GO
ALTER TABLE [dbo].[Attachments] CHECK CONSTRAINT [FK_Attachments_Conversations]
GO
ALTER TABLE [dbo].[Attachments]  WITH CHECK ADD  CONSTRAINT [FK_Attachments_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Attachments] CHECK CONSTRAINT [FK_Attachments_Users]
GO
ALTER TABLE [dbo].[Conversations]  WITH CHECK ADD  CONSTRAINT [FK_Conversations_AdTypes] FOREIGN KEY([AdTypeId])
REFERENCES [dbo].[AdTypes] ([AdTypeId])
GO
ALTER TABLE [dbo].[Conversations] CHECK CONSTRAINT [FK_Conversations_AdTypes]
GO
ALTER TABLE [dbo].[Conversations]  WITH CHECK ADD  CONSTRAINT [FK_Conversations_Users] FOREIGN KEY([Creator])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Conversations] CHECK CONSTRAINT [FK_Conversations_Users]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Conversations] FOREIGN KEY([ConversationId])
REFERENCES [dbo].[Conversations] ([ConversationID])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Conversations]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Users] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Users]
GO
ALTER TABLE [dbo].[Participations]  WITH CHECK ADD  CONSTRAINT [FK_Participations_Conversations] FOREIGN KEY([ConversationId])
REFERENCES [dbo].[Conversations] ([ConversationID])
GO
ALTER TABLE [dbo].[Participations] CHECK CONSTRAINT [FK_Participations_Conversations]
GO
ALTER TABLE [dbo].[Participations]  WITH CHECK ADD  CONSTRAINT [FK_Participations_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Participations] CHECK CONSTRAINT [FK_Participations_Users]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Messages] FOREIGN KEY([MessageId])
REFERENCES [dbo].[Messages] ([MessageID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Messages]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Branches] FOREIGN KEY([BranchId])
REFERENCES [dbo].[Branches] ([BranchId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Branches]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0=ALL && 1=SendEmail && 2=SendSMS && 3=NONE' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Conversations', @level2type=N'COLUMN',@level2name=N'TypeSend'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0=SendSuccess && 1=SendFailed && 2=Isproccess && 3= SendOnlyWeb' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Participations', @level2type=N'COLUMN',@level2name=N'TransactionProccess'
GO
USE [master]
GO
ALTER DATABASE [MailSystem] SET  READ_WRITE 
GO
