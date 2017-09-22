using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvMe_.NoInternet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoInternetPage : ContentPage
    {
        public NoInternetPage()
        {
            InitializeComponent();

            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Connectivity Changed", "IsConnected: " + Plugin.Connectivity.CrossConnectivity.Current.IsConnected, "OK");
                }
                else if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    await Navigation.PushModalAsync(new Login.Login());
                }
            };
        }

        public NoInternetPage(ContentPage page)
        {
            InitializeComponent();

            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Connectivity Changed", "IsConnected: " + Plugin.Connectivity.CrossConnectivity.Current.IsConnected, "OK");
                }
                else if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    await Navigation.PushModalAsync(page);
                }
            };
        }
    }
}