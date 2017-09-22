using FileStoringWithDependency.IFileStoreAndLoad;
using InvMe.BLL.AzureLibraries;
using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.Labels.English;
using InvMe.BLL.Labels.ILabels;
using InvMe.DAL.Model;
using InvMe_.ForgotPassword;
using InvMe_.Register;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvMe_.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        #region attr

        ICimkek cimkek = new Cimkek();

        InvMeBlobStorage asd = new InvMeBlobStorage();

        #endregion

        #region konstruktor

        public Login()
        {
            InitializeComponent();

            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                mainStackLayout.IsVisible = true;

                #region cimkek

                //welcomeLabel.Text = cimkek.GetApplicationName();
                //emailLabel.Text = cimkek.GetEmail().ToLower();
                //passwordLabel.Text = cimkek.GetPassword().ToLower();
                loadingLabel.Text = cimkek.GetLoading();
                signinButton.Text = cimkek.GetSignIn();
                forgotLabel.Text = cimkek.GetForgotPassword();
                signupLabel.Text = cimkek.GetSignUp();

                #endregion
            }
            else
            {
                mainStackLayout.IsVisible = false;
            }
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    InitializeComponent();
                    mainStackLayout.IsVisible = false;
                    await DisplayAlert(cimkek.GetWarning(),cimkek.GetLostConnection(), cimkek.GetOK());
                    //await Navigation.PushModalAsync(new NoInternet.NoInternetPage(this));
                }
                else
                {
                    mainStackLayout.IsVisible = true;

                    #region cimkek

                    //welcomeLabel.Text = cimkek.GetApplicationName();
                    //emailLabel.Text = cimkek.GetEmail().ToLower();
                    //passwordLabel.Text = cimkek.GetPassword().ToLower();
                    loadingLabel.Text = cimkek.GetLoading();
                    signinButton.Text = cimkek.GetSignIn();
                    forgotLabel.Text = cimkek.GetForgotPassword();
                    signupLabel.Text = cimkek.GetSignUp();

                    #endregion
                }
            };

            #region cimkek

            //welcomeLabel.Text = cimkek.GetApplicationName();
            //emailLabel.Text = cimkek.GetEmail().ToLower();
            //passwordLabel.Text = cimkek.GetPassword().ToLower();
            loadingLabel.Text = cimkek.GetLoading();
            signinButton.Text = cimkek.GetSignIn();
            forgotLabel.Text = cimkek.GetForgotPassword();
            signupLabel.Text = cimkek.GetSignUp();

            #endregion
        }

        #endregion

        #region click

        private async void signinButton_Clicked(object sender, EventArgs e)
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;

            mainStackLayout.IsVisible = false;

            if (String.IsNullOrEmpty(emailEntry.Text) || String.IsNullOrEmpty(passwordEntry.Text))
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                mainStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetFillEverything(), cimkek.GetOK());
            }
            else
            {
                string EMAIL = emailEntry.Text.ToLower();
                string PW = passwordEntry.Text;

                ObservableCollection<User> asd = new ObservableCollection<User>();

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        asd = DependencyService.Get<IiOSDatabaseAccess>().GetUser();
                        break;
                    case Device.Android:
                        asd = DependencyService.Get<IAndroidDatabaseAccess>().GetUser();
                        break;
                    case Device.WinPhone:
                        asd = await DependencyService.Get<IDatabaseAccess>().GetUser();
                        break;
                    case Device.Windows:
                        asd = await DependencyService.Get<IDatabaseAccess>().GetUser();
                        break;
                    default:
                        indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                        mainStackLayout.IsVisible = true;

                        await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                        break;
                }

                DBContext DB = new DBContext();

                User user = new User();

                bool findExistEmailandPassword = false;

                foreach (var item in asd)
                {
                    if (item.EMAIL == EMAIL && item.PASSWORD == PW)
                    {
                        user = item;

                        findExistEmailandPassword = true;

                        break;
                    }
                }

                if (findExistEmailandPassword)
                {
                    DependencyService.Get<IFileStoreAndLoad>().SaveText("login.txt", user.EMAIL);

                    await Navigation.PushModalAsync(new MainPage(user));
                }
                else if (!DB.IsValidEmailAddress(emailEntry.Text))
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                    mainStackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetInvalidEmail(), cimkek.GetOK());
                }
                else
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                    mainStackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetWrongPasswordOrEmail(), cimkek.GetOK());
                }
            }
        }

        #endregion

        #region tapped

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ForgotPage());
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SignUp());
        }

        #endregion
    }
}
