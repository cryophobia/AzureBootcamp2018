using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using PCLStorage;
using Smile.iOS.Services;
using Smile.Services;
using UIKit;

namespace Smile.iOS {
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            global::Xamarin.Forms.DependencyService.Register<IBiometricService, BiometricsService>();
            CopyFiles();

            return base.FinishedLaunching(app, options);
        }

        void CopyFiles(){
            var localPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) +"/../Library");

            var pathToResoure = NSBundle.MainBundle.PathForResource("detection1", "jpg");
			File.Copy(pathToResoure, Path.Combine(localPath, "detection1.jpg"), true);
			pathToResoure = NSBundle.MainBundle.PathForResource("detection2", "jpg");
			File.Copy(pathToResoure, Path.Combine(localPath, "detection2.jpg"), true);
			pathToResoure = NSBundle.MainBundle.PathForResource("detection3", "jpg");
			File.Copy(pathToResoure, Path.Combine(localPath, "detection3.jpg"), true);
			pathToResoure = NSBundle.MainBundle.PathForResource("identification1", "jpg");
			File.Copy(pathToResoure, Path.Combine(localPath, "identification1.jpg"), true);
			pathToResoure = NSBundle.MainBundle.PathForResource("identification2", "jpg");
			File.Copy(pathToResoure, Path.Combine(localPath, "identification2.jpg"), true);
        }
    }
}
