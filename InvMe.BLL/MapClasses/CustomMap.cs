using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace InvMe.BLL.MapClasses
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }

        public string kind { get; set; }

        public bool isJustShow { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }
    }
    
}
