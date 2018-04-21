using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Smile {
    public partial class DetailPage : ContentPage {
        public DetailPage() {
            InitializeComponent();
        }

        async void Handle_Tapped(object sender, System.EventArgs e) {
            var image = (Image)sender;
            var source = image.Source as FileImageSource;
            var filename = source.File;

            await this.Navigation.PushAsync(new SmileFace.InfoPage(filename));
        }
    }
}
