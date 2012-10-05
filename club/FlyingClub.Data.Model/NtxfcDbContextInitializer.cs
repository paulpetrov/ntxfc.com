using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using FlyingClub.Data.Model.Entities;
using FlyingClub.Common;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace FlyingClub.Data.Model
{
    /// <summary>
    /// Use strategies: DropCreateDatabaseIfModelChanges<> or DropCreateDatabaseAlways<>
    /// </summary>
    public class NtxfcDbContextInitializer : DropCreateDatabaseIfModelChanges<NtxfcDbContext>
    {
        private NtxfcDbContext _dbContext;

        protected override void Seed(NtxfcDbContext context)
        {
            _dbContext = context;

            CreateRoles();
            CreateLoginsAndMembers();
            CreateAircraft();
            CreateReservation();

            AccessDbImporter oldDbImport = new AccessDbImporter();
        }

        public void CreateLoginsAndMembers()
        {
            Role roleAdmin = _dbContext.Roles.First(r => r.Id == (int)UserRoles.Admin);
            Role rolePilot = _dbContext.Roles.FirstOrDefault(r => r.Id == (int)UserRoles.Pilot);
            Role roleInstructor = _dbContext.Roles.FirstOrDefault(r => r.Id == (int)UserRoles.Instructor);
            Role roleOwner = _dbContext.Roles.First(r => r.Id == (int)UserRoles.AircraftOwner);

            string salt = SimpleHash.GetSalt(32);
            string hash = SimpleHash.MD5("password1", salt);
            Login loginAdmin = new Login()
            {
                Username = "admin",
                Password = SimpleHash.MD5("password1", salt),
                PasswordSalt = salt,
                MemberPIN = "1110",
                Email = "admin@test.com"
            };

            try
            {
                _dbContext.Logins.Add(loginAdmin);
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var e in ex.EntityValidationErrors)
                {
                    Debug.WriteLine(e.Entry);
                }
                throw;
            }

            Member memberAdmin = new Member()
            {
                Status = "Active",
                AddressLine_1 = "1234 Main St",
                City = "Plano",
                Zip = "75035",
                FirstName = "Frank",
                LastName = "Zappa",
                LastMedical = DateTime.Now,
                //PrimaryEmail = "admin@test.com",
                LoginId = loginAdmin.Id,
                Roles = new List<Role>() { roleAdmin }
            };
            _dbContext.Members.Add(memberAdmin);
            _dbContext.SaveChanges();
           
            Login loginOwner1 = new Login()
            {
                Username = "owner1",
                Email = "owner1@ntxfc.com",
                Password = SimpleHash.MD5("test", salt),
                PasswordSalt = salt,
                MemberPIN = "1211"
            };
            _dbContext.Logins.Add(loginOwner1);
            _dbContext.SaveChanges();

            Member memberOwner1 = new Member()
            {
                Status = "Active",
                AddressLine_1 = "1234 Main St",
                City = "Beverly Hills",
                Zip = "23031",
                FirstName = "John",
                LastName = "Travolta",
                LastMedical = DateTime.Now,
                //PrimaryEmail = "johnny.t@test.com",
                LoginId = loginOwner1.Id,
                Roles = new List<Role>() { roleOwner }
            };
            _dbContext.Members.Add(memberOwner1);

            Login loginOwner2 = new Login()
            {
                Username = "owner2",
                Email = "owner2@ntxfc.com",
                Password = SimpleHash.MD5("test", salt),
                PasswordSalt = salt,
                MemberPIN = "1351"
            };
            _dbContext.Logins.Add(loginOwner2);
            _dbContext.SaveChanges();

            Member memberOwner2 = new Member()
            {
                Status = "Active",
                AddressLine_1 = "1234 Poplar Ave",
                City = "Santa Monica",
                Zip = "450123",
                FirstName = "Harrison",
                LastName = "Ford",
                LastMedical = DateTime.Now,
                //PrimaryEmail = "harrison.ford@test.com",
                LoginId = loginOwner2.Id,
                Roles = new List<Role>() { roleOwner }
            };
            _dbContext.Members.Add(memberOwner2);
            
            Login loginPilot1 = new Login()
            {
                Username = "pilot1",
                Email = "pilot1@ntxfc.com",
                Password = SimpleHash.MD5("test", salt),
                PasswordSalt = salt,
                MemberPIN = "1525"
            };
            _dbContext.Logins.Add(loginPilot1);
            _dbContext.SaveChanges();

            Member memberPilot1 = new Member()
            {
                Status = "Active",
                AddressLine_1 = "1010 Addison Circle",
                City = "Addison",
                Zip = "750444",
                FirstName = "Bob",
                LastName = "Hoover",
                LastMedical = DateTime.Now.AddDays(-100),
                //PrimaryEmail = "bob.hoover@test.com",
                LoginId = loginPilot1.Id,
                Roles = new List<Role>() { rolePilot }
            };
            _dbContext.Members.Add(memberPilot1);
            _dbContext.SaveChanges();

            Login loginInstructor1 = new Login()
            {
                Username = "instructor1",
                Email = "instructor1@ntxfc.com",
                Password = SimpleHash.MD5("test", salt),
                PasswordSalt = salt,
                MemberPIN = "1010"
            };
            _dbContext.Logins.Add(loginInstructor1);
            _dbContext.SaveChanges();

            Member memberInstructor1 = new Member()
            {
                Status = "Active",
                AddressLine_1 = "1234 Somewhere Lane",
                City = "Beverly Hills",
                Zip = "90210",
                FirstName = "Billy",
                LastName = "Bathwater",
                LastMedical = DateTime.Now.AddDays(-100),
                //PrimaryEmail = "billy@test.com",
                LoginId = loginInstructor1.Id,
                Roles = new List<Role>() { roleInstructor }
            };

            _dbContext.Members.Add(memberInstructor1);
            _dbContext.SaveChanges();

            InstructorData instructor1Data = new InstructorData()
            {
                AvailableForCheckoutsAnnuals = true,
                CertificateNumber = "1234567890",
                DesignatedForStageChecks = false,
                InstructOnWeekdayNights = false,
                InstructOnWeekdays = false,
                InstructOnWeekends = true,
                Member = memberInstructor1,
                Ratings = "CFI, CFII, MEI"
            };

            _dbContext.InstructorData.Add(instructor1Data);
            _dbContext.SaveChanges();

            Login loginGuest = new Login()
            {
                Username = "guest",
                Email = "guest@ntxfc.com",
                Password = SimpleHash.MD5("password1", salt),
                PasswordSalt = salt,
                ForumUserId = 179,
                MemberPIN = "1790"
            };

            _dbContext.Logins.Add(loginGuest);
            _dbContext.SaveChanges();

            Login jeremyLogin = new Login()
            {
                Username = "jeremyw",
                Password = SimpleHash.MD5("2g4uFOOl", "6YA+Ie1h2GLV1GU/K5EobHfSm4GPpXgAm+BbICN2RvM="),
                PasswordSalt = "6YA+Ie1h2GLV1GU/K5EobHfSm4GPpXgAm+BbICN2RvM=",
                ForumUserId = 1,
                Email = "jeremy.whittington@gmail.com",
                MemberPIN = "1530"
            };
            _dbContext.Logins.Add(jeremyLogin);
            _dbContext.SaveChanges();

            Member jeremyMember = new Member()
            {
                Status = "Active",
                AddressLine_1 = "2511 Cheverny Dr",
                City = "McKinney",
                Zip = "75070",
                FirstName = "Jeremy",
                LastName = "Whittington",
                LastMedical = DateTime.Now,
                //PrimaryEmail = "jeremy.whittington@gmail.com",
                LoginId = jeremyLogin.Id,
                Roles = new List<Role>() { roleAdmin }
            };
            _dbContext.Members.Add(jeremyMember);
            _dbContext.SaveChanges();
        }

        public void CreateAircraft()
        {
            Login login1 = _dbContext.Logins.First(o => o.Username.Contains("owner1"));
            Member owner1 = _dbContext.Members.First(o => o.LoginId == login1.Id);

            Login login2 = _dbContext.Logins.First(o => o.Username.Contains("owner2"));
            Member owner2 = _dbContext.Members.First(o => o.LoginId == login2.Id);
            Aircraft a1 = new Aircraft()
            {
                Model = "Gulfstream II",
                Year = "SEL",
                Make = "Grumman",
                CheckoutRequirements = "Type Rating",
                CruiseSpeed = 390,
                FuelCapacity = 1000,
                HourlyRate = 4000,
                MaxGrossWeight = 65500,
                MaxRange = 4000,
                RegistrationNumber = "N728T",
                Name="Gulfstream",
                TypeDesignation = "G-II",
                UsefulLoad = 15000,
                Owners = new List<Member>() { owner1 },
                Status = AircraftStatus.Online.ToString()
            };

            _dbContext.Aircraft.Add(a1);

            Aircraft a2 = new Aircraft()
            {
                Model = "DHC-2",
                Year = "SEL",
                Make = "de Havilland",
                CheckoutRequirements = "Fly",
                CruiseSpeed = 135,
                FuelCapacity = 200,
                HourlyRate = 180,
                MaxGrossWeight = 5100,
                MaxRange = 400,
                RegistrationNumber = "N28S",
                Name = "Beaver",
                TypeDesignation = "DHC-2",
                UsefulLoad = 2000,
                Owners = new List<Member>() { owner2 },
                Status = AircraftStatus.Grounded.ToString()
            };

            _dbContext.Aircraft.Add(a2);

            _dbContext.SaveChanges();
        }

        public void CreateRoles()
        {
            Role role1 = new Role()
            {
                Id = (int)UserRoles.Admin,
                Name = UserRoles.Admin.ToString(),
                Description = "Site Administrator"
            };
            _dbContext.Roles.Add(role1);

            Role role2 = new Role()
            {
                Id = (int)UserRoles.AircraftOwner,
                Name = UserRoles.AircraftOwner.ToString(),
                Description = "Aircraft Owner"
            };
            _dbContext.Roles.Add(role2);

            Role role3 = new Role()
            {
                Id = (int)UserRoles.Instructor,
                Name = FlyingClub.Common.UserRoles.Instructor.ToString(),
                Description = "Club Instructor"
            };
            _dbContext.Roles.Add(role3);

            Role role4 = new Role()
            {
                Id = (int)UserRoles.SiteEditor,
                Name = UserRoles.SiteEditor.ToString(),
                Description = "Web site content editor"
            };
            _dbContext.Roles.Add(role4);

            Role role5 = new Role()
            {
                Id = (int)UserRoles.Pilot,
                Name = UserRoles.Pilot.ToString(),
                Description = "Club Member"
            };
            _dbContext.Roles.Add(role5);

            Role role6 = new Role()
            {
                Id = (int)UserRoles.AircraftMaintenance,
                Name = UserRoles.AircraftMaintenance.ToString(),
                Description = "Aircraft Maintenance Personnel"
            };
            _dbContext.Roles.Add(role6);
            
            
            _dbContext.SaveChanges();
        }

        public void CreateReservation()
        {
            Login login1 = _dbContext.Logins.First(o => o.Username.Contains("owner1"));
            Member member1 = _dbContext.Members.First(o => o.LoginId == login1.Id);

            Login login2 = _dbContext.Logins.First(o => o.Username.Contains("pilot1"));
            Member member2 = _dbContext.Members.First(o => o.LoginId == login2.Id);

            Aircraft aircraft1 = _dbContext.Aircraft.First(x => x.RegistrationNumber == "N728T");
            Aircraft aircraft2 = _dbContext.Aircraft.First(x => x.RegistrationNumber == "N28S");

            Reservation reservation1 = new Reservation()
            {
                Member = member1,
                Aircraft = aircraft1,
                StartDate = DateTime.Now.Date.AddDays(1).AddHours(8),
                EndDate = DateTime.Now.Date.AddDays(1).AddHours(10)
            };

            _dbContext.Reservations.Add(reservation1);

            Reservation reservation2 = new Reservation()
            {
                Member = member1,
                Aircraft = aircraft1,
                StartDate = DateTime.Now.Date.AddDays(-3).AddHours(8),
                EndDate = DateTime.Now.Date.AddDays(-3).AddHours(10)
            };

            _dbContext.Reservations.Add(reservation2);

            Reservation reservation3 = new Reservation()
            {
                Member = member2,
                Aircraft = aircraft2,
                StartDate = DateTime.Now.Date.AddDays(-1).AddHours(20),
                EndDate = DateTime.Now.Date.AddHours(16)
            };

            _dbContext.Reservations.Add(reservation3);

            _dbContext.SaveChanges();
        }
    }
}
