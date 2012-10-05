using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Objects;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.Diagnostics;

using FlyingClub.Data.Model.Entities;
using FlyingClub.Data.Model.OldSchema;
using FlyingClub.Common;
using FlyingClub.Data.Model.External;

namespace FlyingClub.Data.Model
{
    public class AccessDbImporter
    {
        private NtfcDataSet _dataSet;
        private NtxfcDbContext _dbContext;

        public AccessDbImporter()
        {
            _dbContext = new NtxfcDbContext();
            LoadSourceData();
            ImportUsers();
            ImportAircraft();
            ImportSchedule();
            ImportInstructors();
            ImportReviews();
            ImportSquawks();
            ImportCheckouts();
        }

        private void LoadSourceData()
        {
            try
            {
                _dataSet = new NtfcDataSet();
                using (OleDbConnection cn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\\Users\\genpetrp\\Projects\\NTXFC\\public_html\\Data\\ntfc.mdb;User ID=Admin;Password="))
                {
                    string selectFormat = "SELECT * From {0}";
                    OleDbCommand cmd = new OleDbCommand("", cn);
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

                    foreach (DataTable table in _dataSet.Tables)
                    {
                        string tableName = table.TableName;
                        adapter.SelectCommand.CommandText = String.Format(selectFormat, tableName);
                        adapter.Fill(_dataSet, tableName);
                    }   
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void ImportUsers()
        {
            IEnumerator<NtfcDataSet.LoginRow> srcLogins = _dataSet.Login.GetEnumerator();
            while (srcLogins.MoveNext())
            {
                var srcLogin = srcLogins.Current;
                NtfcDataSet.MembershipRow srcMember = _dataSet.Membership.FirstOrDefault(r => r.Member_ID == srcLogin.Member_ID);

                // drop members that don't have logins
                if (srcMember == null)
                {
                    Debug.WriteLine("Member " + srcLogin.Member_ID + " not found.");
                    continue;
                }

                if (srcMember.IsFirst_NameNull())
                    continue;

                if (srcMember.First_Name.Trim() == String.Empty || srcMember.First_Name.Contains("--"))
                    continue;

                //if (!srcMember.Status)
                //    continue;    
                    
                Login login = new Login();
                login.MemberPIN = srcLogin.Member_ID.ToString();

                string username = (srcMember.First_Name.Trim() + srcMember.Last_Name.Trim()).ToLower();
                username = Regex.Replace(username, @"[\W]", String.Empty);
                login.Username = username;
                login.PasswordSalt = SimpleHash.GetSalt(32);
                login.Password = SimpleHash.MD5(srcLogin.Login_Password, login.PasswordSalt);
                login.FirstName = srcMember.First_Name;
                login.LastName = srcMember.Last_Name;
                login.Email = !srcMember.IsEmailNull() ? srcMember.Email : null;
                login.Email2 = !srcMember.Isemail2Null() ? srcMember.email2 : null;

                Member member = new Member();
                if (srcMember.Status)
                    member.Status = MemberStatus.Active.ToString();
                else
                    member.Status = MemberStatus.Removed.ToString();
                member.AddressLine_1 = !srcMember.IsAddressNull() ? srcMember.Address : null;
                member.Phone = !srcMember.IsHome_PhoneNull() ? srcMember.Home_Phone : null;
                member.AltPhone = !srcMember.IsWork_PhoneNull() ? srcMember.Work_Phone : null;
                member.City = !srcMember.IsCityNull() ? srcMember.City : null;
                member.State = !srcMember.IsStateNull() ? srcMember.State : null;
                member.Zip = !srcMember.IsZipNull() ? srcMember.Zip : null;
                //member.PrimaryEmail = !srcMember.IsEmailNull() ? srcMember.Email : null;
                //member.SecondaryEmail = !srcMember.Isemail2Null() ? srcMember.email2 : null;
                member.EmergencyName = !srcMember.IsE_First_NameNull() ? srcMember.E_First_Name + " " + srcMember.E_Last_Name : null;
                member.EmergencyPhone = !srcMember.IsE_Home_PhoneNull() ? srcMember.E_Home_Phone : null;
                member.FirstName = srcMember.First_Name;
                member.LastName = srcMember.Last_Name;
                member.LastMedical = !srcMember.IsMedical_DateNull() ? (DateTime?)srcMember.Medical_Date : null;
                member.MemberSince = srcMember.Membership_Date;
                if (!srcMember.IsBirthdateNull())
                {
                    DateTime dob = DateTime.MinValue;
                    if (DateTime.TryParse(srcMember.Birthdate, out dob))
                        member.BirthDate = dob;
                }

                member.LastUpdated = srcMember.Last_Update;

                member.PilotStatus = !srcMember.IsPilot_statusNull() ? srcMember.Pilot_status : null;
                member.Roles = new List<Role>();
                member.Roles.Add(_dbContext.Roles.First(r => r.Id == 5));
                if (login.MemberPIN == "547" || login.MemberPIN == "615")
                    member.Roles.Add(_dbContext.Roles.First(r => r.Id == 1));

                //Role instructorRole = _dbContext.Roles.First(r => r.Name.Contains("Instructor"));
                //if(login.OldMemberId == "703" || 
                //    login.OldMemberId == "587" || 
                //    login.OldMemberId == "199" || 
                //    login.OldMemberId == "296" ||
                //    login.OldMemberId == "359" ||
                //    login.OldMemberId == "655" ||
                //    login.OldMemberId == "727" ||
                //    login.OldMemberId == "618")
                //{
                //    member.Roles.Add(instructorRole);
                //}

                login.ClubMember = new List<Member>();
                login.ClubMember.Add(member);
                _dbContext.Logins.Add(login);
                //_dbContext.Members.Add(member);
              

                // insert into forum database
                //User user = new User();
                //user.password = srcLogin.Login_Password;
                //using (ExternalDbContext db = new ExternalDbContext())
                //{
                //    db.Users.Add(user);
                //    db.SaveChanges();

                //    string sqlCommand = String.Format("INSERT INTO `vb3_userfield` (userid) VALUES({0})", user.userid);
                //    db.Database.ExecuteSqlCommand(sqlCommand);
                //    db.SaveChanges();

                //    string sqlCommand2 = String.Format("INSERT INTO `vb3_useractivation` (userid, activationid) VALUES({0},'test')", user.userid);
                //    db.Database.ExecuteSqlCommand(sqlCommand2);
                //    db.SaveChanges();
                //}               
            }

            _dbContext.SaveChanges();
        }

        //public void ImportMembers()
        //{
        //    IEnumerator<NtfcDataSet.MembershipRow> rows = _dataSet.Membership.GetEnumerator();
        //    while (rows.MoveNext())
        //    {
        //        var srcMember = rows.Current;
        //        Login login = _dbContext.Logins.FirstOrDefault(l => l.Username == srcMember.Member_ID);
        //        // skip users that don't have logins
        //        if (login == null)
        //            continue;

        //        if (srcMember.IsFirst_NameNull())
        //            continue;

        //        if (srcMember.First_Name.Trim() == String.Empty || srcMember.First_Name == "----")
        //            continue;

        //        Member member = new Member();
        //        member.LoginId = login.Id;
        //        member.AddressLine_1 = !srcMember.IsAddressNull() ? srcMember.Address : null;
        //        member.Phone = !srcMember.IsHome_PhoneNull() ? srcMember.Home_Phone : null;
        //        member.AltPhone = !srcMember.IsWork_PhoneNull() ? srcMember.Work_Phone : null;
        //        member.City = !srcMember.IsCityNull() ? srcMember.City : null;
        //        member.Zip = !srcMember.IsZipNull() ? srcMember.Zip : null;
        //        member.PrimaryEmail = !srcMember.IsEmailNull() ? srcMember.Email : null;
        //        member.SecondaryEmail = !srcMember.Isemail2Null() ? srcMember.email2 : null;
        //        member.EmergencyName = !srcMember.IsE_First_NameNull() ? srcMember.E_First_Name + srcMember.E_Last_Name : null;
        //        member.EmergencyPhone = !srcMember.IsE_Home_PhoneNull() ? srcMember.E_Home_Phone : null;
        //        member.FirstName = srcMember.First_Name;
        //        member.LastName = srcMember.Last_Name;
        //        member.LastMedical = !srcMember.IsMedical_DateNull() ? (DateTime?)srcMember.Medical_Date : null; 
        //        member.MemberSince = srcMember.Membership_Date;
        //        if (!srcMember.IsBirthdateNull())
        //        {
        //            DateTime dob = DateTime.MinValue;
        //            if (DateTime.TryParse(srcMember.Birthdate, out dob))
        //                member.BirthDate = dob;
        //        }
        //        member.PilotStatus = !srcMember.IsPilot_statusNull() ? srcMember.Pilot_status : null;

        //        _dbContext.Members.Add(member);
        //    }
        //    _dbContext.SaveChanges();
        //}

        public void ImportAircraft()
        {
            IEnumerator<NtfcDataSet.AircraftRow> rows = _dataSet.Aircraft.GetEnumerator();
            Role ownerRole = _dbContext.Roles.First(r => r.Name.Contains("Owner"));
            while (rows.MoveNext())
            {
                var row = rows.Current;
                if (row.Tail_No == "N971BG" || row.Tail_No == "N2590H")
                    continue;

                Aircraft aircraft = new Aircraft();
                aircraft.Year = "";
                aircraft.Make = "";
                aircraft.CheckoutRequirements = row.Checkout;
                aircraft.CruiseSpeed = Convert.ToInt16(row.Speed);
                aircraft.Description = row.Description;
                aircraft.EquipmentList = row.Equipment;
                aircraft.FuelCapacity = Convert.ToInt16(row.FuelCap);
                aircraft.HourlyRate = Convert.ToInt16(row.Hourly_Rate);
                aircraft.MaxGrossWeight = Convert.ToInt16(row.MaxGrossWgt);
                aircraft.MaxRange = Convert.ToInt16(row.Range);
                aircraft.Model = row.Model;
                aircraft.Name = String.Empty;
                aircraft.RegistrationNumber = row.Tail_No;
                aircraft.Status = AircraftStatus.Online.ToString();
                aircraft.StatusNotes = String.Empty;
                aircraft.TypeDesignation = String.Empty;
                aircraft.UsefulLoad = (int)Convert.ToDouble(row.UsefulLoad);
                aircraft.TypeDesignation = row.AC_Type;
                aircraft.BasedAt = "TKI";
        
                Login owner1 = _dbContext.Logins.Where(l => l.MemberPIN == row.Owner_MID).Include(l => l.ClubMember).FirstOrDefault();
                owner1.ClubMember.First().Roles.Add(ownerRole);
                aircraft.Owners = new List<Member>();
                aircraft.Owners.Add(owner1.ClubMember.First());

                if (!row.Isowner2_MIDNull())
                {
                    string id = row.owner2_MID;
                    Login owner2 = _dbContext.Logins.Where(l => l.MemberPIN == id).Include(l => l.ClubMember).FirstOrDefault();
                    if (owner2 != null)
                    {
                        owner2.ClubMember.First().Roles.Add(ownerRole);
                        aircraft.Owners.Add(owner2.ClubMember.First());
                    }     
                }

                _dbContext.Aircraft.Add(aircraft);
                _dbContext.SaveChanges();               
            }
        }

        public void ImportInstructors()
        {
            IEnumerator<NtfcDataSet.InstructorIDRow> rows = _dataSet.InstructorID.GetEnumerator();
            Role instructorRole = _dbContext.Roles.First(r => r.Name.Contains("Instructor"));

            while (rows.MoveNext())
            {
                NtfcDataSet.InstructorIDRow row = rows.Current;

                if (row.Instructor_MID == "537" || row.Instructor_MID == "547" || row.Instructor_MID == "734" || row.Instructor_MID == "735" || row.Instructor_MID == "999")
                    continue;

                InstructorData instructor = new InstructorData();
                if (!row.IsannualsNull())
                    instructor.AvailableForCheckoutsAnnuals = row.annuals.ToLower() == "yes" ? true : false;
                else
                    instructor.AvailableForCheckoutsAnnuals = false;
                instructor.CertificateNumber = String.Empty;
                if(!row.IsnotesNull())
                    instructor.Comments = row.notes;                                                         
                instructor.DesignatedForStageChecks = false;
                if (!row.IsdaysNull())
                    instructor.InstructOnWeekdays = row.days.ToLower() == "yes" ? true : false;
                else
                    instructor.InstructOnWeekdays = false;
                if (!row.IseveningsNull())
                    instructor.InstructOnWeekdayNights = row.evenings.ToLower() == "yes" ? true : false;
                else
                    instructor.InstructOnWeekdayNights = false;
                if (!row.IsweekendsNull())
                    instructor.InstructOnWeekends = row.weekends.ToLower() == "yes" ? true : false;
                else
                    instructor.InstructOnWeekends = false;
                instructor.Ratings = String.Empty;

                Login login = _dbContext.Logins.Where(l => l.MemberPIN == row.Instructor_MID).Include(l => l.ClubMember).FirstOrDefault();
                if (login == null)
                    continue;

                Member member = login.ClubMember.First();
                member.Roles.Add(instructorRole);
                instructor.Member = member;
                _dbContext.InstructorData.Add(instructor);

                _dbContext.SaveChanges();
            }
        }

        public void ImportCheckouts()
        {
            IDictionary<string, List<int>> acMap = new Dictionary<string, List<int>>();
            List<int> ids = _dbContext.Aircraft.Where(a => a.RegistrationNumber == "N49649" || a.RegistrationNumber == "N4952B")
                .Select(a => a.Id).ToList();
            acMap.Add("C150", ids);
            acMap.Add("C152", ids);

            ids = _dbContext.Aircraft.Where(a => a.RegistrationNumber == "N73192")
                .Select(a => a.Id).ToList();
            acMap.Add("C172", ids);

            ids = _dbContext.Aircraft.Where(a => a.RegistrationNumber == "N2099V")
                .Select(a => a.Id).ToList();
            acMap.Add("C120", ids);

            ids = _dbContext.Aircraft.Where(a => a.RegistrationNumber == "N8142H")
                .Select(a => a.Id).ToList();
            acMap.Add("PA28-161", ids);
            acMap.Add("PA28-181", ids);

            ids = _dbContext.Aircraft.Where(a => a.RegistrationNumber == "N8185E")
                .Select(a => a.Id).ToList();
            acMap.Add("PA28-R201", ids);


            List<Member> members = _dbContext.Members.Where(m => m.Status == "Active")
                .Include(m => m.Login)
                .ToList();

            foreach (var member in members)
            {
                List<PilotCheckout> checkouts = new List<PilotCheckout>();

                List<NtfcDataSet.CheckoutRow> srcRows = _dataSet.Checkout
                    .Where(c => c.Member_ID == member.Login.MemberPIN)
                    .OrderByDescending(c => c.Checkout_Date)
                    .ToList();

                if (member.Login.MemberPIN == "735")
                    System.Diagnostics.Debug.Write("735");

                foreach (var row in srcRows)
                {
                    if (!acMap.ContainsKey(row.AC_Type))
                        continue;

                    List<int> acIds = acMap[row.AC_Type];
                    foreach (int id in acIds)
                    {
                        // skip duplicates
                        if (checkouts.Any(c => c.AircraftId == id))
                            continue;
                        

                        PilotCheckout pilotCheckout = new PilotCheckout();
                        pilotCheckout.AircraftId = id;
                        pilotCheckout.CheckoutDate = row.Checkout_Date;

                        pilotCheckout.PilotId = member.Id;

                        Login instructorLogin = _dbContext.Logins.Where(l => l.MemberPIN == row.Instructor_MID).FirstOrDefault();
                        if (instructorLogin == null)
                            continue;

                        int instructorId = _dbContext.Members.First(m => m.LoginId == instructorLogin.Id).Id;
                        pilotCheckout.InstructorId = instructorId;

                        checkouts.Add(pilotCheckout);
                        _dbContext.PilotCheckouts.Add(pilotCheckout);          
                    }
                }
            }


            //IEnumerator<NtfcDataSet.CheckoutRow> rows = _dataSet.Checkout.GetEnumerator();
            //while (rows.MoveNext())
            //{
            //    NtfcDataSet.CheckoutRow row = rows.Current;

            //    if (!acMap.ContainsKey(row.AC_Type))
            //        continue;

            //    List<int> acIds = acMap[row.AC_Type];

            //    foreach (int id in acIds)
            //    {
            //        PilotCheckout pilotCheckout = new PilotCheckout();
            //        pilotCheckout.AircraftId = id;
            //        pilotCheckout.CheckoutDate = row.Checkout_Date;

            //        Login pilotLogin = _dbContext.Logins.Where(l => l.MemberPIN == row.Member_ID).FirstOrDefault();
            //        if (pilotLogin == null)
            //            continue;
            //        int pilotId = _dbContext.Members.First(m => m.LoginId == pilotLogin.Id).Id;
            //        pilotCheckout.PilotId = pilotId;

            //        Login instructorLogin = _dbContext.Logins.Where(l => l.MemberPIN == row.Instructor_MID).FirstOrDefault();
            //        if (instructorLogin == null)
            //            continue;
            //        int instructorId = _dbContext.Members.First(m => m.LoginId == instructorLogin.Id).Id;
            //        pilotCheckout.InstructorId = instructorId;
            //        _dbContext.PilotCheckouts.Add(pilotCheckout);
            //    }
            //}

            _dbContext.SaveChanges();
        }

        public void ImportReviews()
        {
            IEnumerator<NtfcDataSet.BFRRow> rows = _dataSet.BFR.GetEnumerator();
            List<Member> members = _dbContext.Members.Include(m => m.Login).ToList();

            foreach (var member in members)
            {
                NtfcDataSet.BFRRow srcLastReview = _dataSet.BFR
                    .Where(r => r.Member_ID == member.Login.MemberPIN && !r.IsInstructor_MIDNull())
                    .OrderByDescending(r => r.Review_date)
                    .FirstOrDefault();

                if (member.Login.MemberPIN == "537")
                    System.Diagnostics.Debug.WriteLine("Flight review 537");

                if (srcLastReview != null && !srcLastReview.IsReview_dateNull())
                {
                    Member instructor = _dbContext.Members.FirstOrDefault(m => m.Login.MemberPIN == srcLastReview.Instructor_MID);
                    FlightReview flightReview = new FlightReview()
                    {
                        Date = srcLastReview.Review_date,
                        Pilot = member,
                        InstructorName = instructor.FullName
                    };
                    if (!srcLastReview.Istotal_timeNull())
                        flightReview.TotalTime = Single.Parse(srcLastReview.total_time);

                    if (!srcLastReview.Isretract_timeNull())
                        flightReview.RetractTime = Single.Parse(srcLastReview.retract_time);

                    _dbContext.FlightReviews.Add(flightReview);
                }
            }
            _dbContext.SaveChanges();
        }

        public void ImportSquawks()
        {
            TimeSpan squawkBackImport = new TimeSpan(180, 0, 0, 0);
            DateTime cutoffDate = DateTime.Now.Subtract(squawkBackImport);
            IEnumerator<NtfcDataSet.tblThreadsRow> threads = _dataSet.tblThreads.GetEnumerator();

            var articlesByThreadId = _dataSet.tblArticles
                .Where(r => r.Posted > cutoffDate)
                .OrderBy(r => r.Posted)
                .GroupBy(r => r.ThreadID);
            
            foreach (var articleGroup in articlesByThreadId)
            {
                int threadId = articleGroup.Key;
                NtfcDataSet.tblThreadsRow thread = _dataSet.tblThreads.Where(t => t.ThreadId == threadId).FirstOrDefault();
                if (thread == null)
                    continue;
                NtfcDataSet.tblGroupsRow groupRow = _dataSet.tblGroups.FirstOrDefault(g => g.GroupID == thread.GroupID);

                Aircraft aircraft = _dbContext.Aircraft.FirstOrDefault(a => a.RegistrationNumber == groupRow.GroupName.Trim());
                if (aircraft == null)
                    continue;

                Member poster = _dbContext.Members.FirstOrDefault(m => m.FirstName == thread.First_Name.Trim() && m.LastName == thread.Last_name);
                if (poster == null)
                    continue;
                
                Squawk squawk = new Squawk();
                squawk.AircraftId = aircraft.Id;
                squawk.GroundAircraft = false;
                squawk.PostedById = poster.Id;
                squawk.PostedOn = articleGroup.First().Posted;
                squawk.Description = articleGroup.First().Message;
                squawk.Subject = thread.ThreadName;

                if (articleGroup.Count() > 1)
                {
                    squawk.Comments = new List<SquawkComment>();

                    //Member responder = _dbContext.Members.FirstOrDefault(m => m.FirstName == articleGroup.Last().First_Name.Trim() && m.LastName == articleGroup.Last().Last_Name.Trim());
                    //squawk.RespondedBy = responder.Id;
                    //squawk.RespondedOn = articleGroup.Last().Posted;
                    //squawk.Response = articleGroup.Last().Message;

                    for (int i = 1; i < articleGroup.Count(); i++)
                    {
                        NtfcDataSet.tblArticlesRow current = articleGroup.ElementAt(i);
                        SquawkComment comment = new SquawkComment();
                        Member commenter = _dbContext.Members.FirstOrDefault(m => m.FirstName == current.First_Name && m.LastName == current.Last_Name);
                        comment.PostDate = current.Posted;
                        comment.PostedByMemberId = commenter.Id;
                        comment.Text = current.Message.Trim();

                        squawk.Comments.Add(comment);
                    }

                    if (articleGroup.Count() > 2)
                    {

                    }
                }

                _dbContext.Squawks.Add(squawk);
            }
            _dbContext.SaveChanges();
        }

        public void ImportSchedule()
        {
            //List<NtfcDataSet.ScheduleRow> rows = _dataSet.Schedule.Where(r => 
            //    r.Sched_Date > DateTime.Now.Subtract(new TimeSpan(90, 0, 0, 0)) && 
            //    r.AC01_MID != null && r.AC01_MID != String.Empty)
            //    .ToList();

            TimeSpan scheduleBackImport = new TimeSpan(30, 0, 0, 0);
            //-------------- schedule for the N2099V -----------
            string acRegNum = "N2099V";
            var query = from s in _dataSet.Schedule
                        where s.Sched_Date > DateTime.Now.Subtract(scheduleBackImport) &&
                        s.AC01_MID != null &&
                        s.AC01_MID.Trim() != String.Empty
                        orderby s.Sched_Date, s.Sched_Time
                        select s;

            List<NtfcDataSet.ScheduleRow> rows = query.ToList();

            for (int i = 0; i < rows.Count; )
            {
                string username = rows[i].AC01_MID;
                Login login = _dbContext.Logins.Where(l => l.MemberPIN == username)
                    .Include(l => l.ClubMember)
                    .FirstOrDefault();

                if (login == null)
                {
                    i++;
                    continue;
                }//throw new ArgumentException("Unable to find login for member " + username);

                int memberId = login.ClubMember.First().Id;

                Reservation reservation = new Reservation();
                reservation.StartDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time);
                reservation.EndDate = reservation.StartDate.Add(new TimeSpan(0, 30, 0));
                reservation.AircraftId = _dbContext.Aircraft.First(a => a.RegistrationNumber == acRegNum).Id;
                reservation.MemberId = memberId;
                while (++i < rows.Count && username == rows[i].AC01_MID)
                {
                    reservation.EndDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time)
                        .Add(new TimeSpan(0, 30, 0));
                }
                _dbContext.Reservations.Add(reservation);
            }
            _dbContext.SaveChanges();

            //-------- N4952B -----------
            acRegNum = "N4952B";
            query = from s in _dataSet.Schedule
                    where s.Sched_Date > DateTime.Now.Subtract(scheduleBackImport) &&
                        s.AC03_MID != null &&
                        s.AC03_MID.Trim() != String.Empty
                        orderby s.Sched_Date, s.Sched_Time
                        select s;

            rows = query.ToList();

            for (int i = 0; i < rows.Count; )
            {
                string username = rows[i].AC03_MID;
                Login login = _dbContext.Logins.Where(l => l.MemberPIN == username)
                    .Include(l => l.ClubMember)
                    .FirstOrDefault();

                if (login == null)
                {
                    i++;
                    continue;
                }//throw new ArgumentException("Unable to find login for member " + username);

                int memberId = login.ClubMember.First().Id;

                Reservation reservation = new Reservation();
                reservation.StartDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time);
                reservation.EndDate = reservation.StartDate.Add(new TimeSpan(0, 30, 0));
                reservation.AircraftId = _dbContext.Aircraft.First(a => a.RegistrationNumber == acRegNum).Id;
                reservation.MemberId = memberId;
                while (++i < rows.Count && username == rows[i].AC03_MID)
                {
                    reservation.EndDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time)
                        .Add(new TimeSpan(0, 30, 0));
                }
                _dbContext.Reservations.Add(reservation);
            }
            _dbContext.SaveChanges();

            //-------- N8185E -----------
            acRegNum = "N8185E";
            query = from s in _dataSet.Schedule
                    where s.Sched_Date > DateTime.Now.Subtract(scheduleBackImport) &&
                    s.AC04_MID != null &&
                    s.AC04_MID.Trim() != String.Empty
                    orderby s.Sched_Date, s.Sched_Time
                    select s;

            rows = query.ToList();

            for (int i = 0; i < rows.Count; )
            {
                string username = rows[i].AC04_MID;
                Login login = _dbContext.Logins.Where(l => l.MemberPIN == username)
                    .Include(l => l.ClubMember)
                    .FirstOrDefault();

                if (login == null)
                {
                    i++;
                    continue;
                }//throw new ArgumentException("Unable to find login for member " + username);

                int memberId = login.ClubMember.First().Id;

                Reservation reservation = new Reservation();
                reservation.StartDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time);
                reservation.EndDate = reservation.StartDate.Add(new TimeSpan(0, 30, 0));
                reservation.AircraftId = _dbContext.Aircraft.First(a => a.RegistrationNumber == acRegNum).Id;
                reservation.MemberId = memberId;
                while (++i < rows.Count && username == rows[i].AC04_MID)
                {
                    reservation.EndDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time)
                        .Add(new TimeSpan(0, 30, 0));
                }
                _dbContext.Reservations.Add(reservation);
            }
            _dbContext.SaveChanges();

