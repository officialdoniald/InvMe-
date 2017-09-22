﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using Xamarin;
using ImageCircle.Forms.Plugin.Droid;

namespace InvMe_.Droid
{
    [Activity(Label = "InvMe_", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            FormsMaps.Init(this, bundle);
            ImageCircleRenderer.Init();
            
            LoadApplication(new App());
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

