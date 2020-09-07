using System;

namespace HomeAutomation.IdentityService.Models
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Guid HomeAutomationGuid { get; set; }

        public User(string login, string password, Guid homeAutomationGuid)
        {
            Login = login;
            Password = password;
            HomeAutomationGuid = homeAutomationGuid;
        }
    }
}
