using System;
using System.Collections.Generic;
using Smile.Models;
using SmileFace;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Smile {
    public partial class MasterPage : ContentPage {

        public event EventHandler<int> MasterItemSelected;
        public event EventHandler<Type> ItemSelected;

        public MasterPage() {
            InitializeComponent();
            listView.ItemsSource = MasterPage.Data();
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e) {
            var item = ((ListView)sender).SelectedItem as MasterPageItem;

            //if (typeof(DetailPage) == item.TargetType) {
            //    MasterItemSelected?.Invoke(this, 0);
            //} else if (typeof(RateMePage) == item.TargetType) {
            //    MasterItemSelected?.Invoke(this, 1);
            //} else if (typeof(MainPage) == item.TargetType) {
            //    MasterItemSelected?.Invoke(this, 2);
            //}

            ItemSelected?.Invoke(this, item.TargetType);
        }

        static List<MasterPageItem> Data(){
            return new List<MasterPageItem> {
                new MasterPageItem{
                    Title = "Details",
                    IconSource="",
                    TargetType= typeof(DetailPage)
                },
                new MasterPageItem {
                    Title = "Rate Me",
                    IconSource="",
                    TargetType= typeof(RateMePage)
                },
                new MasterPageItem{
                    Title = "Log Out",
                    IconSource="",
                    TargetType= typeof(MainPage)
                }
            };
        }
    }
}
