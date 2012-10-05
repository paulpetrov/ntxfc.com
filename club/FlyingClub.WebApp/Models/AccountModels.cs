using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using FlyingClub.Data.Model.Entities;

namespace FlyingClub.WebApp.Models
{
    public class AccountViewModel
    {
        public AccountViewModel()
        {
            Init();
        }

        public AccountViewModel(Login entity)
        {
            LoginId = entity.Id;
            UserName = entity.Username;
            Email = entity.Email;
            Email2 = entity.Email2;
            Password = entity.Password;
            MemberPIN = entity.MemberPIN;

            //TODO: implement timezone
            // TimeZoneOffset = entity.

            Init();
        }

        [Display(Name = "Login Id")]
        public int LoginId { get; set; }

        [Display(Name = "Member PIN")]
        public string MemberPIN { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Alternative Email")]
        public string Email2 { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool HasMember { get; set; }

        private void Init()
        {
            //TimeZoneList = new List<SelectListItem>();
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -12:00) Eniwetok, Kwajalein", Value = "-12" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -11:00) Midway Island, Samoa", Value = "-11" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -10:00) Hawaii", Value = "-10" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -9:00) Alaska", Value = "-9" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -8:00) Pacific Time (US & Canada)", Value = "-8" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -7:00) Mountain Time (US & Canada)", Value = "-7" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -6:00) Central Time (US & Canada), Mexico City", Value = "-6" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -5:00) Eastern Time (US & Canada), Bogota, Lima", Value = "-5" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -4:30) Caracas", Value = "-4.5" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -4:00) Atlantic Time (Canada), La Paz, Santiago", Value = "-4" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -3:30) Newfoundland", Value = "-3.5" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -3:00) Brazil, Buenos Aires, Georgetown", Value = "-3" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -2:00) Mid-Atlantic", Value = "-2" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT -1:00 hour) Azores, Cape Verde Islands", Value = "-1" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT) Western Europe Time, London, Lisbon, Casablanca", Value = "0" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +1:00 hour) Brussels, Copenhagen, Madrid, Paris", Value = "1" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +2:00) Kaliningrad, South Africa, Cairo", Value = "2" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +3:00) Baghdad, Riyadh, Moscow, St. Petersburg", Value = "3" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +3:30) Tehran", Value = "3.5" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +4:00) Abu Dhabi, Muscat, Yerevan, Baku, Tbilisi", Value = "4" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +4:30) Kabul", Value = "4.5" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +5:00) Ekaterinburg, Islamabad, Karachi, Tashkent", Value = "5" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +5:30) Mumbai, Kolkata, Chennai, New Delhi", Value = "5.5" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +5:45) Kathmandu", Value = "5.75" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +6:00) Almaty, Dhaka, Colombo", Value = "6" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +6:30) Yangon, Cocos Islands", Value = "6.5" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +7:00) Bangkok, Hanoi, Jakarta", Value = "7" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +8:00) Beijing, Perth, Singapore, Hong Kong", Value = "8" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +9:00) Tokyo, Seoul, Osaka, Sapporo, Yakutsk", Value = "9" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +9:30) Adelaide, Darwin", Value = "9.5" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +10:00) Eastern Australia, Guam, Vladivostok", Value = "10" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +11:00) Magadan, Solomon Islands, New Caledonia", Value = "11" });
            //TimeZoneList.Add(new SelectListItem() { Text = "(GMT +12:00) Auckland, Wellington, Fiji, Kamchatka", Value = "12" });
        }
    }

    public class AccountEditViewModel
    {
        public AccountEditViewModel()
        {
        }

        public AccountEditViewModel(Login entity)
        {
            LoginId = entity.Id;
            UserName = entity.Username;
            Email = entity.Email;
        }

        [Display(Name = "Login Id")]
        public int LoginId { get; set; }

        [Display(Name = "Member PIN")]
        public string MemberPIN { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Alternative Email")]
        public string Email2 { get; set; }

    }

    public class ChangePasswordModel
    {
        public int LoginId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public int LoginId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required(ErrorMessage="Username is missing.")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage="Activation Code is missing.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Alternative Email")]
        public string Email2 { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Time Zone")]
        public int TimeZoneOffset { get; set; }

        public List<SelectListItem> TimeZoneList { get; set; }

        public RegisterModel()
        {
            TimeZoneList = new List<SelectListItem>();
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -12:00) Eniwetok, Kwajalein", Value = "-12" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -11:00) Midway Island, Samoa", Value = "-11" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -10:00) Hawaii", Value = "-10" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -9:00) Alaska", Value = "-9" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -8:00) Pacific Time (US & Canada)", Value = "-8" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -7:00) Mountain Time (US & Canada)", Value = "-7" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -6:00) Central Time (US & Canada), Mexico City", Value = "-6" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -5:00) Eastern Time (US & Canada), Bogota, Lima", Value = "-5" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -4:30) Caracas", Value = "-4.5" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -4:00) Atlantic Time (Canada), La Paz, Santiago", Value = "-4" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -3:30) Newfoundland", Value = "-3.5" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -3:00) Brazil, Buenos Aires, Georgetown", Value = "-3" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -2:00) Mid-Atlantic", Value = "-2" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT -1:00 hour) Azores, Cape Verde Islands", Value = "-1" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT) Western Europe Time, London, Lisbon, Casablanca", Value = "0" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +1:00 hour) Brussels, Copenhagen, Madrid, Paris", Value = "1" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +2:00) Kaliningrad, South Africa, Cairo", Value = "2" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +3:00) Baghdad, Riyadh, Moscow, St. Petersburg", Value = "3" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +3:30) Tehran", Value = "3.5" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +4:00) Abu Dhabi, Muscat, Yerevan, Baku, Tbilisi", Value = "4" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +4:30) Kabul", Value = "4.5" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +5:00) Ekaterinburg, Islamabad, Karachi, Tashkent", Value = "5" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +5:30) Mumbai, Kolkata, Chennai, New Delhi", Value = "5.5" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +5:45) Kathmandu", Value = "5.75" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +6:00) Almaty, Dhaka, Colombo", Value = "6" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +6:30) Yangon, Cocos Islands", Value = "6.5" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +7:00) Bangkok, Hanoi, Jakarta", Value = "7" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +8:00) Beijing, Perth, Singapore, Hong Kong", Value = "8" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +9:00) Tokyo, Seoul, Osaka, Sapporo, Yakutsk", Value = "9" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +9:30) Adelaide, Darwin", Value = "9.5" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +10:00) Eastern Australia, Guam, Vladivostok", Value = "10" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +11:00) Magadan, Solomon Islands, New Caledonia", Value = "11" });
            TimeZoneList.Add(new SelectListItem() { Text = "(GMT +12:00) Auckland, Wellington, Fiji, Kamchatka", Value = "12" });

        }
    }

    public class ActivationModel
    {
        [Required]
        [Display(Name = "Username")]
        public string u { get; set; }

        [Required]
        [Display(Name = "Activation Code")]
        public string i { get; set; }
    }

    public class UpdateEmailsModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Primary Email")]
        public string PrimaryEmail { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Secondary Email")]
        public string SecondaryEmail { get; set; }
    }

    public static class AccountExtensionMethods
    {
        public static AccountViewModel ConvertToViewModel(this Login entity)
        {
            if (entity == null)
                throw new Exception("Account not found");

            return new AccountViewModel()
            {
                LoginId = entity.Id,
                Email = entity.Email,
                Email2 = entity.Email2,
                Password = entity.Password,
                UserName = entity.Username,
                MemberPIN = entity.MemberPIN
            };
        }

        public static Login ConvertToEntity(this AccountViewModel entity)
        {
            if (entity == null)
                throw new Exception("Account not found");

            return new Login()
            {
                Id = entity.LoginId,
                Email = entity.Email,
                Email2 = entity.Email2,
                Password = entity.Password,
                Username = entity.UserName,
                MemberPIN = entity.MemberPIN
            };
        }

        public static AccountEditViewModel ConvertToEditViewModel(this Login entity)
        {
            if (entity == null)
                throw new Exception("Account not found");

            return new AccountEditViewModel()
            {
                LoginId = entity.Id,
                Email = entity.Email,
                Email2 = entity.Email2,
                UserName = entity.Username,
                MemberPIN = entity.MemberPIN
            };
        }

        public static Login ConvertToEntity(this AccountEditViewModel entity, Login login)
        {
            if (entity == null)
                throw new Exception("Account not found");

            login.Email = entity.Email;
            login.Email2 = entity.Email2;
            login.Username = entity.UserName;
            login.MemberPIN = entity.MemberPIN;
            return login;
        }
    }
}
