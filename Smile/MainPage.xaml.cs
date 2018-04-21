using Smile.Services;
using SmileFace;
using Xamarin.Forms;

namespace Smile {
    public partial class MainPage : ContentPage {
        private IBiometricService biometricsService;

        public MainPage() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            biometricsService = DependencyService.Resolve<IBiometricService>();

            if (biometricsService != null)
                BiometricButton.Text = biometricsService.BiometricCheck();
        }

        void Handle_Clicked(object sender, System.EventArgs e) {
            //this.Navigation.PushAsync(new RateMePage());
            Application.Current.MainPage = new MasterDetail();
        }

        async void Handle_FaceAuth(object sender, System.EventArgs e) {
            if (biometricsService == null)
                biometricsService = DependencyService.Resolve<IBiometricService>();
            
            if (biometricsService != null){
                if (await biometricsService.AuthenticateMe()) {

                    //await this.Navigation.PushAsync(new RateMePage());
                    Application.Current.MainPage = new MasterDetail();
                }
            }
        }
    }
}
