using System;
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
        public HomePage()
        {
            Content = new Map(MapSpan.FromCenterAndRadius(new Position(47.2236, 8.8180), Distance.FromMeters(500)));
            InitializeComponent();
        }

        public async void OnLocateClicked(object sender, EventArgs e)
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;          

            try
            {
               _position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
            }
            catch (Exception)
            {
                _position = null;
            }           

            if (_position == null)
            {
                await DisplayAlert("Fehler", "GPS nicht verfügbar", "OK");
                return;
            }
            await DisplayAlert("GPS found", "Longitude: " + _position.Longitude + " Latitude: " + _position.Latitude, "OK");
        }

        private void AddNew(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}