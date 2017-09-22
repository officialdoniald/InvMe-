using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.Labels.English;
using InvMe.BLL.Labels.ILabels;
using InvMe.DAL.Model;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvMe_.ForgotPassword
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPage : ContentPage
    {
        #region attr

        ICimkek cimkek = new Cimkek();

        DBContext DB = new DBContext();

        #endregion

        #region konstruktor
        
        public ForgotPage()
        {
            InitializeComponent();

            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                #region cimkek

                //forgotWelcome.Text = cimkek.GetForgotPassword();
                //emailLabel.Text = cimkek.GetEmail().ToLower();
                loadingLabel.Text = cimkek.GetLoading();
                sendEmailButton.Text = cimkek.GetSendEmail();

                mainStackLayout.IsVisible = true;
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
                    await DisplayAlert("Connectivity Changed", "IsConnected: " + Plugin.Connectivity.CrossConnectivity.Current.IsConnected, "OK");
                    //await Navigation.PushModalAsync(new NoInternet.NoInternetPage(this));
                }
                else
                {
                    #region cimkek

                    mainStackLayout.IsVisible = true;
                    //forgotWelcome.Text = cimkek.GetForgotPassword();
                    //emailLabel.Text = cimkek.GetEmail().ToLower();
                    loadingLabel.Text = cimkek.GetLoading();
                    sendEmailButton.Text = cimkek.GetSendEmail();

                    #endregion
                }
            };
        }

        #endregion

        #region click

        private async Task sendEmailButton_Clicked(object sender, EventArgs e)
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;

            mainStackLayout.IsVisible = false;

            ObservableCollection<User> listOfUser = new ObservableCollection<User>();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    listOfUser = DependencyService.Get<IiOSDatabaseAccess>().GetUser();
                    break;
                case Device.Android:
                    listOfUser = DependencyService.Get<IAndroidDatabaseAccess>().GetUser();
                    break;
                case Device.WinPhone:
                    listOfUser = await DependencyService.Get<IDatabaseAccess>().GetUser();
                    break;
                case Device.Windows:
                    listOfUser = await DependencyService.Get<IDatabaseAccess>().GetUser();
                    break;
                default:
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                    mainStackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            User user = new User();

            bool goodEmail = false;

            foreach (var item in listOfUser)
            {
                if (item.EMAIL == emailEntry.Text)
                {
                    user = item;
                    user.FACEBOOK = "asd";
                    goodEmail = true;

                    break;
                }
            }

            if (goodEmail)
            {
                user.PASSWORD = DB.RandomString(10, true);

                var success = await DependencyService.Get<IDatabaseAccess>().UpdateUser(user.id, user);
                
                if (success)
                {
                    string url = String.Format("http://invme.eu/invmeapp/forgotpassword.php?emaill={0}&nev={1}&pwdd={2}", emailEntry.Text, user.FIRSTNAME + "_" + user.LASTNAME, user.PASSWORD);

                    Uri uri = new Uri(url);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    WebResponse res = await request.GetResponseAsync();

                    await DisplayAlert(cimkek.GetSuccess(), cimkek.GetSentEmail(), cimkek.GetOK());

                    await Navigation.PushModalAsync(new Login.Login());
                }
                else
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                    mainStackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                }
            }
            else
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                mainStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetWrongEmail(), cimkek.GetOK());
            }
        }
        #endregion
    }
}
