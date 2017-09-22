using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InvMe.DAL.Model
{
    public class AttendedUser
    {
        public int ID { get; set; }

        public string FIRSTNAME { get; set; }

        public ImageSource PROFILEPICTURE { get; set; }
    }
}
