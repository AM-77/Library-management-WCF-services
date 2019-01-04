using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WorkService
{
    [DataContract]
    public class Work
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string theme {get; set;}
        [DataMember]
        public string keywords { get; set; }
        [DataMember]
        public string barecode { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public int reserved { get; set; }

        public Work() { }

        public Work(int id, string title, string author, string theme, string keywords, string barecode, string type, int reserved)
        {
            this.id = id;
            this.title = title;
            this.author = author;
            this.theme = theme;
            this.keywords = keywords;
            this.barecode = barecode;
            this.type = type;
            this.reserved = reserved;
        }

    }
}