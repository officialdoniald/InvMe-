using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvMe.DAL.Model
{
    public class Attended
    {
        public int id { get; set; }

        public int user_id { get; set; }

        public int event_id { get; set; }
    }
}
