using System;
using System.Threading.Tasks;
using Foundation;
using LocalAuthentication;
using Smile.Services;
using UIKit;

namespace Smile.iOS.Services {
    public class BiometricsService : IBiometricService {

        LAContextReplyHandler replyHandler;
        string BiometryType = "";

        public string BiometricCheck(){
            var context = new LAContext();
            var buttonText = "";
            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out var authError1)) { // has Biometrics (Touch or Face)
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0)) {
                    context.LocalizedReason = "Authorize for access to secrets"; // iOS 11
                    BiometryType = context.BiometryType == LABiometryType.TouchId ? "Touch ID" : "Face ID";
                    buttonText = $"Login with {BiometryType}";
                } else {   // no FaceID before iOS 11
                    buttonText = $"Login with Touch ID";
                }
            } else if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out var authError2)) {
                buttonText = $"Login"; // with device PIN
                BiometryType = "Device PIN";
            } else {
                // Application might choose to implement a custom username/password
                buttonText = "Use unsecured";
                BiometryType = "none";
            }

            return BiometryType;
        }

        public Task<bool> AuthenticateMe(){
            var context = new LAContext();
            NSError AuthError;
            var localizedReason = new NSString("To access secrets");

            // because LocalAuthentication APIs have been extended over time, need to check iOS version before setting some properties
            context.LocalizedFallbackTitle = "Fallback"; // iOS 8

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0)) {
                context.LocalizedCancelTitle = "Cancel"; // iOS 10
            }
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0)) {
                context.LocalizedReason = "Authorize for access to secrets"; // iOS 11
                BiometryType = context.BiometryType == LABiometryType.TouchId ? "TouchID" : "FaceID";
            }

            var tcs = new TaskCompletionSource<bool>();

            //Use canEvaluatePolicy method to test if device is TouchID or FaceID enabled
            //Use the LocalAuthentication Policy DeviceOwnerAuthenticationWithBiometrics
            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError)) {
                Console.WriteLine("TouchID/FaceID available/enrolled");
                replyHandler = new LAContextReplyHandler((success, error) => {
                    //Make sure it runs on MainThread, not in Background
                    UIApplication.SharedApplication.InvokeOnMainThread(() => {
                        if (success) {
                            Console.WriteLine($"You logged in with {BiometryType}!");
                            tcs.SetResult(success);
                            //PerformSegue("AuthenticationSegue", this);
                        } else {
                            Console.WriteLine(error.LocalizedDescription);
                            //Show fallback mechanism here
                            tcs.SetResult(success);
                            tcs.SetException(new Exception(error.Description));
                        }
                    });

                });
                //Use evaluatePolicy to start authentication operation and show the UI as an Alert view
                //Use the LocalAuthentication Policy DeviceOwnerAuthenticationWithBiometrics
                context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, localizedReason, replyHandler);
            } else if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, out AuthError)) {
                Console.WriteLine("When TouchID/FaceID aren't available or enrolled, use the device PIN");
                replyHandler = new LAContextReplyHandler((success, error) => {
                    //Make sure it runs on MainThread, not in Background
                    UIApplication.SharedApplication.InvokeOnMainThread(() => {
                        if (success) {
                            Console.WriteLine($"You logged in with {BiometryType}!");
                            //PerformSegue("AuthenticationSegue", this);
                            tcs.SetResult(success);
                        } else {
                            Console.WriteLine(error.LocalizedDescription);
                            //Show fallback mechanism here
                            tcs.SetResult(success);
                            tcs.SetException(new Exception(error.Description));
                        }
                    });
                });
                //Use evaluatePolicy to start authentication operation and show the UI as an Alert view
                //Use the LocalAuthentication Policy DeviceOwnerAuthenticationWithBiometrics
                context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthentication, localizedReason, replyHandler);
            } else {
                // User hasn't configured a PIN or any biometric auth. 
                // App may implement its own login, or choose to allow open access
                tcs.SetResult(false);
            }

            return tcs.Task;
        }
    }
}
