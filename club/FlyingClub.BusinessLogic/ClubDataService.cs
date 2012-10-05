using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

using FlyingClub.Common;
using FlyingClub.Data.Repository;
using FlyingClub.Data.Repository.EntityFramework;
using FlyingClub.Data.Model.Entities;
using FlyingClub.Data.Model.External;
using System.Data.Entity.Infrastructure;
using System.Data.Common;

namespace FlyingClub.BusinessLogic
{
    [Export(typeof(IClubDataService))]
    public class ClubDataService : IClubDataService
    {
        private IRepository _repository;
        
        public ClubDataService()
        {
            _repository = new GenericRepository();
        }

        #region Login operations

        public List<Login> GetAllLogins()
        {
            return _repository.GetAll<Login>().ToList();
        }

        public Login CreateLogin(Login login)
        {
            _repository.Add<Login>(login);
            _repository.UnitOfWork.SaveChanges();

            return _repository.FindOne<Login>(o => o.Username == login.Username);
        }

        public void UpdateLogin(Login login)
        {
            _repository.Attach<Login>(login);
            _repository.UnitOfWork.SaveChanges();
        }

        public void UpdateLoggedInDate(int loginId, DateTime time)
        {
            Login login = _repository.First<Login>(l => l.Id == loginId);
            login.LastLogOn = time;
            _repository.UnitOfWork.SaveChanges();
        }

        public void UpdateLoginInfo(Login login)
        {
            Login dbLogin = _repository.FindOne<Login>(l => l.Id == login.Id);
            dbLogin.Username = login.Username;
            dbLogin.Email = login.Email;
            dbLogin.MemberPIN = login.MemberPIN;
            _repository.UnitOfWork.SaveChanges();
        }

        public void DeleteLogin(int id)
        {
            _repository.Delete<Login>(a => a.Id == id);
            _repository.UnitOfWork.SaveChanges();
        }

        public Login GetLoginById(int id)
        {
            return _repository.GetQuery<Login>(o => o.Id == id)
                .Include(l => l.ClubMember)
                .FirstOrDefault();
        }

        public Login GetLoginByUsername(string username)
        {
            return _repository.GetQuery<Login>(o => o.Username.ToLower() == username.ToLower()).Include(l => l.ClubMember).FirstOrDefault();
        }

        public bool ValidateUser(string username, string password)
        {
            FlyingClub.Data.Model.Entities.Login login = _repository.FindOne<FlyingClub.Data.Model.Entities.Login>(l => l.Username == username);
            if (login != null && login.Password == SimpleHash.MD5(password, login.PasswordSalt))
                return true;
            else
                return false;
        }

        public bool IsExistingForumSession(string sessionhash)
        {
            using (ExternalDbContext db = new ExternalDbContext())
            {
                var session = db.Sessions.FirstOrDefault(x => x.sessionhash == sessionhash);
                if (session != null)
                    return true;
            }

            return false;
        }

