using System;

namespace FlyingClub.BusinessLogic
{
    interface ISecurityService
    {
        bool ValidateUser(string username, string password);
        string SaveForumSession(string username, string host, string useragent, string forwardedfor);
    }
}
