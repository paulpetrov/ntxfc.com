using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FlyingClub.Data.Model.Entities;
using FlyingClub.BusinessLogic;
using FlyingClub.Common;

namespace FlyingClub.WebApp.Models
{
    public class LoginViewModel
    {
        public LoginViewModel() { }

        public LoginViewModel(Login login)
        {
            Id = login.Id;
            Username = login.Username;
            Password = login.Password;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}