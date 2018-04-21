using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using PCLStorage;
using SkiaSharp;
using Xamarin.Forms;

namespace SmileFace {
    public partial class InfoPage : ContentPage {

        readonly IFaceServiceClient faceServiceClient;
        string filename;
        SKBitmap bitmapImage;

        public InfoPage(string filename) {
            InitializeComponent();

            this.filename = filename;
            faceServiceClient = new FaceServiceClient(SmileFace.Helpers.Constants.FaceApiKey, SmileFace.Helpers.Constants.FaceEndpoint);
        }

        void Handle_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e) {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            if (bitmapImage != null) {
                canvas.Clear();
                var canvasRect = info.Size;
                float x = (info.Width - bitmapImage.Width) / 2;
                float y = (info.Height / 3 - bitmapImage.Height) / 2;
                canvas.DrawBitmap(bitmapImage, 
                                  SKRect.Create(bitmapImage.Info.Size).AspectFit(SKRect.Create(canvasRect).Size), 
                                  SKRect.Create(canvasRect));
            }
        }

        protected async override void OnAppearing() {
            base.OnAppearing();

            //photoImage.Source = ImageSource.FromFile(filename);

            IFile inFile = await FileSystem.Current.LocalStorage.GetFileAsync(filename);

            using (SKManagedStream skStream = new SKManagedStream(await inFile.OpenAsync(FileAccess.Read))) {
                bitmapImage = SKBitmap.Decode(skStream);
            }

            Device.BeginInvokeOnMainThread(() => canvasView.InvalidateSurface());

            Stream photoStream = await inFile.OpenAsync(FileAccess.Read);

            // Recognize emotion
            try {
                var faceAttributes = new FaceAttributeType[] { FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Gender, FaceAttributeType.FacialHair, FaceAttributeType.Age };
                    //using (var photoStream = photo.GetStream()) {
                        Face[] faces = await faceServiceClient.DetectAsync(photoStream, true, false, faceAttributes);
                if (faces.Any()) {
                    infoLabel.Text = "";
                    // Emotions detected are happiness, sadness, surprise, anger, fear, contempt, disgust, or neutral.
                    foreach (var face in faces) {
                        var emotion = face.FaceAttributes.Emotion.ToRankedList().FirstOrDefault().Key;
                        var gender = face.FaceAttributes.Gender;
                        var glasses = face.FaceAttributes.Glasses;
                        var age = face.FaceAttributes.Age;
                        DrawFaceRectangle(face.FaceRectangle.Top, face.FaceRectangle.Top, face.FaceRectangle.Width, face.FaceRectangle.Height);
                        Debug.WriteLine($"Gender: {gender} - Emotion: {emotion} - Glasses: {glasses} - Age: {age}");
                        infoLabel.Text += $"Gender: {gender} - Emotion: {emotion} - Glasses: {glasses} - Age: {age} {Environment.NewLine}";
						infoLabel.Text += $"Face rect: (x:{face.FaceRectangle.Top}, y:{face.FaceRectangle.Top}, w:{face.FaceRectangle.Width}, h:{face.FaceRectangle.Height} {Environment.NewLine})";
                    }
                }
                photoStream.Dispose();
            } catch (FaceAPIException fx) {
                Debug.WriteLine(fx.Message);
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        private void DrawFaceRectangle(int x, int y, int width, int height) {
            SKPaint linePaint = new SKPaint {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Red,
                StrokeWidth = 5
            };
            SKCanvas canvas = new SKCanvas(bitmapImage);

            canvas.DrawLine(x, y, x + width, y, linePaint);
            canvas.DrawLine(x, y, x, y + height, linePaint);
            canvas.DrawLine(x + width, y + height, x + width, y, linePaint);
            canvas.DrawLine(x, y + height, x + width, y + height, linePaint);
            Device.BeginInvokeOnMainThread(() => canvasView.InvalidateSurface());
        }

    }
}
