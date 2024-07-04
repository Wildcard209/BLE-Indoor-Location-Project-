
#if ANDROID

using Android.Content.Res;
using AndroidX.Lifecycle;
using MobileUserClient.Platforms.Android.PlatformPermissions;
using Android.Renderscripts;

#elif IOS

using MobileUserClient.Platforms.iOS.PlatformPermissions;
using Foundation;

#elif MACCATALYST

using MobileUserClient.Platforms.MacCatalyst.PlatformPermissions;
using Foundation;
#endif

using MobileUserClient.Background;
using System.Reflection.Metadata;
using System.Net.Http.Json;
using Shared.Objects;
using static System.Net.WebRequestMethods;
using System.Net.Http;

namespace MobileUserClient
{
    public partial class MainPage : ContentPage
    {
        //public WebView MyWebViewPublic;

        private GetPostMapData _mapData;
        private readonly BackgroundBeaconCheck _backgroundBeaconCheck;

        public MainPage()
        {
            InitializeComponent();
            RequestPermissions().Wait();

            HttpClient httpClient = new HttpClient();
            Task<GetPostMapData> mapResult= httpClient.GetFromJsonAsync<GetPostMapData>($"https://universityapi.jessicawylde.co.uk/mobile/mapdata");
            mapResult.Wait();
            _mapData = mapResult.Result;

            _ = LoadHtmlContentFromFile("skeleton.html");
            _backgroundBeaconCheck = new BackgroundBeaconCheck(MyWebView, _mapData);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = _backgroundBeaconCheck.StartBackgroundTask();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _backgroundBeaconCheck.StopBackgroundTask();
        }
        private async Task LoadHtmlContentFromFile(string fileName)
        {
            try
            {
                string htmlContent = "";
                double screenHight = (DeviceDisplay.MainDisplayInfo.Height/3) - 40;

#if ANDROID
                AssetManager assets = Android.App.Application.Context.Assets;

                using (StreamReader streamReader = new(assets.Open($"Resources/{fileName}")))
                {
                    htmlContent = streamReader.ReadToEnd();
                }

#elif IOS
                NSBundle mainBundle = NSBundle.MainBundle;
                string filePath = mainBundle.PathForResource(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));

                using (StreamReader streamReader = new(filePath))
                {
                    htmlContent = streamReader.ReadToEnd();
                }
#endif
                htmlContent = htmlContent.Replace("{mapHight}", _mapData.ImageHeight.ToString());
                htmlContent = htmlContent.Replace("{mapWidth}", _mapData.ImageWidth.ToString());
                htmlContent = htmlContent.Replace("{defaultX}", _mapData.DefaultX.ToString());
                htmlContent = htmlContent.Replace("{defaultY}", _mapData.DefaultY.ToString());
                htmlContent = htmlContent.Replace("{jsName}", _mapData.SelectedJs.ToString());
                htmlContent = htmlContent.Replace("{cssName}", _mapData.SelectedCss.ToString());
                htmlContent = htmlContent.Replace("{hight}", screenHight.ToString());
                htmlContent = htmlContent.Replace("{apiUrl}", "https://universityapi.jessicawylde.co.uk");

                MyWebView.Source = new HtmlWebViewSource { Html = htmlContent };

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading HTML content: {ex.Message}");
            }
        }

        static async Task RequestPermissions()
        {

            PermissionStatus status = await Permissions.CheckStatusAsync<BluetoothPermission>();
            
            if (status == PermissionStatus.Granted)
            {
                return;
            }

            _ = Permissions.RequestAsync<BluetoothPermission>();
        }
    }
}