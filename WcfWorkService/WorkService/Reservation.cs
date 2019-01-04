using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WorkService
{
    [DataContract]
    public class Reservation
    {
        public int id { get; set; }
        public int id_work { get; set; }
        public string id_client { get; set; }
        public string waitting_list { get; set; }
        public DateTime borrowing_day { get; set; }
        public int confirmed { get; set; }

        public Reservation(int id,int id_work,string id_client,string waitting_list, DateTime borrowing_day, int confirmed)
        {
            this.id = id;
            this.id_work = id_work;
            this.id_client = id_client;
            this.waitting_list = waitting_list;
            this.borrowing_day = borrowing_day;
            this.confirmed = confirmed;
        }

    }
}