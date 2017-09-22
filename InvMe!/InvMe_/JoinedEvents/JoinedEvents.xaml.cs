using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.Labels.English;
using InvMe.BLL.Labels.ILabels;
using InvMe.DAL.Model;
using InvMe_.CreateEvents;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvMe_.JoinedEvents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinedEvents : ContentPage
    {
        #region attr

        ObservableCollection<Events> eventsFromDB = new ObservableCollection<Events>();

        ObservableCollection<BindableEvent> bindableEventList = new ObservableCollection<BindableEvent>();

        ObservableCollection<Attended> attendedEvents = new ObservableCollection<Attended>();

        BindableEvent bindableEvent = new BindableEvent();

        User user = new User();

        ICimkek cimkek = new Cimkek();

        #endregion

        #region konstruktor

        public JoinedEvents(User user)
        {
            InitializeComponent();
            
            loadingLabel.Text = cimkek.GetLoading();
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;

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
                else
                {
                    GetTheEvents();
                }
            };
        }

        #endregion

        #region segedfvk

        private async void GetTheEvents()
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;
            eventsFromDB = new ObservableCollection<Events>();

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
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                    mainStackLayout.IsVisible = true;
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            bindableEventList = new ObservableCollection<BindableEvent>();

            attendedEvents = new ObservableCollection<Attended>();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    attendedEvents = DependencyService.Get<IiOSDatabaseAccess>().GetAttendedByID(user.id);
                    break;
                case Device.Android:
                    attendedEvents = DependencyService.Get<IAndroidDatabaseAccess>().GetAttendedByID(user.id);
                    break;
                case Device.WinPhone:
                    attendedEvents = await DependencyService.Get<IDatabaseAccess>().GetAttendedByID(user.id);
                    break;
                case Device.Windows:
                    attendedEvents = await DependencyService.Get<IDatabaseAccess>().GetAttendedByID(user.id);
                    break;
                default:
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }
            
            ObservableCollection<Events> asd = new ObservableCollection<Events>();

            DateTimeOffset dts = DateTimeOffset.Now.ToLocalTime();

            bool attendornot = false;

            foreach (var item in eventsFromDB)
            {
                for (int i = 0; i < attendedEvents.Count; i++)
                {
                    if (item.id == attendedEvents[i].event_id)
                    {
                        attendornot = true;
                        break;
                    }
                }

                DateTimeOffset dto_begin = new DateTimeOffset();
                DateTimeOffset dto_end = new DateTimeOffset();
                DateTimeOffset.TryParse(item.FROM, out dto_begin);
                DateTimeOffset.TryParse(item.TO, out dto_end);
                dto_begin = dto_begin.ToLocalTime();
                dto_end = dto_end.ToLocalTime();
                if (attendornot && ((dts >= dto_begin && dts < dto_end) || (dts < dto_end || (item.FROM == item.TO && dts < dto_begin)) || (dts <= dto_begin && dts > dto_end)))
                {
                    asd.Add(item);
                }
            }

            foreach (var item in asd)
            {
                bindableEvent = new BindableEvent();

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
            
            updateWelcome.Text = cimkek.GetJoinedEvents();
            mainStackLayout.IsVisible = true;
            homeImage.Source = "home.png";
            addImage.Source = "add.png";
            joinedImage.Source = "tcalendar.png";
            hashtagsImage.Source = "hashtags.png";
            settingsImage.Source = "settings.png";

            eventListView.ItemsSource = bindableEventList;

            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
            mainStackLayout.IsVisible = true;
        }

        #endregion

        #region tapped

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

            await Navigation.PushModalAsync(new EventDescription.EventDescription(user, selectedEvent, true));
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
            await Navigation.PushModalAsync(new JoinedEvents(user));
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
    }
}
