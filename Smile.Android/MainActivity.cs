using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using System.IO;
using PCLStorage;
using System.Collections.Generic;

namespace Smile.Droid {
    [Activity(Label = "Smile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        protected override void OnCreate(Bundle bundle) {
            
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Copyfiles();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults) {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void Copyfiles(){

            var localPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

			using (var asset = Assets.Open("detection1.jpg"))
			using (var dest = File.Create(Path.Combine(localPath, "detection1.jpg")))
                asset.CopyTo(dest);

			using (var asset = Assets.Open("detection2.jpg"))
			using (var dest = File.Create(Path.Combine(localPath, "detection2.jpg")))
                asset.CopyTo(dest);

			using (var asset = Assets.Open("detection3.jpg"))
			using (var dest = File.Create(Path.Combine(localPath, "detection3.jpg")))
                asset.CopyTo(dest);

			using (var asset = Assets.Open("detection4.jpg"))
			using (var dest = File.Create(Path.Combine(localPath, "detection4.jpg")))
                asset.CopyTo(dest);

			using (var asset = Assets.Open("identification1.jpg"))
			using (var dest = File.Create(Path.Combine(localPath, "identification1.jpg")))
                asset.CopyTo(dest);

			using (var asset = Assets.Open("identification2.jpg"))
			using (var dest = File.Create(Path.Combine(localPath, "identification2.jpg")))
                asset.CopyTo(dest);
        }
    }
}

