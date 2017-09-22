using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.Labels.English;
using InvMe.BLL.Labels.ILabels;
using InvMe.DAL.Model;
using InvMe_.CreateEvents;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvMe_.Hashtags
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Hashtags : ContentPage
    {
        #region attr

        ObservableCollection<HashtagsM> hashtags = new ObservableCollection<HashtagsM>();
        ObservableCollection<HashtagsM> Bindablehashtags = new ObservableCollection<HashtagsM>();

        private bool isOnline = false;

        User user = new User();

        ICimkek cimkek = new Cimkek();

        #endregion

        #region konstruktor

        public Hashtags(User user)
        {
            this.user = user;

            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                GetTheEvents();
            }
            else
            {
                InitializeComponent();
                mainStackLayout.IsVisible = false;
            }
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    InitializeComponent();
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
                    mainStackLayout.IsVisible = false;
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetLostConnection(), cimkek.GetOK());
                    //await Navigation.PushModalAsync(new NoInternet.NoInternetPage(this));
                }
                else if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    GetTheEvents();
                }
            };
        }

        #endregion

        #region segedfvk

        private async void GetTheEvents()
        {
            InitializeComponent();

            loadingLabel.Text = cimkek.GetLoading();
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;

            hashtags = new ObservableCollection<HashtagsM>();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    hashtags = DependencyService.Get<IiOSDatabaseAccess>().GetHashtag();
                    break;
                case Device.Android:
                    hashtags = DependencyService.Get<IAndroidDatabaseAccess>().GetHashtag();
                    break;
                case Device.WinPhone:
                    hashtags = await DependencyService.Get<IDatabaseAccess>().GetHashtag();
                    break;
                case Device.Windows:
                    hashtags = await DependencyService.Get<IDatabaseAccess>().GetHashtag();
                    break;
                default:
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            Bindablehashtags = new ObservableCollection<HashtagsM>();

            foreach (var item in hashtags)
            {
                if (item.UID == user.id)
                {
                    Bindablehashtags.Add(item);
                }
            }

            #region labels
            
            //eventnameLabel.Text = cimkek.GetEventName();
            //eventtownLabel.Text = cimkek.GetEventTown();
            addButton.Text = cimkek.GetAddHashtag();
            onlineLabel.Text = cimkek.GetOnline();
            updateWelcome.Text = cimkek.GetAddHashtag();

            #endregion

            mainStackLayout.IsVisible = true;
            homeImage.Source = "home.png";
            addImage.Source = "add.png";
            joinedImage.Source = "calendar.png";
            hashtagsImage.Source = "thashtags.png";
            settingsImage.Source = "settings.png";

            hashtagsListView.ItemsSource = Bindablehashtags;

            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
            mainStackLayout.IsVisible = true;

        }

        #endregion

        #region toggled
        private void onlineSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            isOnline = !isOnline;
            townEntry.IsVisible = !townEntry.IsVisible;
        }

        public async void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var listItem = (HashtagsM)mi.CommandParameter;

            await Navigation.PushModalAsync(new MainPage(user, listItem));
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;

            var mi = ((MenuItem)sender);
            var listItem = (HashtagsM)mi.CommandParameter;

            bool success = false;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    success = DependencyService.Get<IiOSDatabaseAccess>().DeleteHashtag(listItem);
                    break;
                case Device.Android:
                    success = DependencyService.Get<IAndroidDatabaseAccess>().DeleteHashtag(listItem);
                    break;
                case Device.WinPhone:
                    success = await DependencyService.Get<IDatabaseAccess>().DeleteHashtag(listItem);
                    break;
                case Device.Windows:
                    success = await DependencyService.Get<IDatabaseAccess>().DeleteHashtag(listItem);
                    break;
                default:
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                    mainStackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            if (success)
            {
                await DisplayAlert(cimkek.GetSuccess(), cimkek.GetDeleteFromYourHashtags(), cimkek.GetOK());

                await Navigation.PushModalAsync(new Hashtags(user));
            }
            else
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                mainStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
            }
        }

        #endregion

        #region click

        private async void Button_Clicked(object sender, EventArgs e)
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;

            HashtagsM hashtag = new HashtagsM();

            hashtag.UID = user.id;

            if (isOnline)
            {
                hashtag.TOWN = "Online";
            }
            else
            {
                if (!String.IsNullOrEmpty(townEntry.Text))
                {
                    hashtag.TOWN = townEntry.Text;
                }
                else
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                    mainStackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetFillTheHashtagEntry(), cimkek.GetOK());
                }
            }

            if (String.IsNullOrEmpty(hashtagEntry.Text))
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                mainStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetFillTheTownEntry(), cimkek.GetOK());
            }
            else
            {
                hashtag.HASHTAG = hashtagEntry.Text;

                string success = "s";

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        success =  DependencyService.Get<IiOSDatabaseAccess>().InsertHashtag(hashtag);
                        break;
                    case Device.Android:
                        success = DependencyService.Get<IAndroidDatabaseAccess>().InsertHashtag(hashtag);
                        break;
                    case Device.WinPhone:
                        success = await DependencyService.Get<IDatabaseAccess>().InsertHashtag(hashtag);
                        break;
                    case Device.Windows:
                        success = await DependencyService.Get<IDatabaseAccess>().InsertHashtag(hashtag);
                        break;
                    default:
                        indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                        mainStackLayout.IsVisible = true;

                        await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                        break;
                }

                if (success == "")
                {
                    await DisplayAlert(cimkek.GetSuccess(), cimkek.GetAddedToYourHashtags(), cimkek.GetOK());

                    await Navigation.PushModalAsync(new Hashtags(user));
                }
                else
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                    mainStackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                }
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
            await Navigation.PushModalAsync(new CreateEvent(user));
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new JoinedEvents.JoinedEvents(user));
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Hashtags(user));
        }

        private async void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Settings.Settings(user));
        }

        #endregion
    }
}
