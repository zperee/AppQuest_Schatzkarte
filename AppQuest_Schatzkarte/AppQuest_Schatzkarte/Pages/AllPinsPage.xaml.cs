using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppQuest_Schatzkarte.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppQuest_Schatzkarte
{
	public partial class AllPinsPage : ContentPage
	{
		private readonly Map pins;
		ObservableCollection<AllPinsViewModel> data;
		List<TreasurePin> list;

		public AllPinsPage()
		{
			InitializeComponent();

		}

		public AllPinsPage(List<TreasurePin> list)
		{
			InitializeComponent();

			data = new ObservableCollection<AllPinsViewModel>();
			InitializeComponent();
			lstView.ItemsSource = data;

			foreach (var pin in list)
			{
				data.Add(new AllPinsViewModel() { lable = pin.Label, coordinates = pin.Latitude + " " + pin.Longitude});
			}
		}

		public async void OnDeleteItem(object sender, EventArgs e)
		{
		}
	}
}
