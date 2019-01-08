using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemService
{
    public class Work
    {
        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string theme { get; set; }
        public string keywords { get; set; }
        public string barecode { get; set; }
        public string type { get; set; }
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