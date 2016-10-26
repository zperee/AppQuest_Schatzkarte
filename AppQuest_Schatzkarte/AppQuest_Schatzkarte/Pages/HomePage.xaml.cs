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
        private Map _map;
        private bool _sucess;
        public HomePage()
        {
            _map = new Map(MapSpan.FromCenterAndRadius(new Position(47.2236, 8.8180), Distance.FromMeters(500)));
            Content = _map;
            InitializeComponent();
        }

        public void OnLocateClicked(object sender, EventArgs e)
        {
            GetCurrentLocation();
            if (_sucess)
            {
                var pin = new Pin
                {
                    Type = PinType.Generic,
                    Label = "",
                    Position = new Position(_position.Latitude, _position.Longitude)
                };
                _map.Pins.Add(pin);
                _sucess = false;
            }            
        }

        private void OnAddNewPinClicked(object sender, EventArgs e)
        {
            if (_position == null)
            {
                GetCurrentLocation();
                if (_sucess)
                {
                    var pin = new Pin
                    {
                        Type = PinType.SavedPin,
                        Label = "",
                        Position = new Position(_position.Latitude, _position.Longitude)
                    };
                    _map.Pins.Add(pin);
                    _position = null;
                }
                _sucess = false;
            }
            else
            {
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
                _position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                _sucess = true;
            }
            catch (Exception)
            {
                _position = null;
                _sucess = false;
            }

            if (_position == null)
            {
                await DisplayAlert("Fehler", "GPS nicht verfügbar", "OK");
                
            }
            
        }
    }
}