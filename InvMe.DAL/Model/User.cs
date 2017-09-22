using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvMe.DAL.Model
{
    public class User
    {
        public User() { }

        public int id { get; set; }

        public string EMAIL { get; set; }

        public string FIRSTNAME { get; set; }

        public string LASTNAME { get; set; }

        public DateTime BORNDATE { get; set; }

        public string PROFILEPICTURE { get; set; }

        public string FACEBOOK { get; set; }

        public string PASSWORD { get; set; }
    }
}
