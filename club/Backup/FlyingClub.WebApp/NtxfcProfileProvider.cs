using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.Configuration;
using FlyingClub.Data.Model.Entities;
using FlyingClub.BusinessLogic;

namespace FlyingClub.WebApp
{
    public class ProfileCommon : System.Web.Profile.ProfileBase
    {
        

        public static ProfileCommon GetProfile()
        {
            return Create(HttpContext.Current.Request.IsAuthenticated ?
                   HttpContext.Current.User.Identity.Name : HttpContext.Current.Request.AnonymousID,
                   HttpContext.Current.Request.IsAuthenticated) as ProfileCommon;
        }

        public static ProfileCommon GetUserProfile(string username)
        {
            return Create(username) as ProfileCommon;
        }

        public static ProfileCommon GetUserProfile()
        {
            return Create(Membership.GetUser().UserName) as ProfileCommon;
        }

        // Add the Profile properties starting here
        // using the same property name and SQL type
        // as exists in your new profile table

        [CustomProviderData("LoginId")]
        public virtual int LoginId
        {
            get
            {
                return Convert.ToInt32(this.GetPropertyValue("LoginId"));
            }
            set
            {
                this.SetPropertyValue("LoginId", value);
            }
        }

        [CustomProviderData("MemberId")]
        public virtual int MemberId
        {
            get
            {
                return Convert.ToInt32(this.GetPropertyValue("MemberId"));
            }
            set
            {
                this.SetPropertyValue("MemberId", value);
            }
        }

        [CustomProviderData("FirstName")]
        public virtual string FirstName
        {
            get
            {
                return Convert.ToString(this.GetPropertyValue("FirstName"));
            }
            set
            {
                this.SetPropertyValue("FirstName", value);
            }
        }

        [CustomProviderData("LastName")]
        public virtual string LastName
        {
            get
            {
                return Convert.ToString(this.GetPropertyValue("LastName"));
            }
            set
            {
                this.SetPropertyValue("LastName", value);
            }
        }

        [CustomProviderData("PrimaryEmail")]
        public virtual string PrimaryEmail
        {
            get
            {
                return Convert.ToString(this.GetPropertyValue("PrimaryEmail"));
            }
            set
            {
                this.SetPropertyValue("PrimaryEmail", value);
            }
        }
    }

    public class NtxfcProfileProvider : ProfileProvider
    {
        private ClubDataService _dataService;

        public NtxfcProfileProvider()
        {
            _dataService = new ClubDataService();
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
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

        public override System.Configuration.SettingsPropertyValueCollection GetPropertyValues(System.Configuration.SettingsContext context, System.Configuration.SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection svc = new SettingsPropertyValueCollection();
            string username = Convert.ToString(context["UserName"]);
            Member member = null;

            Role guest = new Role()
            {
                Name = "Guest",
                Description = "Guest user",
            };

            ICollection<Role> guestRoles = new List<Role>();
            guestRoles.Add(guest);

            if (Convert.ToBoolean(context["IsAuthenticated"]) && !String.IsNullOrEmpty(username))
            {
                Login login = _dataService.GetLoginByUsername(username);

                if (login != null)
                    member = _dataService.GetMemberByLoginId(login.Id);

                foreach (SettingsProperty item in collection)
                {
                    SettingsPropertyValue value = new SettingsPropertyValue(item);

                    switch (item.Name)
                    {
                        case "LoginId":
                            value.PropertyValue = login != null ? login.Id : 0;
                            break;
                        case "MemberId":
                            value.PropertyValue = member != null ? member.Id : 0;
                            break;
                        case "FirstName":
                            value.PropertyValue = member != null ? member.FirstName : string.Empty;
                            break;
                        case "LastName":
                            value.PropertyValue = member != null ? member.LastName : string.Empty;
                            break;
                        case "PrimaryEmail":
                            value.PropertyValue = member != null ? member.Login.Email : string.Empty;
                            break;
                        case "Roles":
                            value.PropertyValue = member != null ? member.Roles : guestRoles;
                            break;
                    }

                    svc.Add(value);
                }
            }
            
            return svc;
        }

        public override void SetPropertyValues(System.Configuration.SettingsContext context, System.Configuration.SettingsPropertyValueCollection collection)
        {
            //throw new NotImplementedException();
        }
    }
}