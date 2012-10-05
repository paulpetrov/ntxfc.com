use ntxfc_main;

drop table if exists Member_Role;
drop table if exists Role;
drop table if exists InstructorInfo;
drop table if exists InstructorTime;
drop table if exists Instructor_Aircraft;
drop table if exists Reservation;
drop table if exists SquawkComment;
drop table if exists Squawk;
drop table if exists AircraftOwner;
drop table if exists MemberCheckout;
drop table if exists Member;
drop table if exists Login;
drop table if exists AircraftImage;
drop table if exists Aircraft;

create table Login (
	Id int(10) unsigned not null primary key auto_increment,
	Username varchar(50) not null,
	Password varchar(20) not null,
	Status varchar(20)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

insert into Login (Username, Password) values 
	('admin','password1');

create table Member (
	Id int(10) unsigned not null primary key auto_increment,
	LoginId int(10) unsigned not null,
	foreign key (LoginId) references Login(Id) on delete cascade,
	Status varchar(20),
	FirstName varchar(20),
	LastName varchar(50),
	PrimaryEmail varchar(50) not null,
	SecondaryEmail varchar(50),
	Phone varchar(12),
	AltPhone varchar(12),
	AddressLine_1 varchar(100),
	AddressLine_2 varchar(100),
	City varchar(100),
	Zip varchar(5),
	MemberSince date,
	LastMedical date,
	TotalHours int,
	RetractHours int,
	EmergencyName varchar(100),
	EmergencyPhone varchar(12)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

create table Role (
	Id int(10) unsigned not null primary key auto_increment,
	Name varchar(32),
	Description varchar(200)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

insert into Role (Name, Description) values 
	('Admin','Site Administrator'),
	('Editor', 'Web site content editor'),
	('Owner', 'Airplane Owner'),
	('Instructor', 'Designated club instructor'),
	('Member', 'Club Member'),
	('Guest', 'Guest account');

create table Member_Role (
	MemberId int(10) unsigned not null,
	RoleId int(10) unsigned not null,
	index (MemberId),
	foreign key (MemberId) references Member(Id) on delete cascade,
	index (RoleId),
	foreign key (RoleId) references Role(Id) on delete cascade
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

create table InstructorInfo (
	InstructorId int(10) unsigned not null,
	CertificateNumber varchar(50),
	Ratings varchar(100)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

create table InstructorTime (
	InstructorId int(10) unsigned not null,
	MemberId int(10) unsigned not null,
	index(InstructorId),
	foreign key (InstructorId) references Member(Id) on delete cascade,
	index(MemberId),
	foreign key (MemberId) references Member(Id) on delete cascade
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

create table Aircraft (
	Id int(10) unsigned not null primary key auto_increment,
	RegistrationNumber varchar(10) not null,
	Model varchar(50),
	TypeDesignation varchar(30),
	Category varchar(30),
	AircraftClass varchar(30),
	MaxGrossWeight int,
	FuelCapacity smallint,
	UsefulLoad int,
	CruiseSpeed smallint,
	MaxRange smallint,
	Description varchar(250),
	EquipmentList varchar(250),
	IrCertified bit(1),
	CheckoutRequirements varchar(250),
	HourlyRate smallint
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

insert into Aircraft (RegistrationNumber, Model, TypeDesignation) values 
	('N8185E','Piper Arrow IV', 'PA28RT-201');

create table AircraftOwner (
	OwnerId int(10) unsigned not null,
	AircraftId int(10) unsigned not null,
	index(OwnerId),
	foreign key (OwnerId) references Member(Id) on delete cascade,
	index(AircraftId),
	foreign key (AircraftId) references Aircraft(Id) on delete cascade
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

create table Instructor_Aircraft (
	InstructorId int(10) unsigned not null,
	index(InstructorId),
	foreign key (InstructorId) references Member(Id) on delete cascade,
	AircraftId int(10) unsigned not null,
	index(AircraftId),
	foreign key (AircraftId) references Aircraft(Id) on delete cascade
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

create table AircraftImage (
	Id int(10) unsigned not null primary key auto_increment,
	AircraftId int(10) unsigned not null,
	ImageType varchar(20),
	Url varchar(250),
	Description varchar(500),
	index(AircraftId),
	foreign key (AircraftId) references Aircraft(Id) on delete cascade
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

create table MemberCheckout (
	Id int(10) unsigned not null primary key auto_increment,
	MemberId int(10) unsigned not null,
	AircraftId int(10) unsigned not null,
	index (MemberId),
	index (AircraftId),
	foreign key (MemberId) references Member(Id) on delete cascade,
	foreign key (AircraftId) references Aircraft(Id) on delete cascade
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

create table Reservation (
	Id int(10) unsigned not null primary key auto_increment,
	MemberId int(10) unsigned not null,
	AircraftId int(10) unsigned not null,
	StartTime datetime not null,
	EndTime datetime not null,
	Status varchar(20),
	index (MemberId),
	index (AircraftId),
	foreign key (MemberId) references Member(Id) on delete cascade,
	foreign key (AircraftId) references Aircraft(Id) on delete cascade
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

create table Squawk (
	Id int(10) unsigned not null primary key auto_increment,
	PostedOn date not null,
	AircraftId int(10) unsigned not null,
	PosterId int(10) unsigned not null,
	Description varchar(500),
	Response varchar(500),
	RespondedBy int,
	RespondedOn date,
	Status varchar(16),
	index(PosterId),
	foreign key (PosterId) references Member(Id) on delete cascade,
	index (AircraftId),
	foreign key (AircraftId) references Aircraft(Id) on delete cascade
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

create table SquawkComment (
	Id int(10) unsigned not null primary key auto_increment,
	SquawkId int(10) unsigned not null,
	PostedOn date not null,
	PosterId int(10) unsigned not null,
	Text varchar(500),
	index(PosterId),
	foreign key (PosterId) references Member(Id) on delete cascade,
	index (SquawkId),
	foreign key (SquawkId) references Squawk(Id) on delete cascade
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;




