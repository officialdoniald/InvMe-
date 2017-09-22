using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvMe.BLL.FileToByte;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(InvMe_.Droid.FileToByte.FileToByte))]
namespace InvMe_.Droid.FileToByte
{
    public class FileToByte : IFileToByte
    {
        public byte[] ReadAllByteS(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}