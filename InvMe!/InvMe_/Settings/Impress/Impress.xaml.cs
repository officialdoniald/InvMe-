using InvMe.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvMe_.Settings.Impress
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Impress : ContentPage
    {
        User user = new User();

        public Impress(User user)
        {
            this.user = user;

            InitializeComponent();

            homeImage.Source = "home.png";
            addImage.Source = "add.png";
            joinedImage.Source = "calendar.png";
            hashtagsImage.Source = "hashtags.png";
            settingsImage.Source = "tsettings.png";
        }

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
    }
}
