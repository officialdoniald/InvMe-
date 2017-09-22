using InvMe.BLL.FileToByte;
using InvMe_.iOS.FileToByte;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileToByte))]
namespace InvMe_.iOS.FileToByte
{
    public class FileToByte : IFileToByte
    {
        public byte[] ReadAllByteS(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}