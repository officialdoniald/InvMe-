using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvMe.BLL.NuGetPackagesFunctions
{
    public class NuGetPackageFunctions
    {
        public bool CheckConnectivity()
        {
            var isConnected = CrossConnectivity.Current.IsConnected;

            return isConnected;
        }
    }
}
