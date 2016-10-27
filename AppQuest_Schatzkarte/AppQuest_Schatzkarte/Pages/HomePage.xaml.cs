using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using AppQuest_Schatzkarte.Infrastructure;
using AppQuest_Schatzkarte.Model;
using AppQuest_Schatzkarte.Services;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Plugin.Geolocator.Abstractions.Position;

namespace AppQuest_Schatzkarte.Pages
{
    public partial class HomePage : ContentPage
    {
        private readonly Map _map;
        private readonly FileSaver _fileSaver;

        public HomePage()
        {
            _map =
                new Map(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(47.2236, 8.8180),
                    Distance.FromMeters(500)));            

            _fileSaver = new FileSaver("Data.json", "LocalData");

            Content = _map;
            InitializeComponent();
        }

        /// <summary>
        ///     Call the FillPinsAsync method
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if(_map.Pins.Count == 0)
                await FillPinsAsync();
        }

        /// <summary>
        ///     This method fills the IList Pin with the local data if they any objects stored
        /// </summary>
        public async Task FillPinsAsync()
        {
            var json = await _fileSaver.ReadContentFromLocalFileAsync();
            if (string.IsNullOrEmpty(json)) return;
            var list = JsonConvert.DeserializeObject<IEnumerable<TreasurePin>>(json);
            foreach (var item in list)
            {
                var pin = new Pin
                {
                    Type = PinType.Generic,
                    Label = item.Label,
                    Position = new Xamarin.Forms.Maps.Position(item.Latitude, item.Longitude)                    
                };
                pin.Clicked += Pin_Clicked;
                _map.Pins.Add(pin);
            }
                
        }

        private async void Pin_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Pin löschen?", "Wollen Sie den Pin wirklich löschen?", "Ja", "Nein"))
            {
                var pin = (Pin)sender;
                Device.BeginInvokeOnMainThread(() => _map.Pins.Remove(pin));
                await PrepareListForLocalFileAsync();
                _map.Pins.Clear();
                await FillPinsAsync();
            }            
        }

        /// <summary>
        ///     ActionEvent for the button Locate me
        ///     This method will locate you and set the blue standard pin on the map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnLocateClicked(object sender, EventArgs e)
        {
            var position = await GetCurrentLocationAsync();

            if (position != null)
            {
                _map.MoveToRegion(new MapSpan(_map.VisibleRegion.Center, position.Latitude, position.Longitude));
                _map.IsShowingUser = true;
            }
            else
            {
                Debug.WriteLine("Error #2: HomePage.xaml.cs: No GPS Signal avaiable");
                await DisplayAlert("Fehler", "GPS nicht verfügbar", "OK");
            }
        }

        /// <summary>
        ///     ActionEvent for the button Set Pin
        ///     This method will locate you if there isn't any gps data before. After you have been located you have to name the
        ///     pin and save it to the local data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAddNewPinClicked(object sender, EventArgs e)
        {
            var position = await GetCurrentLocationAsync();

            if (position != null)
            {
                UserDialogs.Instance.Prompt(new PromptConfig
                {
                    Title = "Pin Name",
                    InputType = InputType.Default,
                    OkText = "Erstellen",
                    CancelText = "Abbrechen",
                    OnAction = async result =>
                    {
                        if (!result.Ok) return;
                        var pin = new Pin
                        {
                            Type = PinType.Generic,
                            Position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude),
                            Label = result.Text
                        };
                        pin.Clicked += Pin_Clicked;
                        _map.Pins.Add(pin);
                        
                    }
                });                
            }
            else
            {
                await DisplayAlert("Fehler", "GPS nicht verfügbar", "OK");
            }
        }

        /// <summary>
        /// Prepare the JSON Format for the local file
        /// </summary>
        /// <returns>Method has no return value. Async method have to return a Label</returns>
        public async Task PrepareListForLocalFileAsync()
        {
            var list = new List<TreasurePin>();
            foreach (var pin in _map.Pins)
            {
                var treasurePin = new TreasurePin
                {
                    Label = pin.Label,
                    Latitude = pin.Position.Latitude,
                    Longitude = pin.Position.Longitude
                };
                list.Add(treasurePin);
            }
            await _fileSaver.SaveContentToLocalFileAsync(list);
        }

        /// <summary>
        /// Gets your actual position with an accuracy of 5m
        /// </summary>
        /// <returns>Method has no return value. Async method have to return a Label</returns>
        private async Task<Position> GetCurrentLocationAsync()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            try
            {
                return await locator.GetPositionAsync(5000);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void OnSyncButtonClicked(object sender, EventArgs e)
        {
            
            var list = new List<TreasurePoint>();
            foreach (var pin in _map.Pins)
            {
                var treasurePoint = new TreasurePoint
                {                    
                    Latitude = pin.Position.Latitude,
                    Longitude = pin.Position.Longitude
                };
                list.Add(treasurePoint);
            }
            
            var logBuch = DependencyService.Get<ILogBuchService>();

            var json = JsonConvert.SerializeObject(list);
            logBuch.OpenLogBuch("Schatzkarte", json, "points");
        }  

		private void OnAllPinsButtonClicked(object sender, EventArgs e)
		{
			var list = new List<TreasurePin>();
			foreach (var pin in _map.Pins)
			{
				var treasurePin = new TreasurePin
				{
					Label = pin.Label,
					Latitude = pin.Position.Latitude,
					Longitude = pin.Position.Longitude
				};
				list.Add(treasurePin);
			}

			Application.Current.MainPage = new NavigationPage(new AllPinsPage(list));
		}
    }
}