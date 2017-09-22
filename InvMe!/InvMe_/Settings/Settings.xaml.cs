using FileStoringWithDependency.IFileStoreAndLoad;
using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.Labels.English;
using InvMe.BLL.Labels.ILabels;
using InvMe.DAL.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvMe_.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        #region attr

        User user;

        ICimkek cimkek = new Cimkek();

        #endregion

        #region konstruktor

        public Settings(User user)
        {
            InitializeComponent();

            this.user = user;

            homeImage.Source = "home.png";
            addImage.Source = "add.png";
            joinedImage.Source = "calendar.png";
            hashtagsImage.Source = "hashtags.png";
            settingsImage.Source = "tsettings.png";
            
            #region cimkek

            updateButton.Text = cimkek.GetUpdateProfile();

            //myfriendsButton.Text = cimkek.GetMyFriends();

            //impressButton.Text = cimkek.GetImpress();

            deleteButton.Text = cimkek.GetDeleteAccount();

            logoutButton.Text = cimkek.GetLogout();

            #endregion

            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetLostConnection(), cimkek.GetOK());
                    //await Navigation.PushModalAsync(new NoInternet.NoInternetPage(this));
                }
            };
        }

        #endregion

        #region click

        private async void updateButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Profile.Profile(user));
        }

        private async void impressButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Impress.Impress(user));
        }

        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            bool success = false;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    success = DependencyService.Get<IiOSDatabaseAccess>().DeleteUser(user);
                    break;
                case Device.Android:
                    success = DependencyService.Get<IAndroidDatabaseAccess>().DeleteUser(user);
                    break;
                case Device.WinPhone:
                    success = await DependencyService.Get<IDatabaseAccess>().DeleteUser(user);
                    break;
                case Device.Windows:
                    success = await DependencyService.Get<IDatabaseAccess>().DeleteUser(user);
                    break;
                default:
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            if (success)
            {
                DependencyService.Get<IFileStoreAndLoad>().SaveText("login.txt", "not");
                
                await DisplayAlert(cimkek.GetSuccess(),cimkek.GetDeletedAcoountSuccessful(),cimkek.GetOK());

                DBContext dBContext = new DBContext();

                dBContext.SendEmail("deleteaccount", user.EMAIL, user.FIRSTNAME, user.LASTNAME,"","","","","");

                await Navigation.PushModalAsync(new NavigationPage(new Login.Login()));
            }
            else
            {
                await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
            }
        }

        private async void logoutButton_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IFileStoreAndLoad>().SaveText("login.txt", "not");

            await Navigation.PushModalAsync(new NavigationPage(new Login.Login()));
        }

        private async void myfriendsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MyFriends.MyFriends());
        }

        #endregion

        #region tapped

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage(user));
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CreateEvents.CreateEvent(user));
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new JoinedEvents.JoinedEvents(user));
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Hashtags.Hashtags(user));
        }

        private async void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Settings(user));
        }

        #endregion
    }
}
