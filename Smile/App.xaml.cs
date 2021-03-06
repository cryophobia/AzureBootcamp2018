﻿using System;
using Xamarin.Forms;

namespace Smile {
    public partial class App : Application {
        public App() {
            SmileFace.Initializer.Init();

            InitializeComponent();

            MainPage = new NavigationPage (new MainPage());
        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }
}
