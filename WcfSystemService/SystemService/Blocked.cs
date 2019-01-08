using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemService
{
    public class Blocked
    {
        public int id { get; set; }
        public string id_client { get; set; }
        public DateTime blocked_day { get; set; }

        public Blocked(int id, string id_client, DateTime blokced_day) {
            this.id = id;
            this.id_client = id_client;
            this.blocked_day = blocked_day;
        }

    }
}