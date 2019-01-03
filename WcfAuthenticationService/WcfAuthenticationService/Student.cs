using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfAuthenticationService
{
    [DataContract]
    public class Student
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string first_name { get; set; }
        [DataMember]
        public string second_name { get; set; }
        [DataMember]
        public string level { get; set; }
        [DataMember]
        public string specaility { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public int didnt_show { get; set; }

        public Student(string id, string first_name, string second_name, string level, string specaility, string email, string password, int didnt_show)
        {
            this.id = id;
            this.first_name = first_name;
            this.second_name = second_name;
            this.level = level;
            this.specaility = specaility;
            this.email = email;
            this.password = password;
            this.didnt_show = didnt_show;
        }
    }
}