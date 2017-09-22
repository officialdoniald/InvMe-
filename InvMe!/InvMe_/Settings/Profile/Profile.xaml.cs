using InvMe.BLL.AzureLibraries;
using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.FileToByte;
using InvMe.BLL.Labels.English;
using InvMe.BLL.Labels.ILabels;
using InvMe.DAL.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvMe_.Settings.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        #region attr

        private byte[] photo;

        private MediaFile mediaFile;

        User user;

        User newUser = new User();

        DBContext DB = new DBContext();

        ICimkek cimkek = new Cimkek();

        InvMeBlobStorage blobStorage = new InvMeBlobStorage();

        private Stream f;

        private string pathf = "";

        #endregion

        #region konstruktor

        public Profile(User user)
        {
            InitializeComponent();

            loadingLabel.Text = cimkek.GetLoading();
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;

            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                homeImage.Source = "home.png";
                addImage.Source = "add.png";
                joinedImage.Source = "calendar.png";
                hashtagsImage.Source = "hashtags.png";
                settingsImage.Source = "tsettings.png";

                #region placeholders

                this.user = user;

                newUser = user;

                firstnameEntry.Placeholder = user.FIRSTNAME;

                lastnameEntry.Placeholder = user.LASTNAME;

                emailEntry.Placeholder = user.EMAIL;

                bornPicker.Date = user.BORNDATE;

                imageFromDevice.Source = ImageSource.FromUri(new Uri(user.PROFILEPICTURE));

                #endregion

                #region cimkek

                updatepictureWelcome.Text = cimkek.GetUpdatePicture();

                updatepwWelcome.Text = cimkek.GetUpdatePassword();

                updateWelcome.Text = cimkek.GetUpdateProfile();

                updateProfileButton.Text = cimkek.GetUpdateProfile();

                bornLabel.Text = cimkek.GetBorn().ToLower();

                passwordLabel.Text = cimkek.GetNewPassword().ToLower();

                passwordagainLabel.Text = cimkek.GetPasswordAgain().ToLower();

                emailLabel.Text = cimkek.GetEmail().ToLower();

                firstnameLabel.Text = cimkek.GetFirstName().ToLower();

                lastnameLabel.Text = cimkek.GetLastName().ToLower();

                updatePasswordButton.Text = cimkek.GetUpdatePassword();

                originalpasswordLabel.Text = cimkek.GetPassword().ToLower();

                takePhoto.Text = cimkek.GetCamera();

                takeorselectaphotoLabel.Text = cimkek.GetSelectPictureFromAlbum().ToLower();

                selectPhoto.Text = cimkek.GetSelectPhoto();

                updatePictureButton.Text = cimkek.GetUpdatePicture();

                mainStackLayout.IsVisible = true;
                #endregion

                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                mainStackLayout.IsVisible = true;
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
                    mainStackLayout.IsVisible = true;
                    homeImage.Source = "home.png";
                    addImage.Source = "add.png";
                    joinedImage.Source = "calendar.png";
                    hashtagsImage.Source = "hashtags.png";
                    settingsImage.Source = "tsettings.png";

                    #region placeholders

                    this.user = user;

                    newUser = user;

                    firstnameEntry.Placeholder = user.FIRSTNAME;

                    lastnameEntry.Placeholder = user.LASTNAME;

                    emailEntry.Placeholder = user.EMAIL;

                    bornPicker.Date = user.BORNDATE;

                    imageFromDevice.Source = ImageSource.FromUri(new Uri(user.PROFILEPICTURE));

                    #endregion

                    #region cimkek

                    updatepictureWelcome.Text = cimkek.GetUpdatePicture();

                    updatepwWelcome.Text = cimkek.GetUpdatePassword();

                    updateWelcome.Text = cimkek.GetUpdateProfile();

                    updateProfileButton.Text = cimkek.GetUpdateProfile();

                    bornLabel.Text = cimkek.GetBorn().ToLower();

                    passwordLabel.Text = cimkek.GetNewPassword().ToLower();

                    passwordagainLabel.Text = cimkek.GetPasswordAgain().ToLower();

                    emailLabel.Text = cimkek.GetEmail().ToLower();

                    firstnameLabel.Text = cimkek.GetFirstName().ToLower();

                    lastnameLabel.Text = cimkek.GetLastName().ToLower();

                    updatePasswordButton.Text = cimkek.GetUpdatePassword();

                    originalpasswordLabel.Text = cimkek.GetPassword().ToLower();

                    takePhoto.Text = cimkek.GetCamera();

                    takeorselectaphotoLabel.Text = cimkek.GetSelectPictureFromAlbum().ToLower();

                    selectPhoto.Text = cimkek.GetSelectPhoto();

                    updatePictureButton.Text = cimkek.GetUpdatePicture();

                    #endregion

                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                    mainStackLayout.IsVisible = true;
                }
            };
        }

        #endregion
        
        #region clicks

        private async void updateProfileButton_Clicked(object sender, EventArgs e)
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;

            ObservableCollection<User> userList = new ObservableCollection<User>();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    userList = DependencyService.Get<IiOSDatabaseAccess>().GetUser();
                    break;
                case Device.Android:
                    userList = DependencyService.Get<IAndroidDatabaseAccess>().GetUser();
                    break;
                case Device.WinPhone:
                    userList = await DependencyService.Get<IDatabaseAccess>().GetUser();
                    break;
                case Device.Windows:
                    userList = await DependencyService.Get<IDatabaseAccess>().GetUser();
                    break;
                default:
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                    mainStackLayout.IsVisible = true;
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            bool findExistEmail = false;

            foreach (var item in userList)
            {
                if (item.EMAIL == newUser.EMAIL && item.EMAIL != user.EMAIL)
                {
                    findExistEmail = true;
                    break;
                }
            }

            if (findExistEmail)
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                mainStackLayout.IsVisible = true;
                await DisplayAlert(cimkek.GetWarning(), cimkek.GetEmailExists(), cimkek.GetOK());
            }
            else if (!DB.IsValidEmailAddress(newUser.EMAIL) && newUser.EMAIL != "")
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                mainStackLayout.IsVisible = true;
                await DisplayAlert(cimkek.GetWarning(), cimkek.GetInvalidEmail(), cimkek.GetOK());
            }
            else
            {
                bool successInsert = await DependencyService.Get<IDatabaseAccess>().UpdateUser(newUser.id, newUser);

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        successInsert = DependencyService.Get<IiOSDatabaseAccess>().UpdateUser(newUser.id, newUser);
                        break;
                    case Device.Android:
                        successInsert = DependencyService.Get<IAndroidDatabaseAccess>().UpdateUser(newUser.id, newUser);
                        break;
                    case Device.WinPhone:
                        successInsert = await DependencyService.Get<IDatabaseAccess>().UpdateUser(newUser.id, newUser);
                        break;
                    case Device.Windows:
                        successInsert = await DependencyService.Get<IDatabaseAccess>().UpdateUser(newUser.id, newUser);
                        break;
                    default:
                        indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                        mainStackLayout.IsVisible = true;
                        await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                        break;
                }

                if (successInsert)
                {
                    await Navigation.PushModalAsync(new NavigationPage(new MainPage(newUser)));

                    await DisplayAlert(cimkek.GetSuccess(), cimkek.GetSuccess(), cimkek.GetOK());
                }
                else
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                    mainStackLayout.IsVisible = true;
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                }
            }
        }

        private async void updatePasswordButton_Clicked(object sender, EventArgs e)
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;

            ObservableCollection<User> userList = new ObservableCollection<User>();

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    userList = DependencyService.Get<IiOSDatabaseAccess>().GetUser();
                    break;
                case Device.Android:
                    userList = DependencyService.Get<IAndroidDatabaseAccess>().GetUser();
                    break;
                case Device.WinPhone:
                    userList = await DependencyService.Get<IDatabaseAccess>().GetUser();
                    break;
                case Device.Windows:
                    userList = await DependencyService.Get<IDatabaseAccess>().GetUser();
                    break;
                default:
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                    mainStackLayout.IsVisible = true;
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                    break;
            }

            bool findPassword = false;

            foreach (var item in userList)
            {
                if (item.id == user.id)
                {
                    if (originalpasswordEntry.Text == item.PASSWORD) findPassword = true;
                    break;
                }

            }

            if (!findPassword)
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                mainStackLayout.IsVisible = true;
                await DisplayAlert(cimkek.GetWarning(), cimkek.GetWrongPassword(), cimkek.GetOK());
            }
            else if (passwordEntry.Text == passwordagainEntry.Text)
            {
                if (newUser.PASSWORD.Length >= 6 && newUser.PASSWORD.Length <= 17)
                {
                    bool successInsert = await DependencyService.Get<IDatabaseAccess>().UpdateUser(newUser.id, newUser);

                    switch (Device.RuntimePlatform)
                    {
                        case Device.iOS:
                            successInsert = DependencyService.Get<IiOSDatabaseAccess>().UpdateUser(newUser.id, newUser);
                            break;
                        case Device.Android:
                            successInsert = DependencyService.Get<IAndroidDatabaseAccess>().UpdateUser(newUser.id, newUser);
                            break;
                        case Device.WinPhone:
                            successInsert = await DependencyService.Get<IDatabaseAccess>().UpdateUser(newUser.id, newUser);
                            break;
                        case Device.Windows:
                            successInsert = await DependencyService.Get<IDatabaseAccess>().UpdateUser(newUser.id, newUser);
                            break;
                        default:
                            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                            mainStackLayout.IsVisible = true;
                            await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                            break;
                    }

                    if (successInsert)
                    {
                        await Navigation.PushModalAsync(new NavigationPage(new MainPage(newUser)));

                        await DisplayAlert(cimkek.GetSuccess(), cimkek.GetSuccess(), cimkek.GetOK());
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
                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetMakesurepass(), cimkek.GetOK());
                }
            }
            else
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                mainStackLayout.IsVisible = true;
                await DisplayAlert(cimkek.GetWarning(), cimkek.GetNotTheSamePasswords(), cimkek.GetOK());
            }
        }

        #region takeandselectphoto

        private async void takePhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable ||
                !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Warning", "No camera available, please try again later!", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                Name = "test.jpg"
            });

            if (file == null) return;

            photo = DependencyService.Get<IFileToByte>().ReadAllByteS(file.Path);

            f = file.GetStream();
            pathf = file.Path;

            imageFromDevice.Source = ImageSource.FromStream(() => file.GetStream());

        }

        private async void selectPhoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert(cimkek.GetWarning(), cimkek.GetNoPicking(), cimkek.GetOK());
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            mediaFile = file;

            if (file == null) return;

            photo = DependencyService.Get<IFileToByte>().ReadAllByteS(file.Path);

            f = file.GetStream();
            pathf = file.Path;

            imageFromDevice.Source = ImageSource.FromStream(() => file.GetStream());
        }

        private async void updatePictureButton_Clicked(object sender, EventArgs e)
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;
            mainStackLayout.IsVisible = false;

            if (photo == null || photo.Length == 0)
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                mainStackLayout.IsVisible = true;
                await DisplayAlert(cimkek.GetWarning(), cimkek.GetChoosePhoto(), cimkek.GetOK());
            }
            else
            {
                string uniqueBlobName = await blobStorage.UploadFileAsync(pathf, f);

                newUser.PROFILEPICTURE = "https://officialdoniald.blob.core.windows.net/invme/" + uniqueBlobName;

                try
                {
                    bool successInsert = false;

                    switch (Device.RuntimePlatform)
                    {
                        case Device.iOS:
                            successInsert = DependencyService.Get<IiOSDatabaseAccess>().UpdateUser(newUser.id, newUser);
                            break;
                        case Device.Android:
                            successInsert = DependencyService.Get<IAndroidDatabaseAccess>().UpdateUser(newUser.id, newUser);
                            break;
                        case Device.WinPhone:
                            successInsert = await DependencyService.Get<IDatabaseAccess>().UpdateUser(newUser.id, newUser);
                            break;
                        case Device.Windows:
                            successInsert = await DependencyService.Get<IDatabaseAccess>().UpdateUser(newUser.id, newUser);
                            break;
                        default:
                            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                            mainStackLayout.IsVisible = true;
                            await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                            break;
                    }

                    if (successInsert)
                    {
                        await DisplayAlert(cimkek.GetSuccess(), cimkek.GetYouHaceChangedyourPhoto(), cimkek.GetOK());

                        await Navigation.PushModalAsync(new NavigationPage(new MainPage(newUser)));
                    }
                }
                catch (Exception ex2)
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;
                    mainStackLayout.IsVisible = true;
                    await DisplayAlert(cimkek.GetWarning(), ex2.Message, cimkek.GetOK());
                }

            }
        }

        #endregion

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

        #region entryChanges

        private void nameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (firstnameEntry.Text == "")
            {
                newUser.FIRSTNAME = user.FIRSTNAME;
            }
            else
            {
                newUser.FIRSTNAME = e.NewTextValue;
            }
        }

        private void emailEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (emailEntry.Text == "")
            {
                newUser.EMAIL = user.EMAIL;
            }
            else
            {
                newUser.EMAIL = e.NewTextValue;
            }
        }

        private void passwordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passwordEntry.Text == "")
            {
                newUser.PASSWORD = user.PASSWORD;
            }
            else
            {
                newUser.PASSWORD = e.NewTextValue;
            }
        }

        private void lastnameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lastnameEntry.Text == "")
            {
                newUser.LASTNAME = user.LASTNAME;
            }
            else
            {
                newUser.LASTNAME = e.NewTextValue;
            }
        }

        #endregion
    }
}
