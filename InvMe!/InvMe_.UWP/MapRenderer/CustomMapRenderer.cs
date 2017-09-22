using FileStoringWithDependency.UWP.FileStoreAndLoad;
using InvMe.BLL.MapClasses;
using InvMe_.UWP.MapRenderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace InvMe_.UWP.MapRenderer
{
    public class CustomMapRenderer : Xamarin.Forms.Maps.UWP.MapRenderer
    {
        private MapControl _nativeMap;
        private List<CustomPin> _customPins;
        private XamarinMapOverlay _mapOverlay;
        private bool tappedOnTheElement = false;
        FileStoreAndLoad fileFunctions = new FileStoreAndLoad();

        string filename = "";

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //_nativeMap.MapElementClick -= OnMapElementClick;
                _nativeMap.Children.Clear();
                _mapOverlay = null;
                _nativeMap = null;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                _nativeMap = Control as MapControl;
                _customPins = formsMap.CustomPins;

                if (formsMap.kind == "event")
                {
                    filename = "eventcord.txt";
                }
                else
                {
                    filename = "meetcord.txt";
                }

                if (!formsMap.isJustShow) { _nativeMap.MapTapped += _nativeMap_MapTapped; }

                _nativeMap.Children.Clear();
                //_nativeMap.MapElementClick += OnMapElementClick;
                
                foreach (var pin in _customPins)
                {
                    var snPosition = new BasicGeoposition { Latitude = pin.Pin.Position.Latitude, Longitude = pin.Pin.Position.Longitude };
                    var snPoint = new Geopoint(snPosition);

                    var mapIcon = new MapIcon
                    {
                        Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///MapRenderer/pin.png")),
                        CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible,
                        Location = snPoint,
                        NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0)
                    };

                    _nativeMap.MapElements.Add(mapIcon);
                }
            }
        }

        private void _nativeMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            if (!tappedOnTheElement)
            {
                var position = new Position(args.Location.Position.Latitude, args.Location.Position.Longitude);

                var pin = new CustomPin
                {
                    Pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = position,
                        Label = "ASD",
                        Address = "ASD"
                    },
                    Url = "www.google.hu"
                };

                _customPins = new List<CustomPin>();
                _customPins.Add(pin);
                var snPosition = new BasicGeoposition { Latitude = pin.Pin.Position.Latitude, Longitude = pin.Pin.Position.Longitude };

                FileStoreAndLoad sad = new FileStoreAndLoad();
                
                sad.SaveText(filename, pin.Pin.Position.Latitude + ";" + pin.Pin.Position.Longitude + "");

                var snPoint = new Geopoint(snPosition);

                var mapIcon = new MapIcon
                {
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///MapRenderer/pin.png")),
                    CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible,
                    Location = snPoint,
                    NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0)
                };
                _nativeMap.MapElements.Clear();
                _nativeMap.MapElements.Add(mapIcon);
            }
            else
            {
                tappedOnTheElement = false;
            }
        }

        private void OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            tappedOnTheElement = true;
            var mapIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;

            if (mapIcon != null)
            {
                var customPin = GetCustomPin(mapIcon.Location.Position);

                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                _mapOverlay = new XamarinMapOverlay(customPin);

                var xamarinMapOverlays = _nativeMap.Children.Where(o => o is XamarinMapOverlay);

                // Remove shown XamarinMapOverlay
                foreach (var xamarinMapOverlay in xamarinMapOverlays)
                {
                    _nativeMap.Children.Remove(xamarinMapOverlay);
                }

                var snPosition = new BasicGeoposition { Latitude = customPin.Pin.Position.Latitude, Longitude = customPin.Pin.Position.Longitude };
                var snPoint = new Geopoint(snPosition);

                _nativeMap.Children.Add(_mapOverlay);
                MapControl.SetLocation(_mapOverlay, snPoint);
                MapControl.SetNormalizedAnchorPoint(_mapOverlay, new Windows.Foundation.Point(0.5, 1.0));
            }
        }

        private CustomPin GetCustomPin(BasicGeoposition position)
        {
            var pos = new Position(position.Latitude, position.Longitude);
            foreach (var pin in _customPins)
            {
                if (pin.Pin.Position == pos)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}

