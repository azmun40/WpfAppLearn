using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppLearn.Models
{
    [Serializable]
    public class AuthUsers
    {
        public string Login { get; set; }
        public string Email { get; set; }

        public AuthUsers()
        {
        }

        public AuthUsers(string login, string email)
        {
            Login = login;
            Email = email;
        }
    }
}
