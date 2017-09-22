using FileStoringWithDependency.IFileStoreAndLoad;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InvMe.BLL.FileStoreAndLoad
{
    public class FileCheck
    {
        public FileCheck() {
            
        }

        public string Login_filename { get { return "login.txt"; } }

        /// <summary>
        /// Megviszgalja, hogy az adott fajl tartalmaz- e valamit,
        /// ha igen visszakuldi a tartalmat, ha nem akkor nullt ad vissza
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string CheckTheFileContain(string filename)
        {
            try
            {
                string text = 
                    DependencyService.Get<IFileStoreAndLoad>().LoadText(filename);

                return text;
            }
            catch (Exception)
            {
                try
                {
                    DependencyService.Get<IFileStoreAndLoad>().SaveText(filename, "");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

                return null;
            }

        }
    }
}
