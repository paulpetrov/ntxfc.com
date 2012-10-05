using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using FlyingClub.Data.Repository;
using FlyingClub.Data.Repository.EntityFramework;
using FlyingClub.Data.Model;
using FlyingClub.Data.Model.Entities;
using FlyingClub.Data.Model.External;

namespace FlyingClub.BusinessLogic
{
    /// <summary>
    /// Contains custom authntication and authorization logic.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        public bool ValidateUser(string username, string password)
        {
            IRepository repository = new GenericRepository();

            FlyingClub.Data.Model.Entities.Login login = repository.FindOne<FlyingClub.Data.Model.Entities.Login>(l => l.Username == username);
            if (login != null && login.Password == password)
                return true;
            else
                return false;
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

        private string GetSubIP(string host)
        {
            List<string> ip = host.Split('.').ToList();
            ip.RemoveAt(ip.Count-1);
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
    }
}
