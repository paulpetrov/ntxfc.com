using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using FlyingClub.BusinessLogic;
using FlyingClub.Data.Model.Entities;

namespace FlyingClub.WebApp
{
    public class NtxfcRoleProvider : RoleProvider
    {
        private ClubDataService _dataService;

        public NtxfcRoleProvider()
        {
             _dataService = new ClubDataService();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
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

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            List<Role> roles = _dataService.GetRolesByUsername(username);

            if (roles.Count == 0)
                roles.Add(new Role() { Name = "Guest" });

            return roles.Select(x => x.Name).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}