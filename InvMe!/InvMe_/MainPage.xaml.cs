using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.Labels.English;
using InvMe.BLL.Labels.ILabels;
using InvMe.DAL.Model;
using InvMe_.CreateEvents;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace InvMe_
{
    public partial class MainPage : ContentPage
    {
        #region attr

        User user = new User();

        HashtagsM hashtag = new HashtagsM();

        ICimkek cimkek = new Cimkek();

        #endregion

        #region konstruktor

        public MainPage(User user, HashtagsM hashtag)
        {
            InitializeComponent();

            loadingLabel.Text = cimkek.GetLoading();
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;

            this.user = user;

            this.hashtag = hashtag;

            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    InitializeComponent();
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
                    mainStackLayout.IsVisible = false;
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetLostConnection(), cimkek.GetOK());
                    //await Navigation.PushModalAsync(new NoInternetPage(new MainPage(user,hashtag)));
                }
            };
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                GetTheEvents();
            }
            else
            {
                InitializeComponent();
                mainStackLayout.IsVisible = false;
            }
        }

        public MainPage(User user)
        {
            this.user = user;

            InitializeComponent();

            loadingLabel.Text = cimkek.GetLoading();
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;

            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    InitializeComponent();
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
                    mainStackLayout.IsVisible = false;
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetLostConnection(), cimkek.GetOK());
                    //await Navigation.PushModalAsync(new NoInternetPage(new MainPage(user)));
                }
                else
                {
                    getTheUserFromTheDB();
                    mainStackLayout.IsVisible = true;
                }
            };
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                getTheUserFromTheDB();
            }
            else
            {
                mainStackLayout.IsVisible = false;
            }
        }

        #endregion

        #region segedfvk

        private async void getTheUserFromTheDB()
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;
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

            foreach (var item in asd)
            {
                if (item.EMAIL == user.EMAIL)
                {
                    this.user = item;
                    break;
                }
            }

            GetTheEvents();
        }

        private async void GetTheEvents()
        {
            ObservableCollection<Events> eventsFromDB = new ObservableCollection<Events>();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    eventsFromDB = DependencyService.Get<IiOSDatabaseAccess>().GetEvent();
                    break;
                case Device.Android:
                    eventsFromDB = DependencyService.Get<IAndroidDatabaseAccess>().GetEvent();
                    break;
                case Device.WinPhone:
                    eventsFromDB = await DependencyService.Get<IDatabaseAccess>().GetEvent();
                    break;
                case Device.Windows:
                    eventsFromDB = await DependencyService.Get<IDatabaseAccess>().GetEvent();
                    break;
                default:
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            ObservableCollection<BindableEvent> bindableEventList = new ObservableCollection<BindableEvent>();
            ObservableCollection<Events> asd = new ObservableCollection<Events>();

            if (String.IsNullOrEmpty(hashtag.TOWN))
            {
                hashtag.TOWN = "";
            }
            if (String.IsNullOrEmpty(hashtag.HASHTAG))
            {
                hashtag.HASHTAG = "";
            }

            DateTimeOffset dts = DateTimeOffset.Now;

            foreach (var item in eventsFromDB)
            {
                DateTimeOffset dto_begin = new DateTimeOffset();
                DateTimeOffset dto_end = new DateTimeOffset();
                DateTimeOffset.TryParse(item.FROM, out dto_begin);
                DateTimeOffset.TryParse(item.TO, out dto_end);
                dto_begin = dto_begin.ToLocalTime();
                dto_end = dto_end.ToLocalTime();

                if (((dts >= dto_begin && dts < dto_end) || (dts < dto_end || (item.FROM == item.TO && dts < dto_begin)) || (dts <= dto_begin && dts > dto_end)))
                {
                    if (String.IsNullOrEmpty(hashtag.HASHTAG) &&
                        String.IsNullOrEmpty(hashtag.TOWN))
                    {
                        asd.Add(item);
                    }
                    else if (item.ONLINE != 1)
                    {
                        if (hashtag.TOWN == "Online") {;}
                        else if (!String.IsNullOrEmpty(hashtag.TOWN.ToLower()) &&
                            String.IsNullOrEmpty(hashtag.HASHTAG) &&
                          item.TOWN.ToLower().Contains(hashtag.TOWN.ToLower()))
                        {
                            asd.Add(item);
                        }
                        else if (String.IsNullOrEmpty(hashtag.TOWN) &&
                            !String.IsNullOrEmpty(hashtag.HASHTAG.ToLower()) &&
                          item.EVENTNAME.ToLower().Contains(hashtag.HASHTAG.ToLower()))
                        {
                            asd.Add(item);
                        }
                        else if (!String.IsNullOrEmpty(hashtag.TOWN.ToLower()) &&
                            !String.IsNullOrEmpty(hashtag.HASHTAG.ToLower()) &&
                          item.EVENTNAME.ToLower().Contains(hashtag.HASHTAG.ToLower()) &&
                          item.TOWN.ToLower().Contains(hashtag.TOWN.ToLower()))
                        {
                            asd.Add(item);
                        }
                    }
                    else if (item.ONLINE == 1)
                    {
                        if (!String.IsNullOrEmpty(hashtag.HASHTAG.ToLower()) &&
                            !String.IsNullOrEmpty(hashtag.TOWN.ToLower()))
                        {
                            if (item.EVENTNAME.ToLower().Contains(hashtag.HASHTAG.ToLower()) &&
                          hashtag.TOWN == "Online")
                            {
                                asd.Add(item);
                            }
                        }
                        else if ((!String.IsNullOrEmpty(hashtag.HASHTAG.ToLower()) &&
                            item.EVENTNAME.ToLower().Contains(hashtag.HASHTAG.ToLower())) ||
                            hashtag.TOWN == "Online")
                        {
                            asd.Add(item);
                        }
                    }
                }
            }

            foreach (var item in asd)
            {
                BindableEvent bindableEvent = new BindableEvent();

                bindableEvent.ID = item.id;

                DateTimeOffset dto_begin = new DateTimeOffset();
                DateTimeOffset dto_end = new DateTimeOffset();
                DateTimeOffset.TryParse(item.FROM, out dto_begin);
                DateTimeOffset.TryParse(item.TO, out dto_end);
                dto_begin = dto_begin.ToLocalTime();
                dto_end = dto_end.ToLocalTime();

                bindableEvent.EVENTNAME = item.EVENTNAME;
                bindableEvent.FROM = dto_begin.ToString("dddd, MMM dd yyyy HH:mm");

                if (item.ONLINE == 1) bindableEvent.TOWN = "Online";
                else bindableEvent.TOWN = item.TOWN + ", " + item.PLACE;

                if (item.FROM == item.TO) bindableEvent.TO = "No matter";
                else bindableEvent.TO = dto_end.ToString("dddd, MMM dd yyyy HH:mm");

                bindableEventList.Add(bindableEvent);
            }
            
            #region labels

            searchButton.Text = cimkek.GetSearch();
            onlineLabel.Text = cimkek.GetOnline();

            #endregion

            mainStackLayout.IsVisible = true;
            homeImage.Source = "thome.png";
            addImage.Source = "add.png";
            joinedImage.Source = "calendar.png";
            hashtagsImage.Source = "hashtags.png";
            settingsImage.Source = "settings.png";
            
            eventListView.ItemsSource = bindableEventList;

            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
            mainStackLayout.IsVisible = true;
        }

        #endregion

        #region itemTapped

        private async void eventListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            int ID = ((BindableEvent)eventListView.SelectedItem).ID;

            Events selectedEvent = new Events();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    selectedEvent = DependencyService.Get<IiOSDatabaseAccess>().GetEventByID(ID);
                    break;
                case Device.Android:
                    selectedEvent = DependencyService.Get<IAndroidDatabaseAccess>().GetEventByID(ID);
                    break;
                case Device.WinPhone:
                    selectedEvent = await DependencyService.Get<IDatabaseAccess>().GetEventByID(ID);
                    break;
                case Device.Windows:
                    selectedEvent = await DependencyService.Get<IDatabaseAccess>().GetEventByID(ID);
                    break;
                default:
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            await Navigation.PushModalAsync(new EventDescription.EventDescription(user, selectedEvent));
        }

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
            await Navigation.PushModalAsync(new Hashtags.Hashtags(user));
        }

        private async void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Settings.Settings(user));
        }

        #endregion

        #region toggled

        private void onlineSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (onlineSwitch.IsToggled)
            {
                hashtag.TOWN = "Online";
                townEntry.IsVisible = !townEntry.IsVisible;
                townImage.IsVisible = !townImage.IsVisible;
            }
            else
            {
                hashtag.TOWN = townEntry.Text;
                townEntry.IsVisible = !townEntry.IsVisible;
                townImage.IsVisible = !townImage.IsVisible;
            }
        }

        #endregion

        #region click

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!onlineSwitch.IsToggled) hashtag.TOWN = townEntry.Text;
            else if (!String.IsNullOrEmpty(hashtagEntry.Text) ||
                !String.IsNullOrEmpty(townEntry.Text) ||
                hashtag.TOWN == "Online"
                )
            {
                hashtag.HASHTAG = "";
                hashtag.HASHTAG = hashtagEntry.Text;

                await Navigation.PushModalAsync(new MainPage(user, hashtag));
            }
            else if (String.IsNullOrEmpty(hashtagEntry.Text) &&
                String.IsNullOrEmpty(townEntry.Text))
            {
                hashtag = new HashtagsM();

                await Navigation.PushModalAsync(new MainPage(user, hashtag));
            }
        }

        #endregion
    }
}
