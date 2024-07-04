using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions;
using Plugin.BLE;
using Plugin.BLE.Abstractions.EventArgs;
using System.Runtime.InteropServices;
using Org.BouncyCastle.Asn1.Pkcs;
using Shared.Objects;
using MiNET.Utils;
#if ANDROID
using Java.Nio;
using Java.Util;
using MobileUserClient.Platforms.Android.PlatformPermissions;
#elif IOS
using MobileUserClient.Platforms.iOS.PlatformPermissions;
using Foundation;
#elif MACCATALYST
using MobileUserClient.Platforms.MacCatalyst.PlatformPermissions;
using Foundation;
#endif

namespace MobileUserClient.Background
{
    public class BackgroundBeaconCheck
    {
        private CancellationTokenSource _cancellationTokenSource;
        private readonly List<IDevice> _devices = new();
        private readonly List<IDevice> _deviceBlacklist = new();
        private readonly IAdapter _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
        private readonly WebView _webView;
        private readonly GetPostMapData _mapData;
        private Dictionary<int, int> _beaconDataFound;
        private (int, int) _lastLocation = (0, 0);

        private bool firstJs = true;

        public BackgroundBeaconCheck(WebView webView, GetPostMapData mapData)
        {
            _webView = webView;
            _mapData = mapData;
        }

        public async Task StartBackgroundTask()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _bluetoothAdapter.DeviceDiscovered += OnDeviceDiscovered;
            _bluetoothAdapter.DeviceConnectionLost += OnDeviceConnectionLost;

            _bluetoothAdapter.ScanTimeout = 1000;

            PermissionStatus status = await Permissions.CheckStatusAsync<BluetoothPermission>();

            while (status != PermissionStatus.Granted)
            {
                status = await Permissions.CheckStatusAsync<BluetoothPermission>();
            }

            await Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    List<Task> connectTasks = new();
                    await _bluetoothAdapter.StartScanningForDevicesAsync();

                    _beaconDataFound = new Dictionary<int, int>();
                    foreach (var device in _devices)
                    {

                        connectTasks.Add(ConnectToDeviceAsync(device));

                        var records = device.AdvertisementRecords;

                        List<byte> data = new();

                        foreach (var record in records)
                        {
                            foreach (var value in record.Data)
                            {
                                data.Add(value);
                            }
                        }
                    }

