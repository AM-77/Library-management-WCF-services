using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WorkService
{
    [DataContract]
    public class Teacher
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string second_name { get; set; }
        public string grade { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int didnt_show { get; set; }

        public Teacher(string id, string first_name, string second_name, string grade, string email, string password, int didnt_show)
        {
            this.id = id;
            this.first_name = first_name;
            this.second_name = second_name;
            this.grade = grade;
            this.email = email;
            this.password = password;
            this.didnt_show = didnt_show;
        }
    }
}