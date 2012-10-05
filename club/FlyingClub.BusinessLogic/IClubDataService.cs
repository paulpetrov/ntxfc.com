using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FlyingClub.Data.Model.Entities;
using FlyingClub.Data.Model.External;
using FlyingClub.Common;

namespace FlyingClub.BusinessLogic
{
    public interface IClubDataService
    {
        #region Login Operations

        List<Login> GetAllLogins();
        Login CreateLogin(Login login);
        void UpdateLogin(Login login);
        void UpdateLoginInfo(Login login);
        void DeleteLogin(int id);
        Login GetLoginById(int id);
        Login GetLoginByUsername(string username);
        bool ValidateUser(string username, string password);
        bool IsExistingForumSession(string sessionhash);
        bool IsExistingUsername(string username);
        bool IsExistingEmail(string email);
        bool IsValidAuthenticationCode(string username, string activiationId);
        void UpdateForumSession(string username, string sessionhash, int loggedin);
        void DeleteForumSession(string sessionhash);
        void DeleteForumCpSession(string hash);
        string SaveForumSession(string username, string host, string useragent, string forwardedfor);
        User CreateForumUser(User user, string activationid);
        User ActivateUser(string username, string activationid);
        void UpdateLoggedInDate(int loginId, DateTime time);

        #endregion

        #region Member Operations

        List<Member> GetAllClubMembers();
        List<Member> GetAllMembersWithCheckouts();
        List<Member> GetClubMembersByStatus(MemberStatus status);
        List<Member> GetDistinctMembersForRoles(ICollection<string> roles);
        List<Member> GetMembersByRoleAndStatus(UserRoles role, MemberStatus status);
        List<Member> GetMembersWithFlightReview();
        List<Member> GetAuthorizedInstructors(int aircraftId);
        Member GetMember(int id);
        Member GetMemberByLoginId(int loginId);
        Member GetMemberWithPilotData(int id);
        void SaveMember(Member member);
        void UpdateMember(Member member);
        List<Member> GetAllMembersByRole(string role);
        void DeleteMember(int id);
        List<Role> GetAllRoles();
        List<Role> GetRolesByUsername(string username);
        InstructorData GetInstructorInfoByMemberId(int memberId);
        void SaveInstructor(InstructorData instructor);
        PilotCheckout GetCheckout(int checkoutId);
        void AddCheckout(PilotCheckout checkout);
        void RemoveCheckout(int checkoutId);
        bool IsDesignatedForStageChecks(int instructorId);
        void AddPilotStageCheck(StageCheck stageCheck);
        List<Member> GetAllInstructors();
        void RemoveInstructor(int memberId);
        bool IsAircraftOwner(int memberId, int aircraftId);
        int AddFlightReview(FlightReview flightReview);

        #endregion

        #region Aircraft Operations

        List<Aircraft> GetAllAirplanes();
        List<Aircraft> GetAircraftAvailableForCheckIn(int memberId);
        Aircraft GetAircraftById(int id);
        Aircraft GetAircraftByRegNumber(string regNumber);
        Aircraft AddAircraft(Aircraft aircraft);
        void UpdateAircraft(Aircraft aircraft);
        void DeleteAircraft(int id);
        List<AircraftImage> GetAircraftPhotos(int id);
        Squawk AddSquawk(Squawk squawk);
        void UpdateAircraftStatus(int id, string status);
        void AddOwner(int aircraftId, int ownerId);
        void RemoveOwner(int aircraftid, int ownerId);
        void AddAircraftImage(AircraftImage image);
        void RemoveAircraftImage(int imageId);
        List<Member> GetAircraftOwners(int aircraftId);
        List<Aircraft> GetAllAircraftWithOwners();
        List<Aircraft> GetManagedAircraftForMember(int memberId);

        #endregion

        #region Reservation Operations

        List<Reservation> GetReservationListByMember(int id);
        List<Reservation> GetReservationListByAircraft(int aircraftId, DateTime startDate, int days);
        bool IsValidReservationDateRange(int reservationId, int aircraftId, DateTime startDate, DateTime endDate);
        List<Reservation> GetStudentReservationListByInstructor(int id);
        void SaveReservation(Reservation reservation);
        Reservation GetReservation(int id);
        void DeleteReservation(Reservation reservation);
        

        #endregion

        #region Squawk Operations

        List<Squawk> GetAllSquawks();
        List<Squawk> GetActiveSquawks();
        Squawk GetSquawkById(int id);
        List<Squawk> GetSquawksByAircraftId(int id);
        void CreateSquawk(Squawk squawk);
        void UpdateSquawk(Squawk squawk);
        void DeleteSquawk(int id);
        int AddSquawkComment(SquawkComment comment);
        SquawkComment GetSquawkComment(int id);
        void DeleteSquawkComment(int commentId);

        #endregion
    }
}
