using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Position = Xamarin.Forms.Maps.Position;

namespace AppQuest_Schatzkarte.Pages
{
    public partial class HomePage : ContentPage
    {
        private Plugin.Geolocator.Abstractions.Position _position;
        private bool _GPSAvailable;
        private Map _map;
        private bool _isBusy;
        public HomePage()
        {
            _map = new Map(MapSpan.FromCenterAndRadius(new Position(47.2236, 8.8180), Distance.FromMeters(500)));
            Content = _map;
            InitializeComponent();
        }

        public async void OnLocateClicked(object sender, EventArgs e)
        {
            GetCurrentLocation();
            if (_GPSAvailable)
            {
                var pin = new Pin
                {
                    Type = PinType.Generic,
                    Label = "",
                    Position = new Position(_position.Latitude, _position.Longitude)
                };
                _map.Pins.Add(pin);
            } else {
                await DisplayAlert("Fehler", "GPS nicht verfügbar", "OK");
            }          
        }

        private async void OnAddNewPinClicked(object sender, EventArgs e)
        {
            if (_position == null)
            {
                GetCurrentLocation();
                if (_GPSAvailable)
                {
                    var pin = new Pin
                    {
                        Type = PinType.SavedPin,
                        Label = "",
                        Position = new Position(_position.Latitude, _position.Longitude)
                    };
                    _map.Pins.Add(pin);
                    _position = null;
                } else {
                    await DisplayAlert("Fehler", "GPS nicht verfügbar", "OK");
                }
            } else {
                var pin = new Pin
                {
                    Type = PinType.SavedPin,
                    Label = "",
                    Position = new Position(_position.Latitude, _position.Longitude)
                };
                _map.Pins.Add(pin);
                _position = null;
            }  
        }

        private async void GetCurrentLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            try
            {
                _isBusy = true;
                _position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                _isBusy = false;
                _GPSAvailable = true;

                if (_position == null)
                {
                    _GPSAvailable = false;
                }
            }
            catch (Exception)
            {
                _position = null;
                _GPSAvailable = false;
            }
           
        }
    }
}