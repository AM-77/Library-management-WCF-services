using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfAuthenticationService
{
    public class Admin
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string second_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public Admin(int id, string first_name, string second_name, string email, string password)
        {
            this.id = id;
            this.first_name = first_name;
            this.second_name = second_name;
            this.email = email;
            this.password = password;
        }
    }
}