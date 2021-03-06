USE [master]
GO
/****** Object:  Database [MailSystem]    Script Date: 9/28/2019 2:00:36 PM ******/
CREATE DATABASE [MailSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MailSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\MailSystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MailSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\MailSystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
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
/****** Object:  Table [dbo].[AdTypes]    Script Date: 9/28/2019 2:00:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdTypes](
	[AdTypeId] [nvarchar](128) NOT NULL,
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
/****** Object:  Table [dbo].[Attachments]    Script Date: 9/28/2019 2:00:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attachments](
	[AttachmentId] [nvarchar](128) NOT NULL,
	[ConversationId] [nvarchar](128) NULL,
	[FileName] [nvarchar](max) NULL,
	[Extension] [nvarchar](max) NULL,
	[ContentFile] [binary](100) NULL,
	[FileSize] [float] NULL,
	[Hash] [nvarchar](max) NULL,
 CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED 
(
	[AttachmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Branches]    Script Date: 9/28/2019 2:00:36 PM ******/
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
 CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED 
(
	[BranchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Conversations]    Script Date: 9/28/2019 2:00:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conversations](
	[ConversationID] [nvarchar](128) NOT NULL,
	[LastSubject] [nvarchar](max) NULL,
	[TimeStamp] [datetime] NULL,
	[Subject] [nvarchar](max) NULL,
	[IsGroup] [bit] NULL,
	[Creator] [bigint] NULL,
	[Body] [nvarchar](max) NULL,
	[Priolti] [nvarchar](max) NULL,
	[AdTypeId] [nvarchar](128) NULL,
 CONSTRAINT [PK_Conversations] PRIMARY KEY CLUSTERED 
(
	[ConversationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 9/28/2019 2:00:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[MessageID] [nvarchar](128) NOT NULL,
	[ConversationId] [nvarchar](128) NULL,
	[AuthorId] [bigint] NULL,
	[DateTime] [datetime] NULL,
	[Subject] [nvarchar](max) NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Participations]    Script Date: 9/28/2019 2:00:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Participations](
	[ConversationId] [nvarchar](128) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Archive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[IsRead] [bit] NULL,
	[IsDelete] [bit] NULL,
	[IsFavorate] [bit] NULL,
 CONSTRAINT [PK_Participations] PRIMARY KEY CLUSTERED 
(
	[ConversationId] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 9/28/2019 2:00:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[TransactionID] [nvarchar](128) NOT NULL,
	[MessageId] [nvarchar](128) NULL,
	[UserID] [bigint] NULL,
	[IsRead] [bit] NULL,
	[TimeStamp] [datetime] NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 9/28/2019 2:00:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[UserType] [smallint] NOT NULL,
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
INSERT [dbo].[AdTypes] ([AdTypeId], [AdTypeName], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (N'1', N'اعلان', 1, CAST(N'2019-10-10T00:00:00.000' AS DateTime), 7, CAST(N'2019-10-10T00:00:00.000' AS DateTime), 7)
INSERT [dbo].[AdTypes] ([AdTypeId], [AdTypeName], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (N'2', N'قرار', 0, CAST(N'2019-10-10T00:00:00.000' AS DateTime), 7, CAST(N'2019-10-10T00:00:00.000' AS DateTime), 7)
SET IDENTITY_INSERT [dbo].[Branches] ON 

INSERT [dbo].[Branches] ([BranchId], [Name], [Description], [CreatedOn], [CreatedBy], [ModifiedBy], [ModifiedOn], [Status]) VALUES (1, N'الادارية المالية', N'لايوجد', CAST(N'2000-01-02T00:00:00.000' AS DateTime), 1, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Branches] OFF
INSERT [dbo].[Conversations] ([ConversationID], [LastSubject], [TimeStamp], [Subject], [IsGroup], [Creator], [Body], [Priolti], [AdTypeId]) VALUES (N'd89970ca-4213-483c-a19b-68d7b91f4148', N'سلام عليكم ورحمة الله اعلان تجريبي يشمل النقال التالية:نقاط الاولة نقاط التانيةنقاط الثالثةنقاط الرابعةتحياتي لكم', CAST(N'2019-09-28T13:48:44.040' AS DateTime), N'اجتماع تجريبي بخصوص الاختبار', 1, 8, N'<p class="ql-align-right ql-direction-rtl"><strong style="color: rgb(255, 153, 0);">سلام عليكم ورحمة الله </strong></p><p class="ql-align-right ql-direction-rtl"><br></p><p class="ql-align-right ql-direction-rtl"><u>اعلان تجريبي يشمل النقال التالية:</u></p><ol><li class="ql-align-right ql-direction-rtl">نقاط الاولة </li><li class="ql-align-right ql-direction-rtl">نقاط التانية</li><li class="ql-align-right ql-direction-rtl">نقاط الثالثة</li><li class="ql-align-right ql-direction-rtl">نقاط الرابعة</li></ol><p class="ql-align-right ql-direction-rtl"><br></p><p class="ql-align-right ql-direction-rtl"><span style="background-color: rgb(255, 255, 0);">تحياتي لكم</span></p>', N' مهم ', N'1')
INSERT [dbo].[Messages] ([MessageID], [ConversationId], [AuthorId], [DateTime], [Subject]) VALUES (N'33f958a0-348d-40b6-8b27-4afde2a65809', N'd89970ca-4213-483c-a19b-68d7b91f4148', 7, CAST(N'2019-09-28T13:53:08.420' AS DateTime), N'تمام Test Test')
INSERT [dbo].[Messages] ([MessageID], [ConversationId], [AuthorId], [DateTime], [Subject]) VALUES (N'c25a3cb8-e69b-4b9b-be62-545deb9fd040', N'd89970ca-4213-483c-a19b-68d7b91f4148', 8, CAST(N'2019-09-28T13:51:50.373' AS DateTime), N'عليكم السلام
اوك اوك')
INSERT [dbo].[Participations] ([ConversationId], [UserID], [Archive], [CreatedOn], [IsRead], [IsDelete], [IsFavorate]) VALUES (N'd89970ca-4213-483c-a19b-68d7b91f4148', 7, 0, CAST(N'2019-09-28T13:48:49.590' AS DateTime), 1, 0, 0)
INSERT [dbo].[Participations] ([ConversationId], [UserID], [Archive], [CreatedOn], [IsRead], [IsDelete], [IsFavorate]) VALUES (N'd89970ca-4213-483c-a19b-68d7b91f4148', 8, 0, CAST(N'2019-09-28T13:48:53.040' AS DateTime), 1, 0, 0)
INSERT [dbo].[Transactions] ([TransactionID], [MessageId], [UserID], [IsRead], [TimeStamp]) VALUES (N'74900e30-5238-4ea4-a947-42e4dcc84b61', N'33f958a0-348d-40b6-8b27-4afde2a65809', 8, 1, CAST(N'2019-09-28T13:53:08.420' AS DateTime))
INSERT [dbo].[Transactions] ([TransactionID], [MessageId], [UserID], [IsRead], [TimeStamp]) VALUES (N'f1ed3a4a-a588-4210-8af6-8503c89b2a52', N'c25a3cb8-e69b-4b9b-be62-545deb9fd040', 7, 1, CAST(N'2019-09-28T13:51:50.383' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [LoginName], [Password], [FullName], [UserType], [Email], [Gender], [DateOfBirth], [Phone], [LoginTryAttempts], [LoginTryAttemptDate], [LastLoginOn], [Photo], [NationalId], [PersonId], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [BranchId]) VALUES (7, N'AbdullahElamir', N's9s29j8nmkIZkPQ0y1LP+WM0SmcSntd/D/xz+fwfRqu8v9TOnDmYXpe8xuB2k8JjD39xDYMwtgobpCV8ToDiq8aDbVkswA==', N'abdullah elamir', 1, N'abdullahelameer@gmail.com', 1, CAST(N'2011-11-02' AS Date), N'911111290', 0, CAST(N'2011-11-02T00:00:00.000' AS DateTime), CAST(N'2019-09-28T13:52:17.170' AS DateTime), NULL, 1, NULL, 1, CAST(N'2011-11-02T00:00:00.000' AS DateTime), 1, NULL, NULL, 1)
INSERT [dbo].[Users] ([UserId], [LoginName], [Password], [FullName], [UserType], [Email], [Gender], [DateOfBirth], [Phone], [LoginTryAttempts], [LoginTryAttemptDate], [LastLoginOn], [Photo], [NationalId], [PersonId], [Status], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [BranchId]) VALUES (8, N'Test', N's9s29j8nmkIZkPQ0y1LP+WM0SmcSntd/D/xz+fwfRqu8v9TOnDmYXpe8xuB2k8JjD39xDYMwtgobpCV8ToDiq8aDbVkswA==', N'Test Test', 1, N'aaa@gmail.com', 1, CAST(N'2011-11-02' AS Date), N'911111290', 0, CAST(N'2011-11-02T00:00:00.000' AS DateTime), CAST(N'2019-09-28T13:53:37.760' AS DateTime), NULL, 1, NULL, 1, CAST(N'2011-11-02T00:00:00.000' AS DateTime), 1, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
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
USE [master]
GO
ALTER DATABASE [MailSystem] SET  READ_WRITE 
GO
