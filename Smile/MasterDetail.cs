using System;
using SmileFace;
using Xamarin.Forms;

namespace Smile
{
    public class MasterDetail : MasterDetailPage
    {
        public MasterDetail()
        {
            MasterBehavior = MasterBehavior.Split;

            var master = new MasterPage();

            Master = master;

            master.ItemSelected += ItemSelected;

            ItemSelected(this, typeof(RateMePage));
        }

        void ItemSelected(object sender, Type item) {
            if (item == typeof(MainPage)) {
                Application.Current.MainPage = new MainPage();
                return;
            }

            Detail = new NavigationPage(Activator.CreateInstance(item) as Page) {
                BarBackgroundColor = Color.FromHex("#16222A"),
                BarTextColor = Color.White
            };

            if (Device.Idiom == TargetIdiom.Phone)
                IsPresented = false;

        }
    }
}