                    await Task.WhenAll(connectTasks);
                    UpdateLocationValue(_devices.Count.ToString());
                    await Task.Delay(10);
                }
            }, _cancellationTokenSource.Token);
        }

        private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs lostDevice)
        {
            if (lostDevice == null || string.IsNullOrEmpty(lostDevice.Device.Name) || !_devices.Contains(lostDevice.Device))
            {
                return;
            }

            _devices.Remove(lostDevice.Device);
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs foundDevice)
        {
            if (foundDevice == null || string.IsNullOrEmpty(foundDevice.Device.Name) || _deviceBlacklist.Contains(foundDevice.Device) || _devices.Contains(foundDevice.Device))
            {
                return;
            }

            if (foundDevice.Device.AdvertisementRecords.Count == 5)
            {
                var records = foundDevice.Device.AdvertisementRecords;

                List<byte> data = new();

                foreach (var record in records)
                {
                    foreach (var value in record.Data)
                    {
                        data.Add(value);
                    }
                }
                if (data.Count > 30)
                {
                    var uuidbytes = new List<byte>();

                    for (int i = 0; i < 16; i++)
                    {
                        uuidbytes.Add(data[5 + i]);
                    }

#if ANDROID
                    ByteBuffer byteBuffer = ByteBuffer.Wrap(uuidbytes.ToArray());
                    long high = byteBuffer.Long;
                    long low = byteBuffer.Long;

                    var id = new Java.Util.UUID(high, low);
#elif IOS || MACCATALYST
                    NSData uuidData = NSData.FromArray(uuidbytes.ToArray());

                    ulong high = BitConverter.ToUInt64(uuidbytes.ToArray(), 0);
                    ulong low = BitConverter.ToUInt64(uuidbytes.ToArray(), 8);

                    byte[] idBytes = new byte[16];
                    Array.Copy(uuidbytes.ToArray(), idBytes, 16);

                    var id = new NSUuid(NSData.FromArray(idBytes).ToArray());
#endif
                    if (id.ToString() == "1b04c6c8-38fb-4b98-a24c-a39927f242bc")
                    {
                        _devices.Add(foundDevice.Device);
                        return;
                    }

                }
            }
            _deviceBlacklist.Add(foundDevice.Device);
        }

        public void StopBackgroundTask()
        {
            _cancellationTokenSource?.Cancel();
        }

        private async Task ConnectToDeviceAsync(IDevice device)
        {
            try
            {
                Task connectTask = _bluetoothAdapter.ConnectToDeviceAsync(device);
                Task completedTask = await Task.WhenAny(connectTask, Task.Delay(3000));
                connectTask.Dispose();

                if (device.State == DeviceState.Connected)
                {
                    await device.UpdateRssiAsync();

                    List<byte> data = new();

                    foreach (var record in device.AdvertisementRecords)
                    {
                        foreach (var value in record.Data)
                        {
                            data.Add(value);
                        }
                    }

                    byte[] minorBytes = { data[24], data[23] };
                    int minor = BitConverter.ToInt16(minorBytes, 0);

                    _beaconDataFound.Add(minor, device.Rssi);
                }
                else
                {
                    _devices.Remove(device);
                }
            }
            catch
            {
                _devices.Remove(device);
            }
        }

        private void UpdateLocationValue(string value)
        {

            MainThread.BeginInvokeOnMainThread(() =>
            {
                string script = "";
                if(firstJs)
                {
                    _lastLocation = (_mapData.DefaultX, _mapData.DefaultY);
                    HttpClient httpClient = new HttpClient();
                    foreach (PopupContent popup in _mapData.PopupContentList)
                    {
                        try
                        {
                            Task<string> popupDataCall = httpClient.GetStringAsync($"https://universityapi.jessicawylde.co.uk/mobile/html?fileName={popup.ContentName}");
                            popupDataCall.Wait();
                            string popupData = popupDataCall.Result;
                            popupData = popupData.Replace("{apiUrl}", "https://universityapi.jessicawylde.co.uk/");
                            popupData = popupData.Replace("\n", "");
                            script = $"addPopup({popup.LocationY}, {popup.LocationX}, '<div style=\"overflow: auto; max-height: 500px;\">{popupData}</div>');";
                             _webView.EvaluateJavaScriptAsync(script);
                        }
                        catch
                        {
                            //do nothing
                        }
                    }
                    firstJs = false;
                }
                script = $"updateValue('{value}')";
                _webView.EvaluateJavaScriptAsync(script);;

                var xValue = new X_Value.ModelInput();
                var yValue = new Y_Value.ModelInput();

                foreach (var data in _beaconDataFound)
                {
                    switch (data.Key)
                    {
                        case 1:
                            xValue.B1 = data.Value;
                            yValue.B1 = data.Value;
                            break;
                        case 2:
                            xValue.B2 = data.Value;
                            yValue.B2 = data.Value;
                            break;
                        case 3:
                            xValue.B3 = data.Value;
                            yValue.B3 = data.Value;
                            break;
                        case 4:
                            xValue.B4 = data.Value;
                            yValue.B4 = data.Value;
                            break;
                        case 5:
                            xValue.B5 = data.Value;
                            yValue.B5 = data.Value;
                            break;
                        case 6:
                            xValue.B6 = data.Value;
                            yValue.B6 = data.Value;
                            break;
                        case 7:
                            xValue.B7 = data.Value;
                            yValue.B7 = data.Value;
                            break;
                        case 8:
                            xValue.B8 = data.Value;
                            yValue.B8 = data.Value;
                            break;
                        case 9:
                            xValue.B9 = data.Value;
                            yValue.B9 = data.Value;
                            break;
                        case 10:
                            xValue.B10 = data.Value;
                            yValue.B10 = data.Value;
                            break;
                        case 11:
                            xValue.B11 = data.Value;
                            yValue.B11 = data.Value;
                            break;
                        case 12:
                            xValue.B12 = data.Value;
                            yValue.B12 = data.Value;
                            break;
                        case 13:
                            xValue.B13 = data.Value;
                            yValue.B13 = data.Value;
                            break;
                        case 14:
                            xValue.B14 = data.Value;
                            yValue.B14 = data.Value;
                            break;
                        case 15:
                            xValue.B15 = data.Value;
                            yValue.B15 = data.Value;
                            break;
                        case 16:
                            xValue.B16 = data.Value;
                            yValue.B16 = data.Value;
                            break;
                        case 17:
                            xValue.B17 = data.Value;
                            yValue.B17 = data.Value;
                            break;
                        case 18:
                            xValue.B18 = data.Value;
                            yValue.B18 = data.Value;
                            break;
                        case 19:
                            xValue.B19 = data.Value;
                            yValue.B19 = data.Value;
                            break;
                    }
                }

                var resultX = X_Value.Predict(xValue);
                var resultY = Y_Value.Predict(yValue);

                (int, int) largeBottomLeft = (0, 0);
                (int, int) largeTopRight = (_mapData.ImageWidth, _mapData.ImageHeight);
                (int, int) smallBottomLeft = (0, 0);
                (int, int) smallTopRight = (_mapData.BoundX, _mapData.BoundY);
                (int, int) smallGridLocationBottomLeftOnLargeGrid = (_mapData.LowerX, _mapData.LowerY);
                (int, int) smallGridLocationTopRightOnLargeGrid = (_mapData.HigherX, _mapData.HigherY);

                (float, float) smallPoint = (resultX.LocationX, resultY.LocationY);

                (int, int) largePoint = MapToLargeGrid(smallPoint, smallBottomLeft, smallTopRight, smallGridLocationBottomLeftOnLargeGrid, smallGridLocationTopRightOnLargeGrid);

                if (Math.Abs(largePoint.Item1 - _lastLocation.Item1) <= 20 || Math.Abs(largePoint.Item2 - _lastLocation.Item2) <= 20)
                { }
                else
                {
                    _lastLocation = largePoint;
                    script = $"updateLocation('{largePoint.Item1}', '{largePoint.Item2}');";
                    _webView.EvaluateJavaScriptAsync(script);
                }
            });
        }

        public static (int, int) MapToLargeGrid((float, float) point, (int, int) smallBottomLeft, (int, int) smallTopRight, (int, int) largeBottomLeft, (int, int) largeTopRight)
        {
            float scaleX = (float)(largeTopRight.Item1 - largeBottomLeft.Item1) / (smallTopRight.Item1 - smallBottomLeft.Item1);
            float scaleY = (float)(largeTopRight.Item2 - largeBottomLeft.Item2) / (smallTopRight.Item2 - smallBottomLeft.Item2);

            int largeX = (int)Math.Round((point.Item1 - smallBottomLeft.Item1) * scaleX + largeBottomLeft.Item1);
            int largeY = (int)Math.Round((point.Item2 - smallBottomLeft.Item2) * scaleY + largeBottomLeft.Item2);

            return (largeX, largeY);
        }
    }
}