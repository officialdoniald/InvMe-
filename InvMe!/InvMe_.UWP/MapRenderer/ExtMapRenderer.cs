using MyApp.UWP.CustomRenderers;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ExtMap), typeof(ExtMapRenderer))]
namespace MyApp.UWP.CustomRenderers
{
    /// <summary>
    /// Renderer for the xamarin map.
    /// Enable user to get a position by taping on the map.
    /// </summary>
    public class ExtMapRenderer : MapRenderer
    {
        // We use a native google map for Android
        private MapControl _map;

        public void OnMapReady(MapControl googleMap)
        {
            _map = googleMap;

            if (_map != null)
                _map.MapElementClick += googleMap_MapClick;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            if (_map != null)
                _map.MapElementClick -= googleMap_MapClick;

            base.OnElementChanged(e);

            //if (Control != null)
            //    ((MapView)Control).GetMapAsync(this);
        }

        private void googleMap_MapClick(object sender, MapElementClickEventArgs e)
        {
            ((ExtMap)Element).OnTap(new Position(e.Location.Position.Latitude, e.Location.Position.Longitude));
        }
    }
}
