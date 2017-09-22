using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvMe.DAL.Model
{
    public class BindableEvent
    {
        public BindableEvent() { }

        public string EVENTNAME { get; set; }

        public string FROM { get; set; }

        public string TO { get; set; }

        public string TOWN { get; set; }

        public int ID { get; set; }
    }
}
