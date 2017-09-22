using InvMe.BLL.MapClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace InvMe_.UWP.MapRenderer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class XamarinMapOverlay : UserControl
    {
        public XamarinMapOverlay()
        {
            this.InitializeComponent();
        }
        readonly CustomPin _customPin;
        public XamarinMapOverlay(CustomPin pin)
        {
            InitializeComponent();
            _customPin = pin;
            SetupData();
        }

        void SetupData()
        {
            Label.Text = _customPin.Pin.Label;
            Address.Text = _customPin.Pin.Address;
        }

        private async void OnInfoButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(_customPin.Url));
        }
        
    }
}
