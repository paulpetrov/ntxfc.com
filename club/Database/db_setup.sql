/****** Object:  ForeignKey [Aircraft_Images]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Aircraft_Images]') AND parent_object_id = OBJECT_ID(N'[dbo].[AircraftImage]'))
ALTER TABLE [dbo].[AircraftImage] DROP CONSTRAINT [Aircraft_Images]
GO
/****** Object:  ForeignKey [InstructorData_Member]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[InstructorData_Member]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstructorData]'))
ALTER TABLE [dbo].[InstructorData] DROP CONSTRAINT [InstructorData_Member]
GO
/****** Object:  ForeignKey [Aircraft_Owners]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Aircraft_Owners]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member]'))
ALTER TABLE [dbo].[Member] DROP CONSTRAINT [Aircraft_Owners]
GO
/****** Object:  ForeignKey [Login_ClubMember]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Login_ClubMember]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member]'))
ALTER TABLE [dbo].[Member] DROP CONSTRAINT [Login_ClubMember]
GO
/****** Object:  ForeignKey [Member_Roles_Source]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Member_Roles_Source]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member_Role]'))
ALTER TABLE [dbo].[Member_Role] DROP CONSTRAINT [Member_Roles_Source]
GO
/****** Object:  ForeignKey [Member_Roles_Target]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Member_Roles_Target]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member_Role]'))
ALTER TABLE [dbo].[Member_Role] DROP CONSTRAINT [Member_Roles_Target]
GO
/****** Object:  ForeignKey [PilotCheckout_Aircraft]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout_Aircraft]') AND parent_object_id = OBJECT_ID(N'[dbo].[PilotCheckout]'))
ALTER TABLE [dbo].[PilotCheckout] DROP CONSTRAINT [PilotCheckout_Aircraft]
GO
/****** Object:  ForeignKey [PilotCheckout_Instructor]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout_Instructor]') AND parent_object_id = OBJECT_ID(N'[dbo].[PilotCheckout]'))
ALTER TABLE [dbo].[PilotCheckout] DROP CONSTRAINT [PilotCheckout_Instructor]
GO
/****** Object:  ForeignKey [PilotCheckout_Pilot]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout_Pilot]') AND parent_object_id = OBJECT_ID(N'[dbo].[PilotCheckout]'))
ALTER TABLE [dbo].[PilotCheckout] DROP CONSTRAINT [PilotCheckout_Pilot]
GO
/****** Object:  ForeignKey [Reservation_Aircraft]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Reservation_Aircraft]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservation]'))
ALTER TABLE [dbo].[Reservation] DROP CONSTRAINT [Reservation_Aircraft]
GO
/****** Object:  ForeignKey [Reservation_Instructor]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Reservation_Instructor]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservation]'))
ALTER TABLE [dbo].[Reservation] DROP CONSTRAINT [Reservation_Instructor]
GO
/****** Object:  ForeignKey [Reservation_Member]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Reservation_Member]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservation]'))
ALTER TABLE [dbo].[Reservation] DROP CONSTRAINT [Reservation_Member]
GO
/****** Object:  ForeignKey [Squawk_Aircraft]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Squawk_Aircraft]') AND parent_object_id = OBJECT_ID(N'[dbo].[Squawk]'))
ALTER TABLE [dbo].[Squawk] DROP CONSTRAINT [Squawk_Aircraft]
GO
/****** Object:  ForeignKey [Squawk_Originator]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Squawk_Originator]') AND parent_object_id = OBJECT_ID(N'[dbo].[Squawk]'))
ALTER TABLE [dbo].[Squawk] DROP CONSTRAINT [Squawk_Originator]
GO
/****** Object:  ForeignKey [StageCheck_Pilot]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[StageCheck_Pilot]') AND parent_object_id = OBJECT_ID(N'[dbo].[StageCheck]'))
ALTER TABLE [dbo].[StageCheck] DROP CONSTRAINT [StageCheck_Pilot]
GO
/****** Object:  Table [dbo].[InstructorData]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InstructorData]') AND type in (N'U'))
DROP TABLE [dbo].[InstructorData]
GO
/****** Object:  Table [dbo].[PilotCheckout]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout]') AND type in (N'U'))
DROP TABLE [dbo].[PilotCheckout]
GO
/****** Object:  Table [dbo].[Reservation]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reservation]') AND type in (N'U'))
DROP TABLE [dbo].[Reservation]
GO
/****** Object:  Table [dbo].[Member_Role]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Member_Role]') AND type in (N'U'))
DROP TABLE [dbo].[Member_Role]
GO
/****** Object:  Table [dbo].[Squawk]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Squawk]') AND type in (N'U'))
DROP TABLE [dbo].[Squawk]
GO
/****** Object:  Table [dbo].[StageCheck]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StageCheck]') AND type in (N'U'))
DROP TABLE [dbo].[StageCheck]
GO
/****** Object:  Table [dbo].[Member]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Member]') AND type in (N'U'))
DROP TABLE [dbo].[Member]
GO
/****** Object:  Table [dbo].[AircraftImage]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AircraftImage]') AND type in (N'U'))
DROP TABLE [dbo].[AircraftImage]
GO
/****** Object:  Table [dbo].[EdmMetadata]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EdmMetadata]') AND type in (N'U'))
DROP TABLE [dbo].[EdmMetadata]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Login]') AND type in (N'U'))
DROP TABLE [dbo].[Login]
GO
/****** Object:  Table [dbo].[Aircraft]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Aircraft]') AND type in (N'U'))
DROP TABLE [dbo].[Aircraft]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 11/12/2011 21:45:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Role]') AND type in (N'U'))
DROP TABLE [dbo].[Role]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Role]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Role](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK__Role__3214EC0721B6055D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
INSERT [dbo].[Role] ([Id], [Name], [Description]) VALUES (1, N'Admin', N'Site Administrator')
INSERT [dbo].[Role] ([Id], [Name], [Description]) VALUES (2, N'AircraftOwner', N'Aircraft Owner')
INSERT [dbo].[Role] ([Id], [Name], [Description]) VALUES (3, N'Instructor', N'Club Instructor')
INSERT [dbo].[Role] ([Id], [Name], [Description]) VALUES (4, N'SiteEditor', N'Web site content editor')
INSERT [dbo].[Role] ([Id], [Name], [Description]) VALUES (5, N'Pilot', N'Club Member')
INSERT [dbo].[Role] ([Id], [Name], [Description]) VALUES (6, N'AircraftMaintenance', N'Aircraft Maintenance Personnel')
/****** Object:  Table [dbo].[Aircraft]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Aircraft]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Aircraft](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RegistrationNumber] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Model] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TypeDesignation] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Category] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AircraftClass] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MaxGrossWeight] [int] NULL,
	[FuelCapacity] [smallint] NULL,
	[UsefulLoad] [int] NULL,
	[CruiseSpeed] [smallint] NULL,
	[MaxRange] [smallint] NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[EquipmentList] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IrCertified] [bit] NULL,
	[CheckoutRequirements] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HourlyRate] [smallint] NULL,
	[Status] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[StatusNotes] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK__Aircraft__3214EC077F60ED59] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[Aircraft] ON
INSERT [dbo].[Aircraft] ([Id], [Name], [RegistrationNumber], [Model], [TypeDesignation], [Category], [AircraftClass], [MaxGrossWeight], [FuelCapacity], [UsefulLoad], [CruiseSpeed], [MaxRange], [Description], [EquipmentList], [IrCertified], [CheckoutRequirements], [HourlyRate], [Status], [StatusNotes]) VALUES (1, NULL, N'N73192', N'Cessna C-172', N'C-172', N'Single Engine', N'SEL', 1700, 30, 300, 100, 300, NULL, NULL, 0, N'Fly', 100, N'Grounded', NULL)
INSERT [dbo].[Aircraft] ([Id], [Name], [RegistrationNumber], [Model], [TypeDesignation], [Category], [AircraftClass], [MaxGrossWeight], [FuelCapacity], [UsefulLoad], [CruiseSpeed], [MaxRange], [Description], [EquipmentList], [IrCertified], [CheckoutRequirements], [HourlyRate], [Status], [StatusNotes]) VALUES (2, NULL, N'N2099V', N'Cessna C-120', N'C-120', N'Taildragger', N'SEL', 1700, 30, 300, 100, 300, NULL, NULL, 0, N'Fly', 100, N'Online', NULL)
SET IDENTITY_INSERT [dbo].[Aircraft] OFF
/****** Object:  Table [dbo].[Login]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Login]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Login](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ForumUserId] [int] NOT NULL,
	[Username] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Password] [nvarchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PasswordSalt] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Email] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK__Login__3214EC070EA330E9] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[Login] ON
INSERT [dbo].[Login] ([Id], [ForumUserId], [Username], [Password], [PasswordSalt], [Email]) VALUES (1, 0, N'admin', N'testpass1', NULL, NULL)
INSERT [dbo].[Login] ([Id], [ForumUserId], [Username], [Password], [PasswordSalt], [Email]) VALUES (2, 0, N'owner1', N'test', NULL, NULL)
INSERT [dbo].[Login] ([Id], [ForumUserId], [Username], [Password], [PasswordSalt], [Email]) VALUES (3, 0, N'owner2', N'test', NULL, NULL)
INSERT [dbo].[Login] ([Id], [ForumUserId], [Username], [Password], [PasswordSalt], [Email]) VALUES (4, 0, N'pilot1', N'test', NULL, NULL)
INSERT [dbo].[Login] ([Id], [ForumUserId], [Username], [Password], [PasswordSalt], [Email]) VALUES (5, 0, N'instructor1', N'test', NULL, NULL)
INSERT [dbo].[Login] ([Id], [ForumUserId], [Username], [Password], [PasswordSalt], [Email]) VALUES (6, 179, N'guest', N'testpass1', NULL, N'guest@guest.com')

SET IDENTITY_INSERT [dbo].[Login] OFF
/****** Object:  Table [dbo].[EdmMetadata]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EdmMetadata]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EdmMetadata](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModelHash] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK__EdmMetad__3214EC0707020F21] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[EdmMetadata] ON
INSERT [dbo].[EdmMetadata] ([Id], [ModelHash]) VALUES (1, N'AFF3B94C0066F2F26879C8D8CC6008262FA117918C9E45EB9720DB4F6C04F69F')
SET IDENTITY_INSERT [dbo].[EdmMetadata] OFF
/****** Object:  Table [dbo].[AircraftImage]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AircraftImage]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AircraftImage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AircraftId] [int] NOT NULL,
	[Url] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Descritpion] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Type] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK__Aircraft__3214EC0703317E3D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Member]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Member]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Member](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoginId] [int] NOT NULL,
	[Status] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FirstName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LastName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PrimaryEmail] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SecondaryEmail] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Phone] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AltPhone] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AddressLine_1] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AddressLine_2] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[City] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Zip] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MemberSince] [datetime] NULL,
	[LastMedical] [datetime] NULL,
	[TotalHours] [int] NULL,
	[RetractHours] [int] NULL,
	[EmergencyName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[EmergencyPhone] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PilotStatus] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FlightReviewLastDate] [datetime] NULL,
	[FlightReviewInstructorId] [int] NOT NULL,
	[FlightReviewNotes] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FlightReviewInstructrorName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BirthDate] [datetime] NULL,
	[Aircraft_Id] [int] NULL,
 CONSTRAINT [PK__Member__3214EC071273C1CD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[Member] ON
INSERT [dbo].[Member] ([Id], [LoginId], [Status], [FirstName], [LastName], [PrimaryEmail], [SecondaryEmail], [Phone], [AltPhone], [AddressLine_1], [AddressLine_2], [City], [Zip], [MemberSince], [LastMedical], [TotalHours], [RetractHours], [EmergencyName], [EmergencyPhone], [PilotStatus], [FlightReviewLastDate], [FlightReviewInstructorId], [FlightReviewNotes], [FlightReviewInstructrorName], [BirthDate], [Aircraft_Id]) VALUES (1, 1, N'Active', N'Frank', N'Zappa', N'admin@test.com', NULL, NULL, NULL, N'1234 Main St', NULL, N'Plano', N'75035', NULL, CAST(0x00009F9901614E8F AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Member] ([Id], [LoginId], [Status], [FirstName], [LastName], [PrimaryEmail], [SecondaryEmail], [Phone], [AltPhone], [AddressLine_1], [AddressLine_2], [City], [Zip], [MemberSince], [LastMedical], [TotalHours], [RetractHours], [EmergencyName], [EmergencyPhone], [PilotStatus], [FlightReviewLastDate], [FlightReviewInstructorId], [FlightReviewNotes], [FlightReviewInstructrorName], [BirthDate], [Aircraft_Id]) VALUES (2, 2, N'Active', N'Miles', N'Davis', N'miles.davis@test.com', NULL, NULL, NULL, N'1234 Main St', NULL, N'Seattle', N'23031', NULL, CAST(0x00009F9901614E99 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, 1)
INSERT [dbo].[Member] ([Id], [LoginId], [Status], [FirstName], [LastName], [PrimaryEmail], [SecondaryEmail], [Phone], [AltPhone], [AddressLine_1], [AddressLine_2], [City], [Zip], [MemberSince], [LastMedical], [TotalHours], [RetractHours], [EmergencyName], [EmergencyPhone], [PilotStatus], [FlightReviewLastDate], [FlightReviewInstructorId], [FlightReviewNotes], [FlightReviewInstructrorName], [BirthDate], [Aircraft_Id]) VALUES (3, 3, N'Active', N'Carlos', N'Santana', N'carlos.santana@test.com', NULL, NULL, NULL, N'1234 Poplar Ave', NULL, N'McKinney', N'750123', NULL, CAST(0x00009F9901614E99 AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Member] ([Id], [LoginId], [Status], [FirstName], [LastName], [PrimaryEmail], [SecondaryEmail], [Phone], [AltPhone], [AddressLine_1], [AddressLine_2], [City], [Zip], [MemberSince], [LastMedical], [TotalHours], [RetractHours], [EmergencyName], [EmergencyPhone], [PilotStatus], [FlightReviewLastDate], [FlightReviewInstructorId], [FlightReviewNotes], [FlightReviewInstructrorName], [BirthDate], [Aircraft_Id]) VALUES (4, 4, N'Active', N'Jimi', N'Hendrix', N'jimi.hendrix@test.com', NULL, NULL, NULL, N'1010 Addison Circle', NULL, N'Addison', N'750444', NULL, CAST(0x00009F3501614E9A AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Member] ([Id], [LoginId], [Status], [FirstName], [LastName], [PrimaryEmail], [SecondaryEmail], [Phone], [AltPhone], [AddressLine_1], [AddressLine_2], [City], [Zip], [MemberSince], [LastMedical], [TotalHours], [RetractHours], [EmergencyName], [EmergencyPhone], [PilotStatus], [FlightReviewLastDate], [FlightReviewInstructorId], [FlightReviewNotes], [FlightReviewInstructrorName], [BirthDate], [Aircraft_Id]) VALUES (5, 5, N'Active', N'Billy', N'Bathwater', N'billy@test.com', NULL, NULL, NULL, N'1234 Somewhere Lane', NULL, N'Beverly Hills', N'90210', NULL, CAST(0x00009F3501614E9B AS DateTime), NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL)

SET IDENTITY_INSERT [dbo].[Member] OFF
/****** Object:  Table [dbo].[StageCheck]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StageCheck]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[StageCheck](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PilotId] [int] NOT NULL,
	[InstructorId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[StageName] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK__StageChe__3214EC0729572725] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Squawk]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Squawk]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Squawk](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PostedOn] [datetime] NOT NULL,
	[AircraftId] [int] NOT NULL,
	[OriginatorId] [int] NOT NULL,
	[Subject] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[GroundAircraft] [bit] NOT NULL,
	[Response] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RespondedBy] [int] NULL,
	[RespondedOn] [datetime] NULL,
	[Status] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK__Squawk__3214EC0725869641] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Member_Role]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Member_Role]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Member_Role](
	[MemberId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK__Member_R__B45FE7F9164452B1] PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
INSERT [dbo].[Member_Role] ([MemberId], [RoleId]) VALUES (1, 1)
INSERT [dbo].[Member_Role] ([MemberId], [RoleId]) VALUES (2, 2)
INSERT [dbo].[Member_Role] ([MemberId], [RoleId]) VALUES (3, 2)
INSERT [dbo].[Member_Role] ([MemberId], [RoleId]) VALUES (4, 5)
INSERT [dbo].[Member_Role] ([MemberId], [RoleId]) VALUES (5, 3)
INSERT [dbo].[Member_Role] ([MemberId], [RoleId]) VALUES (6, 1)
/****** Object:  Table [dbo].[Reservation]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reservation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Reservation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[InstructorId] [int] NULL,
	[AircraftId] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Status] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK__Reservat__3214EC071DE57479] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[Reservation] ON
INSERT [dbo].[Reservation] ([Id], [MemberId], [InstructorId], [AircraftId], [StartDate], [EndDate], [Status]) VALUES (1, 2, NULL, 2, CAST(0x00009F9A0083D600 AS DateTime), CAST(0x00009F9A00A4CB80 AS DateTime), NULL)
INSERT [dbo].[Reservation] ([Id], [MemberId], [InstructorId], [AircraftId], [StartDate], [EndDate], [Status]) VALUES (2, 2, NULL, 2, CAST(0x00009F960083D600 AS DateTime), CAST(0x00009F9600A4CB80 AS DateTime), NULL)
INSERT [dbo].[Reservation] ([Id], [MemberId], [InstructorId], [AircraftId], [StartDate], [EndDate], [Status]) VALUES (3, 4, NULL, 1, CAST(0x00009F9801499700 AS DateTime), CAST(0x00009F990107AC00 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Reservation] OFF
/****** Object:  Table [dbo].[PilotCheckout]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PilotCheckout](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AircraftId] [int] NOT NULL,
	[PilotId] [int] NOT NULL,
	[InstructorId] [int] NOT NULL,
	[CheckoutDate] [datetime] NOT NULL,
 CONSTRAINT [PK__PilotChe__3214EC071A14E395] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[InstructorData]    Script Date: 11/12/2011 21:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InstructorData]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InstructorData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[CertificateNumber] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Ratings] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[InstructOnWeekends] [bit] NOT NULL,
	[InstructOnWeekdays] [bit] NOT NULL,
	[InstructOnWeekdayNights] [bit] NOT NULL,
	[AvailableForCheckoutsAnnuals] [bit] NOT NULL,
	[Comments] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DesignatedForStageChecks] [bit] NOT NULL,
 CONSTRAINT [PK__Instruct__3214EC070AD2A005] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
SET IDENTITY_INSERT [dbo].[InstructorData] ON
INSERT [dbo].[InstructorData] ([Id], [MemberId], [CertificateNumber], [Ratings], [InstructOnWeekends], [InstructOnWeekdays], [InstructOnWeekdayNights], [AvailableForCheckoutsAnnuals], [Comments], [DesignatedForStageChecks]) VALUES (1, 5, N'1234567890', N'CFI, CFII, MEI', 1, 0, 0, 1, NULL, 0)
SET IDENTITY_INSERT [dbo].[InstructorData] OFF
/****** Object:  ForeignKey [Aircraft_Images]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Aircraft_Images]') AND parent_object_id = OBJECT_ID(N'[dbo].[AircraftImage]'))
ALTER TABLE [dbo].[AircraftImage]  WITH CHECK ADD  CONSTRAINT [Aircraft_Images] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircraft] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Aircraft_Images]') AND parent_object_id = OBJECT_ID(N'[dbo].[AircraftImage]'))
ALTER TABLE [dbo].[AircraftImage] CHECK CONSTRAINT [Aircraft_Images]
GO
/****** Object:  ForeignKey [InstructorData_Member]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[InstructorData_Member]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstructorData]'))
ALTER TABLE [dbo].[InstructorData]  WITH CHECK ADD  CONSTRAINT [InstructorData_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[InstructorData_Member]') AND parent_object_id = OBJECT_ID(N'[dbo].[InstructorData]'))
ALTER TABLE [dbo].[InstructorData] CHECK CONSTRAINT [InstructorData_Member]
GO
/****** Object:  ForeignKey [Aircraft_Owners]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Aircraft_Owners]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member]'))
ALTER TABLE [dbo].[Member]  WITH CHECK ADD  CONSTRAINT [Aircraft_Owners] FOREIGN KEY([Aircraft_Id])
REFERENCES [dbo].[Aircraft] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Aircraft_Owners]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member]'))
ALTER TABLE [dbo].[Member] CHECK CONSTRAINT [Aircraft_Owners]
GO
/****** Object:  ForeignKey [Login_ClubMember]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Login_ClubMember]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member]'))
ALTER TABLE [dbo].[Member]  WITH CHECK ADD  CONSTRAINT [Login_ClubMember] FOREIGN KEY([LoginId])
REFERENCES [dbo].[Login] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Login_ClubMember]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member]'))
ALTER TABLE [dbo].[Member] CHECK CONSTRAINT [Login_ClubMember]
GO
/****** Object:  ForeignKey [Member_Roles_Source]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Member_Roles_Source]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member_Role]'))
ALTER TABLE [dbo].[Member_Role]  WITH CHECK ADD  CONSTRAINT [Member_Roles_Source] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Member_Roles_Source]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member_Role]'))
ALTER TABLE [dbo].[Member_Role] CHECK CONSTRAINT [Member_Roles_Source]
GO
/****** Object:  ForeignKey [Member_Roles_Target]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Member_Roles_Target]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member_Role]'))
ALTER TABLE [dbo].[Member_Role]  WITH CHECK ADD  CONSTRAINT [Member_Roles_Target] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Member_Roles_Target]') AND parent_object_id = OBJECT_ID(N'[dbo].[Member_Role]'))
ALTER TABLE [dbo].[Member_Role] CHECK CONSTRAINT [Member_Roles_Target]
GO
/****** Object:  ForeignKey [PilotCheckout_Aircraft]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout_Aircraft]') AND parent_object_id = OBJECT_ID(N'[dbo].[PilotCheckout]'))
ALTER TABLE [dbo].[PilotCheckout]  WITH CHECK ADD  CONSTRAINT [PilotCheckout_Aircraft] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircraft] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout_Aircraft]') AND parent_object_id = OBJECT_ID(N'[dbo].[PilotCheckout]'))
ALTER TABLE [dbo].[PilotCheckout] CHECK CONSTRAINT [PilotCheckout_Aircraft]
GO
/****** Object:  ForeignKey [PilotCheckout_Instructor]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout_Instructor]') AND parent_object_id = OBJECT_ID(N'[dbo].[PilotCheckout]'))
ALTER TABLE [dbo].[PilotCheckout]  WITH CHECK ADD  CONSTRAINT [PilotCheckout_Instructor] FOREIGN KEY([InstructorId])
REFERENCES [dbo].[Member] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout_Instructor]') AND parent_object_id = OBJECT_ID(N'[dbo].[PilotCheckout]'))
ALTER TABLE [dbo].[PilotCheckout] CHECK CONSTRAINT [PilotCheckout_Instructor]
GO
/****** Object:  ForeignKey [PilotCheckout_Pilot]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout_Pilot]') AND parent_object_id = OBJECT_ID(N'[dbo].[PilotCheckout]'))
ALTER TABLE [dbo].[PilotCheckout]  WITH CHECK ADD  CONSTRAINT [PilotCheckout_Pilot] FOREIGN KEY([PilotId])
REFERENCES [dbo].[Member] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[PilotCheckout_Pilot]') AND parent_object_id = OBJECT_ID(N'[dbo].[PilotCheckout]'))
ALTER TABLE [dbo].[PilotCheckout] CHECK CONSTRAINT [PilotCheckout_Pilot]
GO
/****** Object:  ForeignKey [Reservation_Aircraft]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Reservation_Aircraft]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservation]'))
ALTER TABLE [dbo].[Reservation]  WITH CHECK ADD  CONSTRAINT [Reservation_Aircraft] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircraft] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Reservation_Aircraft]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservation]'))
ALTER TABLE [dbo].[Reservation] CHECK CONSTRAINT [Reservation_Aircraft]
GO
/****** Object:  ForeignKey [Reservation_Instructor]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Reservation_Instructor]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservation]'))
ALTER TABLE [dbo].[Reservation]  WITH CHECK ADD  CONSTRAINT [Reservation_Instructor] FOREIGN KEY([InstructorId])
REFERENCES [dbo].[Member] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Reservation_Instructor]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservation]'))
ALTER TABLE [dbo].[Reservation] CHECK CONSTRAINT [Reservation_Instructor]
GO
/****** Object:  ForeignKey [Reservation_Member]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Reservation_Member]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservation]'))
ALTER TABLE [dbo].[Reservation]  WITH CHECK ADD  CONSTRAINT [Reservation_Member] FOREIGN KEY([MemberId])
REFERENCES [dbo].[Member] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Reservation_Member]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reservation]'))
ALTER TABLE [dbo].[Reservation] CHECK CONSTRAINT [Reservation_Member]
GO
/****** Object:  ForeignKey [Squawk_Aircraft]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Squawk_Aircraft]') AND parent_object_id = OBJECT_ID(N'[dbo].[Squawk]'))
ALTER TABLE [dbo].[Squawk]  WITH CHECK ADD  CONSTRAINT [Squawk_Aircraft] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircraft] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Squawk_Aircraft]') AND parent_object_id = OBJECT_ID(N'[dbo].[Squawk]'))
ALTER TABLE [dbo].[Squawk] CHECK CONSTRAINT [Squawk_Aircraft]
GO
/****** Object:  ForeignKey [Squawk_Originator]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Squawk_Originator]') AND parent_object_id = OBJECT_ID(N'[dbo].[Squawk]'))
ALTER TABLE [dbo].[Squawk]  WITH CHECK ADD  CONSTRAINT [Squawk_Originator] FOREIGN KEY([OriginatorId])
REFERENCES [dbo].[Member] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Squawk_Originator]') AND parent_object_id = OBJECT_ID(N'[dbo].[Squawk]'))
ALTER TABLE [dbo].[Squawk] CHECK CONSTRAINT [Squawk_Originator]
GO
/****** Object:  ForeignKey [StageCheck_Pilot]    Script Date: 11/12/2011 21:45:33 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[StageCheck_Pilot]') AND parent_object_id = OBJECT_ID(N'[dbo].[StageCheck]'))
ALTER TABLE [dbo].[StageCheck]  WITH CHECK ADD  CONSTRAINT [StageCheck_Pilot] FOREIGN KEY([PilotId])
REFERENCES [dbo].[Member] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[StageCheck_Pilot]') AND parent_object_id = OBJECT_ID(N'[dbo].[StageCheck]'))
ALTER TABLE [dbo].[StageCheck] CHECK CONSTRAINT [StageCheck_Pilot]
GO
