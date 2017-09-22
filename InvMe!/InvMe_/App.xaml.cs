using FileStoringWithDependency.IFileStoreAndLoad;
using InvMe.BLL;
using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.FileStoreAndLoad;
using InvMe.BLL.NuGetPackagesFunctions;
using InvMe.DAL.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InvMe_
{
    public partial class App : Application
    {
        #region attr

        User user = new User();

        #endregion

        #region konstruktor

        public App()
        {
            InitializeComponent();

            NuGetPackageFunctions nuGetPackageFunctions = new NuGetPackageFunctions();

            if (nuGetPackageFunctions.CheckConnectivity())
            {
                string text = "";

                try
                {
                    text = DependencyService.Get<IFileStoreAndLoad>().LoadText("login.txt");
                }
                catch (Exception)
                {
                    DependencyService.Get<IFileStoreAndLoad>().SaveText("login.txt", "not");
                }

                user.EMAIL = text;

                if (!string.IsNullOrEmpty(text) && text != "not")
                {
                    MainPage = new MainPage(user);
                }
                else { MainPage = new Login.Login(); }
            }
            else
            {
                MainPage = new NoInternet.NoInternetPage();
            }
        }

        #endregion
        
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
