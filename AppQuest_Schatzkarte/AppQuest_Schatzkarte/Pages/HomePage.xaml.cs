using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Position = Xamarin.Forms.Maps.Position;
using PCLStorage;
using Acr.UserDialogs;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppQuest_Schatzkarte.Pages
{
	public partial class HomePage : ContentPage
	{
		private const string Folder = "AppQuest_Schatzkarte";
		private const string File = "Data.json";
		private IFile _localFile;
		private IFolder _localFolder;
		private IFolder _rootFolder;
		private Plugin.Geolocator.Abstractions.Position _position;
		private bool _GPSAvailable;
		private Map _map;
		private bool _isBusy = false;

		public HomePage()
		{
			_map = new Map(MapSpan.FromCenterAndRadius(new Position(47.2236, 8.8180), Distance.FromMeters(500)));
			Content = _map;
			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			_rootFolder = FileSystem.Current.LocalStorage;
			_localFolder = await _rootFolder.CreateFolderAsync(Folder, CreationCollisionOption.OpenIfExists);
			_localFile = await _localFolder.CreateFileAsync(File, CreationCollisionOption.OpenIfExists);

			var result = "";
			result = await _localFile.ReadAllTextAsync();

			if (result.Length > 0)
			{
				FillPins(result);
			}
		}

		public void FillPins(string file)
		{
			var json = JsonConvert.DeserializeObject<IEnumerable<Pin>>(file);
			foreach (var item in json)
			{
				_map.Pins.Add(item);
			}
		}

		public async void OnLocateClicked(object sender, EventArgs e)
		{
			await GetCurrentLocation();
			if (_GPSAvailable)
			{
				_map.MoveToRegion(new MapSpan(_map.VisibleRegion.Center, _position.Latitude, _position.Longitude));
				_map.IsShowingUser = true;
			}
			else {
				await DisplayAlert("Fehler", "GPS nicht verfügbar", "OK");
			}
		}

		private async void OnAddNewPinClicked(object sender, EventArgs e)
		{
			if (_position == null)
			{
				await GetCurrentLocation();
			}

			if (_GPSAvailable)
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
						var pin = new Pin();
						pin.Type = PinType.Generic;
						pin.Position = new Position(_position.Latitude, _position.Longitude);
						pin.Label = result.Text;
						_map.Pins.Add(pin);
						await SaveFile();
						_position = null;
					}
				});


			}
			else {
				await DisplayAlert("Fehler", "GPS nicht verfügbar", "OK");
			}
		}

		private async Task SaveFile()
		{
			var json = JsonConvert.SerializeObject(_map.Pins);
			await _localFile.WriteAllTextAsync(json);
		}

		private async Task GetCurrentLocation()
		{
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 5;

			try
			{
				_isBusy = true;
				_position = await locator.GetPositionAsync(timeoutMilliseconds: 5000);
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

		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				_isBusy = value;
				OnPropertyChanged();
			}
		}
	}
}