using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Net.Mail;

using FlyingClub.BusinessLogic;
using FlyingClub.Common;
using FlyingClub.Data.Model.Entities;
using FlyingClub.Data.Model.External;

namespace FlyingClub.WebApp
{
    public enum MembershipActiviationStatus
    {
        Success = 0,
        InvalidActivationCode = 1,
        InvalidUsername = 2,
        ProviderError = 3
    }

    public class NtxfcMembershipProvider : MembershipProvider
    {
        private ClubDataService _dataService;

        public NtxfcMembershipProvider()
        {
            _dataService = new ClubDataService();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            Login login = _dataService.GetLoginByUsername(username);

            if (login != null)
            {
                if (login.Password == SimpleHash.MD5(oldPassword, login.PasswordSalt))
                {
                    login.Password = SimpleHash.MD5(newPassword, login.PasswordSalt);
                    _dataService.UpdateLogin(login);

                    return true;
                }
            }

            return false;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (_dataService.IsExistingUsername(username))
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            if (_dataService.IsExistingEmail(email))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            string salt = SimpleHash.GetSalt(32);
            string activationCode = ShortGuid.NewGuid();

            User user = new User()
            {
                email = email,
                usergroupid = 3,
                username = username,
                password = SimpleHash.MD5(password, salt),
                passworddate = DateTime.Now.Date,
                usertitle = "Guest",
                joindate = Convert.ToInt32(ts.TotalSeconds),
                timezoneoffset = -6,
                options = 45108439,
                salt = salt,
                showbirthday = 0,
                showvbcode = 0, 
                membergroupids = string.Empty
            };

            user = _dataService.CreateForumUser(user, activationCode);

            if (user != null)
            {
                Login login = new Login();
                login.Username = username;
                login.Password = SimpleHash.MD5(password, salt);
                login.Email = email;
                login.ForumUserId = user.userid;
                login.PasswordSalt = salt;

                login = _dataService.CreateLogin(login);

                if (login != null)
                {
                    NtxfcMembershipProvider provider = Membership.Provider as NtxfcMembershipProvider;
                    provider.SendConfirmationEmail(username, email, String.Format("{0}/Account/Activation/?u={1}&i={2}", ConfigurationManager.AppSettings["MemberUrl"], HttpContext.Current.Server.UrlEncode(username), activationCode));
                    status = MembershipCreateStatus.Success;
                    return new MembershipUser(this.Name, username, login.Id, login.Email, string.Empty, string.Empty, true, false, new DateTime(), new DateTime(), new DateTime(), new DateTime(), new DateTime());
                }
            }

            status = MembershipCreateStatus.ProviderError;
            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(username) || username.Length > 100) return null;
            username = username.ToLower();
            try
            {
                Login login = _dataService.GetLoginByUsername(username);

                if (login != null)
                    return new MembershipUser(this.Name, username, login.Id, login.Email, string.Empty, string.Empty, true, false, new DateTime(), new DateTime(), new DateTime(), new DateTime(), new DateTime());
                
                return null;
            }
            catch { throw; } // Security context hack for HostingEnvironment.Impersonate
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            ClubDataService service = new ClubDataService();
            return service.ValidateUser(username, password);
        }

        public void SendConfirmationEmail(string username, string email, string activationUrl)
        {
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(ConfigurationManager.AppSettings["AdminEmail"]);
                message.To.Add(email);
                message.Subject = "North Texas Flying Club Activiation";
                message.Body = String.Format("<p>Dear {0},</p><p>Thank you for registering. Before we can activate your account one last step must be taken to complete your registration.</p><p>Please note - you must complete this last step to become a registered member. You will only need to visit this URL once to activate your account.</p><p>To complete your registration, please visit this URL: <a href=\"{1}\">{1}</a>.</p><p>If the above URL does not work, please contact the administrator at <a href=\"{2}\">{2}</a></p><p>Thank you,<br/>North Texas Flying Club</p>", username, activationUrl, ConfigurationManager.AppSettings["AdminEmail"]);
                message.Priority = MailPriority.Normal;
                message.IsBodyHtml = true;

                using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient())
                {
                    client.Send(message);
                }
            }
        }

        public User ActivateUser(string username, string activationCode, out MembershipActiviationStatus status)
        {
            if (!_dataService.IsValidAuthenticationCode(username, activationCode))
            {
                status = MembershipActiviationStatus.InvalidActivationCode;
                return null;
            }

            var user = _dataService.ActivateUser(username, activationCode);

            if (user != null)
            {
                status = MembershipActiviationStatus.Success;
                return user;
            }

            status = MembershipActiviationStatus.ProviderError;
            return null;
        }
    }
}