using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppQuest_Schatzkarte.Droid
{
    [Activity(Label = "AppQuest_Schatzkarte", Icon = "@drawable/icon", MainLauncher = true,
         ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            DependencyService.Register<IGeolocator, GeolocatorImplementation>();

            Forms.Init(this, bundle);

            FormsMaps.Init(this, bundle);

            UserDialogs.Init(() => (Activity)Forms.Context);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}