            //-------- N8142H -----------
            acRegNum = "N8142H";
            query = from s in _dataSet.Schedule
                    where s.Sched_Date > DateTime.Now.Subtract(scheduleBackImport) &&
                        s.AC07_MID != null &&
                        s.AC07_MID.Trim() != String.Empty
                        orderby s.Sched_Date, s.Sched_Time
                        select s;

            rows = query.ToList();

            for (int i = 0; i < rows.Count; )
            {
                string username = rows[i].AC07_MID;
                Login login = _dbContext.Logins.Where(l => l.MemberPIN == username)
                    .Include(l => l.ClubMember)
                    .FirstOrDefault();

                if (login == null)
                {
                    i++;
                    continue;
                }//throw new ArgumentException("Unable to find login for member " + username);

                int memberId = login.ClubMember.First().Id;

                Reservation reservation = new Reservation();
                reservation.StartDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time);
                reservation.EndDate = reservation.StartDate.Add(new TimeSpan(0, 30, 0));
                reservation.AircraftId = _dbContext.Aircraft.First(a => a.RegistrationNumber == acRegNum).Id;
                reservation.MemberId = memberId;
                while (++i < rows.Count && username == rows[i].AC07_MID)
                {
                    reservation.EndDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time)
                        .Add(new TimeSpan(0, 30, 0));
                }
                _dbContext.Reservations.Add(reservation);
            }
            _dbContext.SaveChanges();

            //-------- N49649 -----------
            acRegNum = "N49649";
            query = from s in _dataSet.Schedule
                    where s.Sched_Date > DateTime.Now.Subtract(scheduleBackImport) &&
                    s.AC09_MID != null &&
                    s.AC09_MID.Trim() != String.Empty
                    orderby s.Sched_Date, s.Sched_Time
                    select s;

            rows = query.ToList();

            for (int i = 0; i < rows.Count; )
            {
                string username = rows[i].AC09_MID;
                Login login = _dbContext.Logins.Where(l => l.MemberPIN == username)
                    .Include(l => l.ClubMember)
                    .FirstOrDefault();

                if (login == null)
                {
                    i++;
                    continue;
                }//throw new ArgumentException("Unable to find login for member " + username);

                int memberId = login.ClubMember.First().Id;

                Reservation reservation = new Reservation();
                reservation.StartDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time);
                reservation.EndDate = reservation.StartDate.Add(new TimeSpan(0, 30, 0));
                reservation.AircraftId = _dbContext.Aircraft.First(a => a.RegistrationNumber == acRegNum).Id;
                reservation.MemberId = memberId;
                while (++i < rows.Count && username == rows[i].AC09_MID)
                {
                    reservation.EndDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time)
                        .Add(new TimeSpan(0, 30, 0));
                }
                _dbContext.Reservations.Add(reservation);
            }
            _dbContext.SaveChanges();

            //-------- N73192 -----------
            acRegNum = "N73192";
            query = from s in _dataSet.Schedule
                    where s.Sched_Date > DateTime.Now.Subtract(scheduleBackImport) &&
                    s.AC10_MID != null &&
                    s.AC10_MID.Trim() != String.Empty
                    orderby s.Sched_Date, s.Sched_Time
                    select s;

            rows = query.ToList();

            for (int i = 0; i < rows.Count; )
            {
                string username = rows[i].AC10_MID;
                Login login = _dbContext.Logins.Where(l => l.MemberPIN == username)
                    .Include(l => l.ClubMember)
                    .FirstOrDefault();

                if (login == null)
                {
                    i++;
                    continue;
                }//throw new ArgumentException("Unable to find login for member " + username);

                int memberId = login.ClubMember.First().Id;

                Reservation reservation = new Reservation();
                reservation.StartDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time);
                reservation.EndDate = reservation.StartDate.Add(new TimeSpan(0, 30, 0));
                reservation.AircraftId = _dbContext.Aircraft.First(a => a.RegistrationNumber == acRegNum).Id;
                reservation.MemberId = memberId;
                while (++i < rows.Count && username == rows[i].AC10_MID)
                {
                        reservation.EndDate = ParseDateTime(rows[i].Sched_Date, rows[i].Sched_Time)
                            .Add(new TimeSpan(0, 30, 0));
                }
                _dbContext.Reservations.Add(reservation);
            }
            _dbContext.SaveChanges();         
        }

        private DateTime ParseDateTime(DateTime datePart, int hourPart)
        {
            TimeSpan ts = TimeSpan.ParseExact(hourPart.ToString().PadLeft(4, '0'), "hhmm", CultureInfo.InvariantCulture);
            return datePart.Add(ts);
        }
    }
}













