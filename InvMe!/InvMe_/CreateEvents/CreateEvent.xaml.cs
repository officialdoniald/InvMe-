using FileStoringWithDependency.IFileStoreAndLoad;
using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.Labels.English;
using InvMe.BLL.Labels.ILabels;
using InvMe.BLL.MapClasses;
using InvMe.DAL.Model;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace InvMe_.CreateEvents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateEvent : ContentPage
    {
        #region attr

        private bool noMatterHowLong = false;
        private bool onlineEvent = false;
        private bool noMatterHowManyPerson = false;
        private bool fillExeption = false;
        private bool convertint = false;
        private bool begindateproblem = false;
        private bool enddateproblem = false;
        private bool fileproblems = false;
        private bool begindatenotbiggerthentheend = false;

        CustomMap map;
        CustomMap map2;

        User user;

        ICimkek cimkek = new Cimkek();

        #endregion

        #region konstruktor
        
        public CreateEvent(User user)
        {
            this.user = user;

            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    InitializeComponent();
                    simpleStackLayout.IsVisible = false;
                    await GetUserLocation();
                    simpleStackLayout.IsVisible = true;
                });
            }
            else
            {
                simpleStackLayout.IsVisible = false;
            }
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    InitializeComponent();
                    activityIndicator.IsVisible = activityIndicator.IsRunning = true;
                    simpleStackLayout.IsVisible = false;
                    indicatorStackLayout.IsVisible = false;
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetLostConnection(), cimkek.GetOK());
                    //await Navigation.PushModalAsync(new NoInternetPage(this));
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () => {
                        InitializeComponent();
                        simpleStackLayout.IsVisible = false;
                        await GetUserLocation();
                        simpleStackLayout.IsVisible = true;
                    });
                }
            };
        }

        #endregion

        #region segedfvk

        private async Task GetUserLocation()
        {
            activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            loadingLabel.Text = cimkek.GetLoading();

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync();

            homeImage.Source = "home.png";
            addImage.Source = "tadd.png";
            joinedImage.Source = "calendar.png";
            hashtagsImage.Source = "hashtags.png";
            settingsImage.Source = "settings.png";

            updateWelcome.Text = cimkek.GetCreateEvent();
            eventNameLabel.Text = cimkek.GetEventName() + cimkek.GetRequired();
            whenbeginLabel.Text = cimkek.GetEventFrom();
            notmatterhowlongLabel.Text = cimkek.GetNoMatter();
            whenendLabel.Text = cimkek.GetEventTo();
            onlineEventLabel.Text = cimkek.GetOnlineEvent();
            meetingLabel.Text = cimkek.GetPinOnTheMapTheMeetingPoint() + cimkek.GetRequired();
            eventLabel.Text = cimkek.GetPinOnTheMapThePlacePoint() + cimkek.GetRequired(); ;
            eventTownLabel.Text = cimkek.GetEventTown() + cimkek.GetRequired();
            eventPlaceLabel.Text = cimkek.GetEventPlace() + cimkek.GetRequired();
            enyoneLabel.Text = cimkek.GetEnyoneCanAttend();
            howmanyPersonLabel.Text = cimkek.GetHowManyPersonCanAttend() + cimkek.GetRequired();
            submitButton.Text = cimkek.GetCreateEvent();

            whenbegindatePicker.Format = "D";
            beginclock.Time = new TimeSpan(17, 0, 0);
            enddatePicker.Format = "D";
            endclock.Time = new TimeSpan(17, 0, 0);

            var today = DateTime.Now;

            var tomorrow = today.AddDays(60);

            whenbegindatePicker.MinimumDate = today;
            whenbegindatePicker.MaximumDate = tomorrow;

            enddatePicker.MinimumDate = today;
            enddatePicker.MaximumDate = tomorrow;

            map = new CustomMap()
            {
                WidthRequest = 320,
                HeightRequest = 200,
                IsShowingUser = true,
                MapType = MapType.Street,
                longitude = position.Longitude,
                latitude = position.Latitude,
                kind = "event"
            };

            map2 = new CustomMap()
            {
                WidthRequest = 320,
                HeightRequest = 200,
                IsShowingUser = true,
                MapType = MapType.Street,
                longitude = position.Longitude,
                latitude = position.Latitude,
                kind = "meeting"
            };

            map2.CustomPins = new List<CustomPin> { };

            map.CustomPins = new List<CustomPin> { };

            meetStack.Children.Add(map);

            eventStack.Children.Add(map2);
            
            map.isJustShow = false;
            map2.isJustShow = false;

            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
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

        #endregion

        #region toggled

        private void NomatterHowLong_Toggled(object sender, ToggledEventArgs e)
        {
            whenendLabel.IsVisible = !whenendLabel.IsVisible;
            enddatePicker.IsVisible = !enddatePicker.IsVisible;
            endclock.IsVisible = !endclock.IsVisible;

            noMatterHowLong = !noMatterHowLong;
        }

        private void OnlineEventSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            meetingLabel.IsVisible = !meetingLabel.IsVisible;
            map.IsVisible = !map.IsVisible;
            eventLabel.IsVisible = !eventLabel.IsVisible;
            map2.IsVisible = !map2.IsVisible;
            eventTownLabel.IsVisible = !eventTownLabel.IsVisible;
            eventTownEntry.IsVisible = !eventTownEntry.IsVisible;
            eventPlaceLabel.IsVisible = !eventPlaceLabel.IsVisible;
            eventPlaceEntry.IsVisible = !eventPlaceEntry.IsVisible;

            onlineEvent = !onlineEvent;
        }

        private void EnyoneSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            howmanyPersonLabel.IsVisible = !howmanyPersonLabel.IsVisible;
            howmanyPersonEntry.IsVisible = !howmanyPersonEntry.IsVisible;

            noMatterHowManyPerson = !noMatterHowManyPerson;
        }

        #endregion

        #region clicks

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;

            simpleStackLayout.IsVisible = false;

            fillExeption = false;
            convertint = false;
            begindateproblem = false;
            enddateproblem = false;
            fileproblems = false;
            begindatenotbiggerthentheend = false;

            Events eventCreateObject = new Events();

            eventCreateObject.FROM = (whenbegindatePicker.Date + beginclock.Time).ToUniversalTime().ToString("dddd, MMM dd yyyy HH:mm zzz");

            DateTimeOffset dto_begin = (whenbegindatePicker.Date + beginclock.Time);

            DateTimeOffset dto_end = new DateTimeOffset();

            if (dto_begin.ToLocalTime() < DateTimeOffset.Now.ToLocalTime()) { begindateproblem = true; }

            if (String.IsNullOrEmpty(eventNameEntry.Text))
            {
                fillExeption = true;
            }
            else
            {
                eventCreateObject.EVENTNAME = eventNameEntry.Text;
            }

            if (noMatterHowLong) eventCreateObject.TO = eventCreateObject.FROM;
            else if (dto_begin > enddatePicker.Date + endclock.Time) begindatenotbiggerthentheend = true;
            else
            {
                eventCreateObject.TO = (enddatePicker.Date + endclock.Time).ToUniversalTime().ToString("dddd, MMM dd yyyy HH:mm zzz");

                dto_end = (enddatePicker.Date + endclock.Time);

                if (dto_end.ToLocalTime() < DateTimeOffset.Now.ToLocalTime()) enddateproblem = true;
            }

            if (onlineEvent)
            {
                eventCreateObject.PLACECORD = "";
                eventCreateObject.MEETINGCORD = "";
                eventCreateObject.TOWN = "";
                eventCreateObject.PLACE = "";
                eventCreateObject.ONLINE = 1;
            }
            else if (String.IsNullOrEmpty(eventTownEntry.Text) || String.IsNullOrEmpty(eventPlaceEntry.Text))
            {
                fillExeption = true;
            }
            else
            {
                try
                {
                    eventCreateObject.PLACECORD = DependencyService.Get<IFileStoreAndLoad>().LoadText("eventcord.txt");
                    eventCreateObject.MEETINGCORD = DependencyService.Get<IFileStoreAndLoad>().LoadText("meetcord.txt");
                }
                catch (Exception)
                {
                    fileproblems = true;
                }
                if (eventCreateObject.PLACECORD == "" || eventCreateObject.MEETINGCORD == "")
                {
                    fillExeption = true;
                }

                eventCreateObject.TOWN = eventTownEntry.Text;
                eventCreateObject.PLACE = eventPlaceEntry.Text;
                eventCreateObject.ONLINE = 0;
            }


            if (noMatterHowManyPerson) { eventCreateObject.HOWMANY = 1; }
            else if (String.IsNullOrEmpty(howmanyPersonEntry.Text)) { fillExeption = true; }
            else
            {
                try
                {
                    eventCreateObject.HOWMANY = Convert.ToInt32(howmanyPersonEntry.Text);
                }
                catch (Exception)
                {
                    convertint = true;
                }
            }
            if (fillExeption)
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                simpleStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetFillEverything(), cimkek.GetOK());
            }
            else if (convertint)
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                simpleStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetHowmanyIsANumber(), cimkek.GetOK());
            }
            else if (begindatenotbiggerthentheend)
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                simpleStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetBiggerEndDateThenTheBeginDate(), cimkek.GetOK());
            }
            else if (fileproblems)
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                simpleStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
            }
            else if (begindateproblem)
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                simpleStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetBiggerBeginDateThenTheCurrentDate(), cimkek.GetOK());
            }
            else if (enddateproblem)
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                simpleStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetBiggerEndDateThenTheCurrentDate(), cimkek.GetOK());
            }
            else
            {
                eventCreateObject.MDESCRIPTION = "";

                int success = -1;

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        success = DependencyService.Get<IiOSDatabaseAccess>().InsertEvent(eventCreateObject);
                        break;
                    case Device.Android:
                        success = DependencyService.Get<IAndroidDatabaseAccess>().InsertEventAsync(eventCreateObject);
                        break;
                    case Device.WinPhone:
                        success = await DependencyService.Get<IDatabaseAccess>().InsertEventAsync(eventCreateObject);
                        break;
                    case Device.Windows:
                        success = await DependencyService.Get<IDatabaseAccess>().InsertEventAsync(eventCreateObject);
                        break;
                    default:
                        indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                        simpleStackLayout.IsVisible = true;

                        await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                        break;
                }

                if (success == -1)
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                    simpleStackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                }
                else
                {
                    Attended attend = new Attended();
                    attend.user_id = user.id;
                    attend.event_id = success;

                    bool successq = false;

                    switch (Device.RuntimePlatform)
                    {
                        case Device.iOS:
                            successq = DependencyService.Get<IiOSDatabaseAccess>().InsertAttended(attend);
                            break;
                        case Device.Android:
                            successq = DependencyService.Get<IAndroidDatabaseAccess>().InsertAttended(attend);
                            break;
                        case Device.WinPhone:
                            successq = await DependencyService.Get<IDatabaseAccess>().InsertAttendedAsync(attend);
                            break;
                        case Device.Windows:
                            successq = await DependencyService.Get<IDatabaseAccess>().InsertAttendedAsync(attend);
                            break;
                        default:
                            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                            simpleStackLayout.IsVisible = true;

                            await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                            break;
                    }

                    if (successq)
                    {
                        await DisplayAlert(cimkek.GetSuccess(), cimkek.GetSuccessfulCreatedTheEvent(), cimkek.GetOK());

                        DBContext dBContext = new DBContext();
                        dBContext.SendEmail("eventcreate", user.EMAIL, user.FIRSTNAME, user.LASTNAME, eventCreateObject.EVENTNAME, eventCreateObject.FROM, eventCreateObject.TO, eventCreateObject.TOWN, eventCreateObject.PLACE);

                        ObservableCollection<HashtagsM> hashtags = new ObservableCollection<HashtagsM>();

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
                                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                                simpleStackLayout.IsVisible = true;

                                await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                                break;
                        }

                        ObservableCollection<User> usersHowsOkay = new ObservableCollection<User>();

                        foreach (var item in hashtags)
                        {
                            if (item.UID != user.id)
                            {
                                if (
                                    (!String.IsNullOrEmpty(item.TOWN.ToLower()) &&
                                    item.TOWN.ToLower().Contains(item.TOWN.ToLower())
                                    ) ||
                                    (!String.IsNullOrEmpty(item.HASHTAG.ToLower()) &&
                                    (eventCreateObject.EVENTNAME.ToLower().Contains(item.HASHTAG.ToLower()) ||
                                    eventCreateObject.DESCRIPTION.ToLower().Contains(item.HASHTAG.ToLower())))
                                    )
                                {
                                    User userId = new User();

                                    switch (Device.RuntimePlatform)
                                    {
                                        case Device.iOS:
                                            userId = DependencyService.Get<IiOSDatabaseAccess>().GetUserByID(item.UID);
                                            break;
                                        case Device.Android:
                                            userId = DependencyService.Get<IAndroidDatabaseAccess>().GetUserByID(item.UID);
                                            break;
                                        case Device.WinPhone:
                                            userId = await DependencyService.Get<IDatabaseAccess>().GetUserByID(item.UID);
                                            break;
                                        case Device.Windows:
                                            userId = await DependencyService.Get<IDatabaseAccess>().GetUserByID(item.UID);
                                            break;
                                        default:
                                            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                                            simpleStackLayout.IsVisible = true;

                                            await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                                            break;
                                    }

                                    bool wasThat = false;

                                    foreach (var useritem in usersHowsOkay)
                                    {
                                        if (useritem.id == userId.id)
                                        {
                                            wasThat = true;
                                            break;
                                        }
                                    }

                                    if (!wasThat)
                                    {
                                        usersHowsOkay.Add(userId);
                                    }
                                }
                            }
                        }

                        foreach (var item in usersHowsOkay)
                        {
                            if (item.id != user.id)
                            {
                                dBContext.SendEmail("sendmailtosubscribeduser", item.EMAIL, item.FIRSTNAME, item.LASTNAME, eventCreateObject.EVENTNAME, eventCreateObject.FROM, eventCreateObject.TO, eventCreateObject.TOWN, eventCreateObject.PLACE);
                            }
                        }

                        await Navigation.PushModalAsync(new MainPage(user));
                    }
                    else
                    {
                        indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                        simpleStackLayout.IsVisible = true;

                        await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    }
                }
            }
        }

        #endregion
    }
}
