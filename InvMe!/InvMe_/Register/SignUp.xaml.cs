using FileStoringWithDependency.IFileStoreAndLoad;
using InvMe.BLL.AzureLibraries;
using InvMe.BLL.DatabaseAccess;
using InvMe.BLL.Labels.English;
using InvMe.DAL.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvMe_.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        #region attr
        
        private MediaFile mediaFile;

        Cimkek cimkek = new Cimkek();

        DBContext DB = new DBContext();

        User user = new User();

        InvMeBlobStorage blobStorage = new InvMeBlobStorage();

        private Stream f;

        private string pathf = "";

        string uniqueBlobName = "";

        #endregion

        #region konstruktor

        public SignUp()
        {
            InitializeComponent();

            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                #region cimkek

                loadingLabel.Text = cimkek.GetLoading();
                signupWelcome.Text = cimkek.GetSignUp();
                firstnameLabel.Text = cimkek.GetFirstName().ToLower();
                lastnameLabel.Text = cimkek.GetLastName().ToLower();
                emailLabel.Text = cimkek.GetEmail().ToLower();
                passwordLabel.Text = cimkek.GetPassword().ToLower();
                passwordagainLabel.Text = cimkek.GetPasswordAgain().ToLower();
                bornLabel.Text = cimkek.GetBorn().ToLower();
                takePhoto.Text = cimkek.GetCamera();
                takeorselectaphotoLabel.Text = cimkek.GetSelectPictureFromAlbum().ToLower();
                sigupButton.Text = cimkek.GetSignUp();
                selectPhoto.Text = cimkek.GetSelectPhoto();

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

                    loadingLabel.Text = cimkek.GetLoading();
                    mainStackLayout.IsVisible = true;
                    signupWelcome.Text = cimkek.GetSignUp();
                    firstnameLabel.Text = cimkek.GetFirstName().ToLower();
                    lastnameLabel.Text = cimkek.GetLastName().ToLower();
                    emailLabel.Text = cimkek.GetEmail().ToLower();
                    passwordLabel.Text = cimkek.GetPassword().ToLower();
                    passwordagainLabel.Text = cimkek.GetPasswordAgain().ToLower();
                    bornLabel.Text = cimkek.GetBorn().ToLower();
                    takePhoto.Text = cimkek.GetCamera();
                    takeorselectaphotoLabel.Text = cimkek.GetSelectPictureFromAlbum().ToLower();
                    sigupButton.Text = cimkek.GetSignUp();
                    selectPhoto.Text = cimkek.GetSelectPhoto();

                    #endregion
                }
            };
        }

        #endregion

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

            //photo = DependencyService.Get<IFileToByte>().ReadAllByteS(file.Path);

            f = file.GetStream();
            pathf = file.Path;

            imageFromDevice.Source = ImageSource.FromStream(() => file.GetStream());

        }

        private async void selectPhoto_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Warning", "No picking available, please try again later!", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            mediaFile = file;

            if (file == null) return;

            //photo = DependencyService.Get<IFileToByte>().ReadAllByteS(file.Path);

            f = file.GetStream();
            pathf = file.Path;

            imageFromDevice.Source = ImageSource.FromStream(() => file.GetStream());
        }

        #endregion

        #region clicks

        private async void sigupButton_Clicked(object sender, EventArgs e)
        {
            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = true;

            mainStackLayout.IsVisible = false;
            
            if (bornPicker.Date == null ||
                String.IsNullOrEmpty(firstnameEntry.Text) ||
                String.IsNullOrEmpty(emailEntry.Text) ||
                String.IsNullOrEmpty(passwordEntry.Text) ||
                String.IsNullOrEmpty(lastnameEntry.Text))
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                mainStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetFillEverything(), cimkek.GetOK());
            }
            else if (passwordEntry.Text != passwordagainEntry.Text)
            {
                indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                mainStackLayout.IsVisible = true;

                await DisplayAlert(cimkek.GetWarning(), cimkek.GetNotTheSamePasswords(), cimkek.GetOK());
            }
            else
            {
                user = new User()
                {
                    BORNDATE = bornPicker.Date,
                    FIRSTNAME = firstnameEntry.Text,
                    EMAIL = emailEntry.Text.ToLower(),
                    PASSWORD = passwordEntry.Text,
                    LASTNAME = lastnameEntry.Text
                };

                if (f != null)
                {
                    uniqueBlobName = await blobStorage.UploadFileAsync(pathf, f);

                    user.PROFILEPICTURE = "https://officialdoniald.blob.core.windows.net/invme/" + uniqueBlobName;
                }
                else
                {
                    user.PROFILEPICTURE = "luser.png";
                }

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

                bool findExistEmail = false;

                foreach (var item in asd)
                {
                    if (item.EMAIL == user.EMAIL)
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
                else if (!DB.IsValidEmailAddress(user.EMAIL))
                {
                    indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                    mainStackLayout.IsVisible = true;

                    await DisplayAlert(cimkek.GetWarning(), cimkek.GetInvalidEmail(), cimkek.GetOK());
                }
                else if (user.PASSWORD.Length >= 6 && user.PASSWORD.Length <= 17)
                {
                    bool success = false;

                    switch (Device.RuntimePlatform)
                    {
                        case Device.iOS:
                            success = DependencyService.Get<IiOSDatabaseAccess>().InsertUser(user);
                            break;
                        case Device.Android:
                            success = DependencyService.Get<IAndroidDatabaseAccess>().InsertUser(user);
                            break;
                        case Device.WinPhone:
                            success = await DependencyService.Get<IDatabaseAccess>().InsertUserAsync(user);
                            break;
                        case Device.Windows:
                            success = await DependencyService.Get<IDatabaseAccess>().InsertUserAsync(user);
                            break;
                        default:
                            indicatorStackLayout.IsVisible = activityIndicator.IsVisible = activityIndicator.IsRunning = false;

                            mainStackLayout.IsVisible = true;

                            await DisplayAlert(cimkek.GetWarning(), cimkek.GetSomethingWentWrong(), cimkek.GetOK());
                            break;
                    }

                    if (success)
                    {
                        string name = string.Format("{0} {1}",firstnameEntry.Text,lastnameEntry.Text);

                        string url = String.Format("http://invme.eu/invmeapp/registration.php?emaill={0}&nev={1}", emailEntry.Text, name);
                        Uri uri = new Uri(url);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "GET";
                        WebResponse res = await request.GetResponseAsync();

                        await DisplayAlert(cimkek.GetSuccess(), cimkek.GetSuccess(), cimkek.GetOK());

                        DependencyService.Get<IFileStoreAndLoad>().SaveText("logint.txt",user.EMAIL);

                        await Navigation.PushModalAsync(new MainPage(user));
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
        }

        #endregion
    }
}
