using InvMe.BLL.Labels.English;
using InvMe.BLL.Labels.ILabels;
using InvMe.DAL.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvMe_.UserDescription
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDescriptionPage : ContentPage
    {
        #region attr

        User user = new User();

        User selectedUser = new User();

        ICimkek cimkek = new Cimkek();

        #endregion

        #region konstruktor

        public UserDescriptionPage(User currentUser, User selectedUser)
        {
            this.user = currentUser;
            this.selectedUser = selectedUser;

            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                InitializeComponent();

                userDescriptionWelcome.Text = cimkek.GetUserDescription();
                loadingLabel.Text = cimkek.GetLoading();
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
                indicatorStackLayout.IsVisible = true;
                generalStackLayout.IsVisible = false;
                
                homeImage.Source = "home.png";
                addImage.Source = "add.png";
                joinedImage.Source = "calendar.png";
                hashtagsImage.Source = "hashtags.png";
                settingsImage.Source = "settings.png";

                if (String.IsNullOrEmpty(selectedUser.FACEBOOK)) facebookLink.IsVisible = false;
                else facebookLink.IsVisible = true;

                if (selectedUser.PROFILEPICTURE != "luser.png")
                {
                    profilepictureImage.Source = ImageSource.FromUri(new Uri(selectedUser.PROFILEPICTURE));
                }
                else
                {
                    profilepictureImage.Source = "luser.png";
                }
                firstnameLabel.Text = selectedUser.FIRSTNAME + " " + selectedUser.LASTNAME;
                bornDateLabel.Text = selectedUser.BORNDATE.ToString("dd/MM/yyyy");
                emailLabel.Text = selectedUser.EMAIL;
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                generalStackLayout.IsVisible = true;
            }
            else
            {
                InitializeComponent();
                generalStackLayout.IsVisible = false;
            }
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    InitializeComponent();
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
                    generalStackLayout.IsVisible = false;
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetLostConnection(), cimkek.GetOK());
                    //await Navigation.PushModalAsync(new NoInternetPage(this));
                }
                else
                {
                    InitializeComponent();

                    userDescriptionWelcome.Text = cimkek.GetUserDescription();
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
                    loadingLabel.Text = cimkek.GetLoading();
                    activityIndicator.IsVisible = activityIndicator.IsRunning = true;
                    indicatorStackLayout.IsVisible = true;
                    generalStackLayout.IsVisible = false;

                    homeImage.Source = "home.png";
                    addImage.Source = "add.png";
                    joinedImage.Source = "calendar.png";
                    hashtagsImage.Source = "hashtags.png";
                    settingsImage.Source = "settings.png";

                    if (String.IsNullOrEmpty(selectedUser.FACEBOOK)) facebookLink.IsVisible = false;
                    else facebookLink.IsVisible = true;

                    if (selectedUser.PROFILEPICTURE != "luser.png")
                    {
                        profilepictureImage.Source = ImageSource.FromUri(new Uri(selectedUser.PROFILEPICTURE));
                    }
                    else
                    {
                        profilepictureImage.Source = "luser.png";
                    }
                    firstnameLabel.Text = selectedUser.FIRSTNAME + " " + selectedUser.LASTNAME;
                    bornDateLabel.Text = selectedUser.BORNDATE.ToString("dd/MM/yyyy");
                    emailLabel.Text = selectedUser.EMAIL;
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                    indicatorStackLayout.IsVisible = false;
                    generalStackLayout.IsVisible = true;
                }
            };
        }

        #endregion

        #region click

        private async void facebookButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                Uri uri = new Uri(selectedUser.FACEBOOK);
                Device.OpenUri(uri);
            }
            catch (Exception)
            {
                await DisplayAlert(cimkek.GetWarning(),cimkek.GetNoFacebookAccount(),cimkek.GetOK());
            }
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
            await Navigation.PushModalAsync(new Settings.Settings(user));
        }

        private void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
