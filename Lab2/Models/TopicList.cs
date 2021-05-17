using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab2.Models
{
    public class TopicList
    {
        public int Topic_Id { get; set; }

        public int Mod_Id { get; set; }
        public string Topic_name { get; set; }
        public string Topic_desc { get; set; }
    }
}