        public bool IsExistingUsername(string username)
        {
            using (NtxfcDbContext db = new NtxfcDbContext())
            {
                var login = db.Logins.FirstOrDefault(x => x.Username == username);

                if (login != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsExistingEmail(string email)
        {
            using (NtxfcDbContext db = new NtxfcDbContext())
            {
                var login = db.Logins.FirstOrDefault(x => x.Email == email);

                if (login != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsValidAuthenticationCode(string username, string activiationId)
        {
            using (ExternalDbContext db = new ExternalDbContext())
            {
                var activation = db.Database.SqlQuery<UserActivation>("SELECT vb3_useractivation.* FROM vb3_useractivation, vb3_user WHERE vb3_user.username = @username AND vb3_useractivation.activationid = @activiationId", new MySqlParameter("username", username), new MySqlParameter("activiationId", activiationId));

                if (activation.Any())
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateForumSession(string username, string sessionhash, int loggedin)
        {
            int forumUserId = 0;

            using (NtxfcDbContext db = new NtxfcDbContext())
            {
                Login login = db.Logins.FirstOrDefault(x => x.Username == username);
                if (login != null)
                    forumUserId = login.ForumUserId;
            }

            if (forumUserId != 0)
            {
                using (ExternalDbContext db = new ExternalDbContext())
                {
                    var session = db.Sessions.FirstOrDefault(x => x.sessionhash == sessionhash);
                    if (session != null)
                    {
                        session.loggedin = loggedin;
                        session.userid = forumUserId;
                        db.Sessions.Attach(session);
                        db.Entry(session).Property(u => u.loggedin).IsModified = true;
                        db.Entry(session).Property(u => u.userid).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }
        }

        public void DeleteForumSession(string sessionhash)
        {
            using (ExternalDbContext db = new ExternalDbContext())
            {
                var session = db.Sessions.FirstOrDefault(x => x.sessionhash == sessionhash);
                if (session != null)
                {
                    db.Sessions.Remove(session);
                    db.SaveChanges();
                }
            }
        }

        public void DeleteForumCpSession(string hash)
        {
            using (ExternalDbContext db = new ExternalDbContext())
            {
                var session = db.CpSessions.FirstOrDefault(x => x.hash == hash);
                if (session != null)
                {
                    db.CpSessions.Remove(session);
                    db.SaveChanges();
                }
            }
        }

        public string SaveForumSession(string username, string host, string useragent, string forwardedfor)
        {
            int forumUserId = 0;

            using (NtxfcDbContext db = new NtxfcDbContext())
            {
                Login login = db.Logins.FirstOrDefault(x => x.Username == username);
                if (login != null)
                    forumUserId = login.ForumUserId;
            }

            if (forumUserId != 0)
            {
                Session session = new Session();
                session.sessionhash = Guid.NewGuid().ToString("N");
                session.userid = forumUserId;
                session.host = host;
                session.idhash = CalculateMD5Hash(String.Concat(useragent, GetSubIP(GetRealIP(host, forwardedfor))));
                TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                session.lastactivity = Convert.ToInt32(ts.TotalSeconds);
                session.location = "/login.php";
                session.useragent = useragent;
                session.loggedin = 1;
                session.apiaccesstoken = string.Empty;

                using (ExternalDbContext db = new ExternalDbContext())
                {
                    db.Sessions.Add(session);
                    db.SaveChanges();
                }

                return session.sessionhash;
            }

            return string.Empty;
        }

        public User CreateForumUser(User user, string activationid)
        {
            using (ExternalDbContext db = new ExternalDbContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }

            using (ExternalDbContext db = new ExternalDbContext())
            {
                db.Database.ExecuteSqlCommand("INSERT INTO `vb3_userfield` (userid) VALUES(@userid)", new MySqlParameter("userid", user.userid));
                db.SaveChanges();
            }

            using (ExternalDbContext db = new ExternalDbContext())
            {
                db.Database.ExecuteSqlCommand("INSERT INTO `vb3_useractivation` (userid, activationid) VALUES(@userid,@activationid)", new MySqlParameter("userid", user.userid), new MySqlParameter("activationid", activationid));
                db.SaveChanges();
            }

            return user;
        }

        public User ActivateUser(string username, string activationid)
        {
            User user;

            using (ExternalDbContext db = new ExternalDbContext())
            {
                user = db.Users.FirstOrDefault(x => x.username == username);
                user.usergroupid = 2;
                db.Users.Attach(user);
                db.Entry(user).Property(u => u.usergroupid).IsModified = true;
                db.SaveChanges();
            }

            if (user != null)
            {
                using (ExternalDbContext db = new ExternalDbContext())
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM `vb3_useractivation` WHERE userid = @userid AND activationid = @activationid", new MySqlParameter("userid", user.userid), new MySqlParameter("activationid", activationid));
                    db.SaveChanges();
                }
            }

            return user;
        }

        private string GetSubIP(string host)
        {
            List<string> ip = host.Split('.').ToList();
            ip.RemoveAt(ip.Count - 1);
            return string.Join(".", ip.ToArray());
        }

        private string GetRealIP(string host, string forwardedfor)
        {
            if (string.IsNullOrEmpty(forwardedfor))
                return host;

            return forwardedfor;
        }

        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private string CalculateMD5Hash(string input, string salt)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        #endregion

        #region Member Operations

        public List<Member> GetAllClubMembers()
        {
            return _repository.GetQuery<Member>().Include(m => m.Login).ToList();
        }

        public List<Member> GetAllMembersWithCheckouts()
        {
            string status = MemberStatus.Active.ToString();
            return _repository.GetQuery<Member>()
                        .Include(m => m.Checkouts)
                            .Where(m => m.Status == status).ToList();
        }

        public List<Member> GetMembersWithFlightReview()
        {
            string status = MemberStatus.Active.ToString();

            return _repository.GetQuery<Member>()
                        .Include(m => m.FlightReviews)
                            .Where(m => m.Status == status)
                            .ToList();
        }

        public List<Member> GetClubMembersByStatus(MemberStatus status)
        {
            string strStatus = status.ToString();
            return _repository.GetQuery<Member>()
                .Where(m => m.Status == strStatus)
                .Include(m => m.Login)
                .ToList();
        }

        public Member GetMember(int id)
        {
            return _repository.GetQuery<Member>(m => m.Id == id)
                .Include(o => o.Roles)
                .Include(o => o.Login)
                .FirstOrDefault();
        }

        public Member GetMemberWithPilotData(int id)
        {
            return _repository.GetQuery<Member>(m => m.Id == id)
                .Include(m => m.FlightReviews)
                .Include(m => m.Checkouts)
                .Include(m => m.StageChecks)
                .FirstOrDefault();
        }

        public Member GetMemberByLoginId(int loginId)
        {
            return _repository.GetQuery<Member>(m => m.LoginId == loginId)
                .Include(o => o.Roles)
                .FirstOrDefault();
        }

        public void SaveMember(Member member)
        {
            member.LastUpdated = DateTime.Now;
            _repository.Add<Member>(member);
            _repository.UnitOfWork.SaveChanges();
        }

        public void UpdateMember(Member member)
        {
            member.LastUpdated = DateTime.Now;
            _repository.Attach(member);
            _repository.UnitOfWork.SaveChanges();
        }

        public List<Member> GetAllMembersByRole(string role)
        {
            return _repository.Find<Member>(x => x.Roles.Any(r => r.Name.Contains(role))).ToList();
        }

        public List<Member> GetMembersByRoleAndStatus(UserRoles role, MemberStatus status)
        {
            string strStatus = status.ToString();
            string strRole = role.ToString();

            return _repository.GetQuery<Member>(m => m.Status == strStatus && m.Roles.Any(r => r.Name == strRole))
                .Include(m => m.Login)
                .ToList();
        }

        public List<Member> GetDistinctMembersForRoles(ICollection<string> roles)
        {
            string strStatus = MemberStatus.Active.ToString();

            return _repository.GetQuery<Member>(m => m.Status == strStatus && m.Roles.Any(r => roles.Contains(r.Name)))
                .Distinct()
                .Include(m => m.Login)
                .ToList();
        }

        public void DeleteMember(int id)
        {
            _repository.Delete<Member>(m => m.Id == id);
            _repository.UnitOfWork.SaveChanges();
        }

        public List<Role> GetAllRoles()
        {
            return _repository.GetAll<Role>().ToList();
        }

        public List<Role> GetRolesByUsername(string username)
        {
            Login login = _repository.FindOne<Login>(x => x.Username == username);
            if (login != null)
                return _repository.GetQuery<Role>(x => x.Members.Any(y => y.LoginId == login.Id)).ToList();
            else
                return new List<Role>();
        }

        public List<Member> GetAuthorizedInstructors(int aircraftId)
        {
            List<int> authorizedIds = _repository.GetQuery<InstructorAuthorization>(ia => ia.AircraftId == aircraftId)
                .Select<InstructorAuthorization, int>(ia => ia.InstructorId)
                .Distinct()
                .ToList();

            return _repository.GetQuery<Member>(m => authorizedIds.Contains(m.Id)).ToList();
        }

        public InstructorData GetInstructorInfoByMemberId(int memberId)
        {
            InstructorData info = _repository.GetQuery<InstructorData>(o => o.MemberId == memberId)
                .Include(o => o.Member)
                .Include(o => o.AuthorizedAircraft)
                .Include("AuthorizedAircraft.Aircraft")
                .FirstOrDefault();

            return info;
        }

        public void SaveInstructor(InstructorData instructor)
        {
            if(instructor.Id > 0)
                _repository.Attach(instructor);
            else
                _repository.Add(instructor);
            _repository.UnitOfWork.SaveChanges();
        }

        public PilotCheckout GetCheckout(int checkoutId)
        {
            return _repository.GetQuery<PilotCheckout>(o => o.Id == checkoutId)
                .Include(o => o.Pilot)
                .Include(o => o.Instructor)
                .Include(o => o.Aircraft)
                .FirstOrDefault();
        }

        public void AddCheckout(PilotCheckout checkout)
        {
            _repository.Add<PilotCheckout>(checkout);
            _repository.UnitOfWork.SaveChanges();
        }

        public int AddFlightReview(FlightReview flightReview)
        {
            _repository.Add<FlightReview>(flightReview);
            _repository.UnitOfWork.SaveChanges();

            return flightReview.Id;
        }

        public void RemoveCheckout(int checkoutId)
        {
            _repository.Delete<PilotCheckout>(o => o.Id == checkoutId);
            _repository.UnitOfWork.SaveChanges();
        }

        public bool IsDesignatedForStageChecks(int instructorId)
        {
            return _repository.FindOne<InstructorData>(i => i.MemberId == instructorId).DesignatedForStageChecks;
        }

        public void AddPilotStageCheck(StageCheck stageCheck)
        {
            _repository.Add<StageCheck>(stageCheck);
            _repository.UnitOfWork.SaveChanges();
        }

        public void RemoveInstructor(int memberId)
        {
            string roleInstructor = UserRoles.Instructor.ToString();
            InstructorData instructor = _repository.GetQuery<InstructorData>(d => d.MemberId == memberId)
                .Include(d => d.Member)
                .FirstOrDefault();

            if(instructor == null)
                throw new ApplicationException("No instructor record found associated with MemberID=" + memberId.ToString());

            Member member = _repository.GetQuery<Member>(m => m.Id == memberId)
                .Include(m => m.Roles)
                .FirstOrDefault();

            // delete instructor data
            _repository.Delete<InstructorData>(i => i.Id == instructor.Id);

            // remove instructor role
            Role role = member.Roles.FirstOrDefault(r => r.Name == roleInstructor);
            if (role != null)
                member.Roles.Remove(role);

            _repository.UnitOfWork.SaveChanges();
        }

        public bool IsAircraftOwner(int memberId, int aircraftId)
        {
            return _repository.Find<Aircraft>(a => a.Id == aircraftId && a.Owners.Any(o => o.Id == memberId)).Count() > 0;          
        }

        #endregion

        #region Aircraft Operations

        public List<Aircraft> GetAllAirplanes()
        {
            return _repository.GetQuery<Aircraft>().Include(a => a.Images).ToList();
        }

        public List<Aircraft> GetAircraftAvailableForCheckIn(int memberId)
        {
            List<int> checkedOutIn = _repository.GetQuery<PilotCheckout>(c => c.PilotId == memberId)
                .Select(c => c.AircraftId).ToList();

            return _repository.Find<Aircraft>(a => !checkedOutIn.Contains(a.Id)).ToList();
        }

        public Aircraft GetAircraftById(int id)
        {
            return _repository.GetQuery<Aircraft>(x => x.Id == id)
                .Include(a => a.Squawks)
                .Include(a => a.Owners)
                .Include(a => a.Images)
                .FirstOrDefault();
        }

        public Aircraft GetAircraftByRegNumber(string regNumber)
        {
            return _repository.GetQuery<Aircraft>(x => x.RegistrationNumber == regNumber).FirstOrDefault();
        }

        public List<Aircraft> GetManagedAircraftForMember(int memberId)
        {
            Member member = _repository.GetQuery<Member>(m => m.Id == memberId)
                .Include(m => m.Roles)
                .FirstOrDefault();
            string adminRole = UserRoles.Admin.ToString();
            string apRole = UserRoles.AircraftMaintenance.ToString();
            string ownerRole = UserRoles.AircraftOwner.ToString();

            if (member.Roles.Any(r => r.Name == adminRole || r.Name == apRole))
            {
                return _repository.GetAll<Aircraft>().ToList();
            }

            if (member.Roles.Any(r => r.Name == ownerRole))
            {
                return _repository.Find<Aircraft>(a => a.Owners.Count(o => o.Id == memberId) == 1).ToList();
            }

            return new List<Aircraft>();
        }

        public Aircraft AddAircraft(Aircraft aircraft)
        {
            _repository.Add<Aircraft>(aircraft);
            _repository.UnitOfWork.SaveChanges();
            return _repository.FindOne<Aircraft>(a => a.RegistrationNumber == aircraft.RegistrationNumber);
        }

        public void UpdateAircraft(Aircraft aircraft)
        {
            _repository.Attach<Aircraft>(aircraft);
            _repository.UnitOfWork.SaveChanges();
        }

        public void DeleteAircraft(int id)
        {
            _repository.Delete<Aircraft>(a => a.Id == id);
            _repository.UnitOfWork.SaveChanges();
        }

        public List<AircraftImage> GetAircraftPhotos(int id)
        {
            return new List<AircraftImage>(_repository.Find<AircraftImage>(x => x.AircraftId == id));
        }

        public Squawk AddSquawk(Squawk squawk)
        {
            _repository.Add<Squawk>(squawk);
            _repository.UnitOfWork.SaveChanges();
            return squawk;
        }

        public void UpdateAircraftStatus(int id, string status)
        {
            Aircraft aircraft = _repository.FindOne<Aircraft>(a => a.Id == id);
            if (aircraft != null)
            {
                aircraft.Status = status;
                _repository.UnitOfWork.SaveChanges();
            }
        }

        public List<Member> GetAircraftOwners(int aircraftId)
        {
            return _repository.GetQuery<Aircraft>(a => a.Id == aircraftId)
                .FirstOrDefault().Owners.ToList();
        }

        public void AddOwner(int aircraftId, int ownerId)
        {
            Aircraft aircraft = _repository.GetQuery<Aircraft>(a => a.Id == aircraftId).FirstOrDefault();
            Member owner = _repository.FindOne<Member>(o => o.Id == ownerId);
            aircraft.Owners.Add(owner);
            _repository.UnitOfWork.SaveChanges();
        }

        public void RemoveOwner(int aircraftId, int ownerId)
        {
            Aircraft aircraft = _repository.GetQuery<Aircraft>(a => a.Id == aircraftId).Include(a => a.Owners).FirstOrDefault();
            Member owner = aircraft.Owners.FirstOrDefault(o => o.Id == ownerId);
            aircraft.Owners.Remove(owner);
            _repository.UnitOfWork.SaveChanges();
        }

        public void AddAircraftImage(AircraftImage image)
        {
            _repository.Add<AircraftImage>(image);
            _repository.UnitOfWork.SaveChanges();
        }

        public void RemoveAircraftImage(int imageId)
        {
            _repository.Delete<AircraftImage>(i => i.Id == imageId);
            _repository.UnitOfWork.SaveChanges();
        }

        public List<Aircraft> GetAllAircraftWithOwners()
        {
            string statusInactive = AircraftStatus.Offline.ToString();
            return _repository.GetQuery<Aircraft>(a => a.Status != statusInactive)
                .Include(a => a.Owners)
                .ToList();
        }

        public List<Member> GetAllInstructors()
        {
            string roleInstructor = UserRoles.Instructor.ToString();
            return _repository.GetQuery<Member>(m => m.Roles.Any(r => r.Name == roleInstructor)).ToList();
        }

        #endregion

        #region Reservation Operations

        public List<Reservation> GetReservationListByMember(int id)
        {
            return _repository.GetQuery<Reservation>(x => x.MemberId == id).Include("Aircraft").Include("Member").ToList();
        }

        public List<Reservation> GetReservationListByAircraft(int aircraftId, DateTime startDate, int days)
        {
            DateTime rangeStartDate = startDate.Add(new TimeSpan(06, 00, 00)); // Start Date at 6am
            DateTime rangeEndDate = startDate.Add(new TimeSpan(days, 22, 00, 00)); // End Date at 10pm
            return _repository.GetQuery<Reservation>(x => x.AircraftId == aircraftId && (x.StartDate.CompareTo(rangeEndDate) <= 0 && x.EndDate.CompareTo(rangeStartDate) >= 0)).Include("Aircraft").Include("Member").ToList();
        }

        public bool IsValidReservationDateRange(int reservationId, int aircraftId, DateTime startDate, DateTime endDate)
        {
            var reservation = _repository.GetQuery<Reservation>(x => x.Id != reservationId && x.AircraftId == aircraftId && (x.StartDate.CompareTo(endDate) < 0 && x.EndDate.CompareTo(startDate) > 0)).FirstOrDefault();
            return (reservation == null) ? true : false;
        }

        public List<Reservation> GetStudentReservationListByInstructor(int id)
        {
            return _repository.GetQuery<Reservation>(x => x.InstructorId == id).Include("Aircraft").Include("Member").ToList();
        }

        public void SaveReservation(Reservation reservation)
        {
            if (reservation.Id == 0)
                _repository.Add<Reservation>(reservation);
            else
                _repository.Attach<Reservation>(reservation);

            _repository.UnitOfWork.SaveChanges();
        }

        public Reservation GetReservation(int id)
        {
            return _repository.GetQuery<Reservation>(x => x.Id == id).Include("Member").Include("Aircraft").Include("Instructor").FirstOrDefault();
        }

        public void DeleteReservation(Reservation reservation)
        {
            _repository.Delete<Reservation>(reservation);
            _repository.UnitOfWork.SaveChanges();
        }

        #endregion

        #region Squawk Operations

        public List<Squawk> GetAllSquawks()
        {
            return _repository.GetQuery<Squawk>()
                .Include(s => s.Aircraft)
                .Include(s => s.PostedBy)
                .OrderByDescending(s => s.PostedOn)
                .ToList();
        }

        public List<Squawk> GetActiveSquawks()
        {
            string statusOpen = SquawkStatus.Open.ToString();
            DateTime postThreshold = DateTime.Now.Subtract(new TimeSpan(60, 0, 0, 0));
            return _repository.GetQuery<Squawk>(s => s.Status == statusOpen || s.PostedOn > postThreshold)
                        .Include(s => s.Aircraft)
                        .Include(s => s.PostedBy)
                        .OrderByDescending(s => s.PostedOn)
                        .ToList();
        }

        public Squawk GetSquawkById(int id)
        {
            return _repository.GetQuery<Squawk>(x => x.Id == id)
            .Include(s => s.Aircraft)
            .Include(s => s.PostedBy)
            .FirstOrDefault();
        }

        public List<Squawk> GetSquawksByAircraftId(int id)
        {
            return _repository.GetQuery<Squawk>().Where(x => x.AircraftId == id)
            .Include(s => s.Aircraft)
            .Include(s => s.PostedBy)
            .ToList();
        }

        public void CreateSquawk(Squawk squawk)
        {
            _repository.Add<Squawk>(squawk);
            _repository.UnitOfWork.SaveChanges();
        }

        public void UpdateSquawk(Squawk squawk)
        {
            _repository.Attach(squawk);
            _repository.UnitOfWork.SaveChanges();
        }

        public void DeleteSquawk(int id)
        {
            _repository.Delete<Squawk>(s => s.Id == id);
            _repository.UnitOfWork.SaveChanges();
        }

        public int AddSquawkComment(SquawkComment comment)
        {
            _repository.Add<SquawkComment>(comment);
            _repository.UnitOfWork.SaveChanges();
            return comment.Id;
        }

        public SquawkComment GetSquawkComment(int id)
        {
            return _repository.FindOne<SquawkComment>(c => c.Id == id);
        }

        public void DeleteSquawkComment(int commentId)
        {
            _repository.Delete<SquawkComment>(c => c.Id == commentId);
            _repository.UnitOfWork.SaveChanges();
        }

        #endregion
    }
}
