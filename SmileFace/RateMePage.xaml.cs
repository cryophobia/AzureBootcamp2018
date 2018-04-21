using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace SmileFace {
    public partial class RateMePage : ContentPage {
        readonly IFaceServiceClient faceServiceClient;
        MediaFile photo;

        public RateMePage() {
            InitializeComponent();

            faceServiceClient = new FaceServiceClient(SmileFace.Helpers.Constants.FaceApiKey, SmileFace.Helpers.Constants.FaceEndpoint);
        }

        async void OnTakePhotoButtonClicked(object sender, EventArgs e) {
            await CrossMedia.Current.Initialize();

            // Take photo
            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported) {
                photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions {
                    Name = "emotion.jpg",
                    PhotoSize = PhotoSize.Small
                });

                if (photo != null) {
                    image.Source = ImageSource.FromStream(photo.GetStream);
                }
            } else {
                await DisplayAlert("No Camera", "Camera unavailable.", "OK");
            }

            ((Button)sender).IsEnabled = false;
            activityIndicator.IsRunning = true;

            // Recognize emotion
            try {
                if (photo != null) {

                    var faceAttributes = new FaceAttributeType[] { 
						FaceAttributeType.Emotion, 
						FaceAttributeType.Glasses, 
						FaceAttributeType.Gender, 
						FaceAttributeType.FacialHair };
                    using (var photoStream = photo.GetStream()) {
                        Face[] faces = await faceServiceClient.DetectAsync(photoStream, true, false, faceAttributes);
                        if (faces.Any()) {
                            // Emotions detected are happiness, sadness, surprise, anger, fear, contempt, disgust, or neutral.
                            Face face = faces.FirstOrDefault();

                            Device.BeginInvokeOnMainThread(() => {
                                emotionResultLabel.Text = face.FaceAttributes.Emotion.ToRankedList().FirstOrDefault().Key;
                            });
                        }
                        photo.Dispose();
                    }
                }
            } catch (FaceAPIException fx) {
                Debug.WriteLine(fx.Message);
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }

            activityIndicator.IsRunning = false;
            ((Button)sender).IsEnabled = true;
        }
    }
}