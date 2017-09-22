using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.Labels.English;
using InvMe.BLL.Labels.ILabels;
using InvMe.BLL.MapClasses;
using InvMe.DAL.Model;
using InvMe_.CreateEvents;
using InvMe_.UserDescription;
using Plugin.ExternalMaps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace InvMe_.EventDescription
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventDescription : ContentPage
    {
        #region attr

        Events ThisEvent = new Events();

        User user = new User();

        Attended attend = new Attended();

        bool isAttended = false;

        Pin ppin = new Pin();

        Pin mpin = new Pin();

        ICimkek cimkek = new Cimkek();

        #endregion

        #region konstruktor
        
        public EventDescription(User user, Events events)
        {
            InitializeComponent();

            loadingLabel.Text = cimkek.GetLoading();
            activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            indicatorStackLayout.IsVisible = true;
            generalSackLayout.IsVisible = false;

            this.user = user;

            ThisEvent = events;

            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                isAttendedorNot();

                SeeTheEvent();
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
                    InitializeComponent();

                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
                    mainStackLayout.IsVisible = false;

                    isAttendedorNot();

                    SeeTheEvent();
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                    mainStackLayout.IsVisible = true;
                }
            };
        }

        public EventDescription(User user, Events events, bool isAttended)
        {
            InitializeComponent();

            loadingLabel.Text = cimkek.GetLoading();
            activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            indicatorStackLayout.IsVisible = true;
            generalSackLayout.IsVisible = false;

            this.user = user;

            this.isAttended = isAttended;

            ThisEvent = events;

            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                SeeTheEvent();
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
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
                    mainStackLayout.IsVisible = false;
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetLostConnection(), cimkek.GetOK());
                    //await Navigation.PushModalAsync(new NoInternet.NoInternetPage(this));
                }
                else
                {
                    SeeTheEvent();
                }
            };

        }

        #endregion

        #region segedfvk
        
        private async void isAttendedorNot()
        {
            ObservableCollection<Attended> attended = new ObservableCollection<Attended>();

            attended = new ObservableCollection<Attended>();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    attended = DependencyService.Get<IiOSDatabaseAccess>().GetAttendedByID(user.id);
                    break;
                case Device.Android:
                    attended = DependencyService.Get<IAndroidDatabaseAccess>().GetAttendedByID(user.id);
                    break;
                case Device.WinPhone:
                    attended = await DependencyService.Get<IDatabaseAccess>().GetAttendedByID(user.id);
                    break;
                case Device.Windows:
                    attended = await DependencyService.Get<IDatabaseAccess>().GetAttendedByID(user.id);
                    break;
                default:
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            foreach (var item in attended)
            {
                if (ThisEvent.id == item.event_id) { attend = item; isAttended = true; break; }
            }
        }

        private async void SeeTheEvent()
        {
            ObservableCollection<Attended> attendedToThisEvent = new ObservableCollection<Attended>();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    attendedToThisEvent = DependencyService.Get<IiOSDatabaseAccess>().GetAttendedByEventID(ThisEvent.id);
                    break;
                case Device.Android:
                    attendedToThisEvent = DependencyService.Get<IAndroidDatabaseAccess>().GetAttendedByEventID(ThisEvent.id);
                    break;
                case Device.WinPhone:
                    attendedToThisEvent = await DependencyService.Get<IDatabaseAccess>().GetAttendedByEventID(ThisEvent.id);
                    break;
                case Device.Windows:
                    attendedToThisEvent = await DependencyService.Get<IDatabaseAccess>().GetAttendedByEventID(ThisEvent.id);
                    break;
                default:
                    await DisplayAlert("Warning", "Something went wrong, please check back later!", "OK");
                    break;
            }

            whoattendedLabelText.Text = cimkek.GetWhoAttended();
            mainStackLayout.IsVisible = true;
            homeImage.Source = "home.png";
            addImage.Source = "add.png";
            joinedImage.Source = "calendar.png";
            hashtagsImage.Source = "hashtags.png";
            settingsImage.Source = "settings.png";

            eventNameLabel.Text = ThisEvent.EVENTNAME;

            DateTimeOffset dto_begin = new DateTimeOffset();
            DateTimeOffset dto_end = new DateTimeOffset();
            DateTimeOffset.TryParse(ThisEvent.FROM, out dto_begin);
            DateTimeOffset.TryParse(ThisEvent.TO, out dto_end);
            dto_begin = dto_begin.ToLocalTime();
            dto_end = dto_end.ToLocalTime();

            eventFromLabel.Text = dto_begin.ToString("dddd, MMM dd yyyy HH:mm");

            if (ThisEvent.FROM == ThisEvent.TO)
            {
                eventToLabel.Text = "Dosn't matter";
            }
            else
            {
                eventToLabel.Text = dto_end.ToString("dddd, MMM dd yyyy HH:mm");
            }
            

            if (ThisEvent.ONLINE == 1)
            {
                eventTownLabel.Text = "Online";

                eventPlaceLabel.IsVisible = false;
                eventPlaceImage.IsVisible = false;

                eventStack.IsVisible = false;
                meetStack.IsVisible = false;
                GetDirectionMeetingButton.IsVisible = false;
                GetDirectionPlaceButton.IsVisible = false;
            }
            else
            {
                eventTownLabel.Text = ThisEvent.TOWN;
                eventPlaceLabel.Text = ThisEvent.PLACE;


                Position ppos = new Position(Convert.ToDouble(ThisEvent.PLACECORD.Split(';')[0]),
                    Convert.ToDouble(ThisEvent.PLACECORD.Split(';')[1]));
                Position mpos = new Position(Convert.ToDouble(ThisEvent.MEETINGCORD.Split(';')[0]),
                    Convert.ToDouble(ThisEvent.MEETINGCORD.Split(';')[1]));

                ppin = new Pin();
                mpin = new Pin();

                ppin.Position = ppos;
                ppin.Label = ThisEvent.EVENTNAME + ", " + ThisEvent.PLACE;
                ppin.Type = PinType.Place;
                ppin.Address = "";
                mpin.Position = mpos;
                mpin.Label = "Meeting place";
                mpin.Type = PinType.Place;
                mpin.Address = "";
                var pin = new CustomPin
                {
                    Pin = ppin,
                    Id = "Xamarin",
                    Url = "http://invme.eu"
                };
                var pin2 = new CustomPin
                {
                    Pin = mpin,
                    Id = "Xamarin2",
                    Url = "http://invme.eu"
                };

                CustomMap eventPlaceMap = new CustomMap()
                {
                    IsShowingUser = true,
                    HeightRequest = 200,
                    isJustShow = true,
                    longitude = pin.Pin.Position.Longitude,
                    latitude = pin.Pin.Position.Latitude
                };
                eventPlaceMap.CustomPins = new List<CustomPin> { pin };
                eventPlaceMap.Pins.Add(pin.Pin);

                CustomMap meetPlaceMap = new CustomMap()
                {
                    IsShowingUser = true,
                    HeightRequest = 200,
                    isJustShow = true,
                    longitude = pin2.Pin.Position.Longitude,
                    latitude = pin2.Pin.Position.Latitude
                };
                meetPlaceMap.CustomPins = new List<CustomPin> { pin2 };
                meetPlaceMap.Pins.Add(pin2.Pin);

                eventStack.Children.Add(eventPlaceMap);
                meetStack.Children.Add(meetPlaceMap);
                
                eventPlaceMap.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                    ppos, Distance.FromMiles(1.0)));
                eventPlaceMap.IsShowingUser = true;
                
                meetPlaceMap.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        mpos, Distance.FromMiles(1.0)));
                meetPlaceMap.IsShowingUser = true;
            }

            if (ThisEvent.HOWMANY == 1)
            {
                howManyLabel.Text = "Anyone";
            }
            else
            {
                howManyLabel.Text = ThisEvent.HOWMANY.ToString() + "/" + attendedToThisEvent.Count;
            }

            ObservableCollection<AttendedUser> attendedUserCollection = new ObservableCollection<AttendedUser>();

            AttendedUser attendedUser = new AttendedUser();

            foreach (var item in attendedToThisEvent)
            {
                attendedUser = new AttendedUser();

                attendedUser.ID = item.user_id;
                
                User getUser = new User();

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        getUser = DependencyService.Get<IiOSDatabaseAccess>().GetUserByID(item.user_id);
                        break;
                    case Device.Android:
                        getUser = DependencyService.Get<IAndroidDatabaseAccess>().GetUserByID(item.user_id);
                        break;
                    case Device.WinPhone:
                        getUser = await DependencyService.Get<IDatabaseAccess>().GetUserByID(item.user_id);
                        break;
                    case Device.Windows:
                        getUser = await DependencyService.Get<IDatabaseAccess>().GetUserByID(item.user_id);
                        break;
                    default:
                        await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                        break;
                }

                if (getUser.PROFILEPICTURE != "luser.png")
                {
                    attendedUser.PROFILEPICTURE = ImageSource.FromUri(new Uri(getUser.PROFILEPICTURE));
                }
                else
                {
                    attendedUser.PROFILEPICTURE = "suser.png";
                }

                attendedUser.FIRSTNAME = (getUser).FIRSTNAME + " " + (getUser).LASTNAME;

                attendedUserCollection.Add(attendedUser);

                generalSackLayout.IsVisible = true;
            }

            if (isAttended) submitordeleteButton.Text = "Leave";
            else submitordeleteButton.Text = "Attend";

            if (ThisEvent.HOWMANY != 1 && attendedToThisEvent.Count >= ThisEvent.HOWMANY)
            {
                submitordeleteButton.IsVisible = false;
            }
            
            attendedListView.ItemsSource = attendedUserCollection;

            indicatorStackLayout.IsVisible = false;
        }

        #endregion

        #region clicks

        private async void submitordeleteButton_Clicked(object sender, EventArgs e)
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;

            generalSackLayout.IsVisible = false;

            if (isAttended)
            {
                bool success = false;

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        success = DependencyService.Get<IiOSDatabaseAccess>().DeleteAttended(attend);
                        break;
                    case Device.Android:
                        success = DependencyService.Get<IAndroidDatabaseAccess>().DeleteAttended(attend);
                        break;
                    case Device.WinPhone:
                        success = await DependencyService.Get<IDatabaseAccess>().DeleteAttended(attend);
                        break;
                    case Device.Windows:
                        success = await DependencyService.Get<IDatabaseAccess>().DeleteAttended(attend);
                        break;
                    default:
                        indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                        generalSackLayout.IsVisible = true;

                        await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                        break;
                }

                if (success)
                {
                    await DisplayAlert(cimkek.GetSuccess(), cimkek.GetSuccessfulUnsubscribed(), cimkek.GetOK());

                    await Navigation.PushModalAsync(new EventDescription(user, ThisEvent, false));
                }
                else
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                    generalSackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                }
            }
            else
            {
                attend = new Attended();
                attend.user_id = user.id;
                attend.event_id = ThisEvent.id;

                bool success = false;

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        success = DependencyService.Get<IiOSDatabaseAccess>().InsertAttended(attend);
                        break;
                    case Device.Android:
                        success = DependencyService.Get<IAndroidDatabaseAccess>().InsertAttended(attend);
                        break;
                    case Device.WinPhone:
                        success = await DependencyService.Get<IDatabaseAccess>().InsertAttendedAsync(attend);
                        break;
                    case Device.Windows:
                        success = await DependencyService.Get<IDatabaseAccess>().InsertAttendedAsync(attend);
                        break;
                    default:
                        indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                        generalSackLayout.IsVisible = true;

                        await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                        break;
                }

                if (success)
                {
                    await DisplayAlert(cimkek.GetSuccess(), cimkek.GetSuccessfulAttendToThisEvent(), cimkek.GetOK());

                    DBContext dBContext = new DBContext();

                    dBContext.SendEmail("joinedevent", user.EMAIL, user.FIRSTNAME, user.LASTNAME, ThisEvent.EVENTNAME, ThisEvent.FROM, ThisEvent.TO, ThisEvent.TOWN, ThisEvent.PLACE);

                    await Navigation.PushModalAsync(new EventDescription(user, ThisEvent));
                }
                else
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                    generalSackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                }
            }
        }

        private async void GetDirectionPlaceButton_Clicked(object sender, EventArgs e)
        {
            await CrossExternalMaps.Current.NavigateTo(
                ppin.Label, 
                ppin.Position.Latitude, 
                ppin.Position.Longitude,
                Plugin.ExternalMaps.Abstractions.NavigationType.Driving
                );
        }

        private void GetDirectionMeetingButton_Clicked(object sender, EventArgs e)
        {
            CrossExternalMaps.Current.NavigateTo(
                mpin.Label,
                mpin.Position.Latitude,
                mpin.Position.Longitude,
                Plugin.ExternalMaps.Abstractions.NavigationType.Driving
                );
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
            await Navigation.PushModalAsync(new Hashtags.Hashtags(user));
        }

        private async void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Settings.Settings(user));
        }

        private async void attendedListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedAttendedUser = ((AttendedUser)attendedListView.SelectedItem);

            User selectedUser = new User();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    selectedUser = DependencyService.Get<IiOSDatabaseAccess>().GetUserByID(selectedAttendedUser.ID);
                    break;
                case Device.Android:
                    selectedUser = DependencyService.Get<IAndroidDatabaseAccess>().GetUserByID(selectedAttendedUser.ID);
                    break;
                case Device.WinPhone:
                    selectedUser = await DependencyService.Get<IDatabaseAccess>().GetUserByID(selectedAttendedUser.ID);
                    break;
                case Device.Windows:
                    selectedUser = await DependencyService.Get<IDatabaseAccess>().GetUserByID(selectedAttendedUser.ID);
                    break;
                default:
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            await Navigation.PushModalAsync(new UserDescriptionPage(user, selectedUser));
        }

        #endregion
    }
}
