using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvMe.DAL.Model
{
    public class Events
    {
        public int id { get; set; }

        public string DESCRIPTION { get; set; }

        public string EVENTNAME { get; set; }

        public string FROM { get; set; }

        public string TO { get; set; }

        public string TOWN { get; set; }

        public string PLACE { get; set; }

        public string MDESCRIPTION { get; set; }

        public int HOWMANY { get; set; }

        public int ONLINE { get; set; }

        public string MEETINGCORD { get; set; }

        public string PLACECORD { get; set; }

    }
}
