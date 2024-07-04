using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Nio;
using Java.Util;
using MobileClient.Platforms.Android.PlatformPermissions;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Shared.Objects;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using IAdapter = Plugin.BLE.Abstractions.Contracts.IAdapter;

namespace MobileClient.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<BeaconInfo> beacons;

        [ObservableProperty]
        string beaconX;

        [ObservableProperty]
        string beaconY;

        public MainViewModel()
        {
            beacons = new ObservableCollection<BeaconInfo>();
            Beacons = beacons;

            BeaconX = beaconX;
            BeaconY = beaconY;

            _ = RequestBluetooth();
        }

        static async Task RequestBluetooth()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<BluetoothPermission>();

            //Check if permissions are already permitted.
            if (status == PermissionStatus.Granted)
            {
                return;
            }

            //Check if rational should be shown, only used if user previously denied.
            if (Permissions.ShouldShowRationale<BluetoothPermission>())
            {
                await Shell.Current.DisplayAlert("Permission required", "Bluetooth permissions are required to run the app.", "Okay");
            }

            //Show pop up to request permissions
            status = await Permissions.RequestAsync<BluetoothPermission>();


            if (status == PermissionStatus.Granted) { }
            else
            {
                await Shell.Current.DisplayAlert("Permission required", "Bluetooth permissions are required to run the app.", "Okay");
            }
        }

        [RelayCommand]
        async Task GetAllBluetooth()
        {
            IAdapter bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
            List<IDevice> devices = new();
            bluetoothAdapter.DeviceDiscovered += (sender, foundDevice) =>
            {
                if (foundDevice == null || string.IsNullOrEmpty(foundDevice.Device.Name))
                { }
                else
                {
                    if (foundDevice.Device.AdvertisementRecords.Count == 5)
                    {
                        devices.Add(foundDevice.Device);
                    }
                }
            };

            bluetoothAdapter.DeviceConnectionLost -= (sender, lostDevice) =>
            {
                if (lostDevice == null || string.IsNullOrEmpty(lostDevice.Device.Name))
                { }
                else
                {
                    if (lostDevice.Device.AdvertisementRecords.Count == 5)
                    {
                        devices.Remove(lostDevice.Device);
                    }
                }
            };

            await bluetoothAdapter.StartScanningForDevicesAsync();
            for (int count = 0; count < 15; count++)
            {
                
                Beacons = new ObservableCollection<BeaconInfo>();
                PostBeaconX postBeaconX = new();
                PostBeaconY postBeaconY = new();
                bluetoothAdapter.ScanTimeout = 1000;
                await bluetoothAdapter.StartScanningForDevicesAsync();

                //foreach (var device in bluetoothAdapter.ConnectedDevices)
                //{
                //    devices.Add(device);
                //}

                foreach (var device in devices)
                {
                    try
                    {
                        //First check to see if device is already connected or not
                        if (device.State == DeviceState.Connected) { }
                        else
                        {
                            await bluetoothAdapter.ConnectToDeviceAsync(device);
                        }

                        //Second one is to skip rest of code if connection failed
                        if (device.State == DeviceState.Connected) { }
                        else
                        {
                            continue;
                        }
                        await device.UpdateRssiAsync();
                        var records = device.AdvertisementRecords;

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

                            ByteBuffer byteBuffer = ByteBuffer.Wrap(uuidbytes.ToArray());
                            long high = byteBuffer.Long;
                            long low = byteBuffer.Long;

                            var id = new UUID(high, low);

                            if (id.ToString() == "1b04c6c8-38fb-4b98-a24c-a39927f242bc")
                            {
                                var beacon = new BeaconInfo();
                                string macFull = device.Id.ToString()[Math.Max(0, device.Id.ToString().Length - 12)..].ToUpper();
                                macFull = Regex.Replace(macFull, ".{2}", "$0:");
                                macFull = macFull.Remove(macFull.Length - 1, 1);

                                beacon.Name = device.Name;
                                beacon.MacAdress = macFull;
                                beacon.UUID = id.ToString();

                                byte[] majorBytes = { data[22], data[21] };
                                beacon.Major = BitConverter.ToInt16(majorBytes, 0).ToString();

                                byte[] minorBytes = { data[24], data[23] };
                                int minor = BitConverter.ToInt16(minorBytes, 0);
                                beacon.Minor = minor.ToString();

                                byte rssiByte = data[25];
                                sbyte rssiSignedByte = unchecked((sbyte)rssiByte);
                                beacon.RSSI1M = rssiSignedByte.ToString();

                                beacon.RSSI = device.Rssi.ToString();

                                switch(minor)
                                {
                                    case 1:
                                        postBeaconX.b1 = device.Rssi;
                                        postBeaconY.b1 = device.Rssi;
                                        break;
                                    case 2:
                                        postBeaconX.b2 = device.Rssi;
                                        postBeaconY.b2 = device.Rssi;
                                        break;
                                    case 3:
                                        postBeaconX.b3 = device.Rssi;
                                        postBeaconY.b3 = device.Rssi;
                                        break;
                                    case 4:
                                        postBeaconX.b4 = device.Rssi;
                                        postBeaconY.b4 = device.Rssi;
                                        break;
                                    case 5:
                                        postBeaconX.b5 = device.Rssi;
                                        postBeaconY.b5 = device.Rssi;
                                        break;
                                    case 6:
                                        postBeaconX.b6 = device.Rssi;
                                        postBeaconY.b6 = device.Rssi;
                                        break;
                                    case 7:
                                        postBeaconX.b7 = device.Rssi;
                                        postBeaconY.b7 = device.Rssi;
                                        break;
                                    case 8:
                                        postBeaconX.b8 = device.Rssi;
                                        postBeaconY.b8 = device.Rssi;
                                        break;
                                    case 9:
                                        postBeaconX.b9 = device.Rssi;
                                        postBeaconY.b9 = device.Rssi;
                                        break;
                                    case 10:
                                        postBeaconX.b10 = device.Rssi;
                                        postBeaconY.b10 = device.Rssi;
                                        break;
                                    case 11:
                                        postBeaconX.b11 = device.Rssi;
                                        postBeaconY.b11 = device.Rssi;
                                        break;
                                    case 12:
                                        postBeaconX.b12 = device.Rssi;
                                        postBeaconY.b12 = device.Rssi;
                                        break;
                                    case 13:
                                        postBeaconX.b13 = device.Rssi;
                                        postBeaconY.b13 = device.Rssi;
                                        break;
                                    case 14:
                                        postBeaconX.b14 = device.Rssi;
                                        postBeaconY.b14 = device.Rssi;
                                        break;
                                    case 15:
                                        postBeaconX.b15 = device.Rssi;
                                        postBeaconY.b15 = device.Rssi;
                                        break;
                                    case 16:
                                        postBeaconX.b16 = device.Rssi;
                                        postBeaconY.b16 = device.Rssi;
                                        break;
                                    case 17:
                                        postBeaconX.b17 = device.Rssi;
                                        postBeaconY.b17 = device.Rssi;
                                        break;
                                    case 18:
                                        postBeaconX.b18 = device.Rssi;
                                        postBeaconY.b18 = device.Rssi;
                                        break;
                                    case 19:
                                        postBeaconX.b19 = device.Rssi;
                                        postBeaconY.b19 = device.Rssi;
                                        break;
                                }

                                //Beacons.Add(beacon);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //await bluetoothAdapter.DisconnectDeviceAsync(device);
                    }
                }

                int x = int.Parse(BeaconX);
                int y = int.Parse(BeaconY);

                postBeaconX.locationX = x;
                postBeaconY.locationY = y;

                HttpClient httpClient = new();

                string postBeaconXjson = JsonSerializer.Serialize<PostBeaconX>(postBeaconX);
                StringContent postBeaconXcontent = new(postBeaconXjson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("https://universityapi.jessicawylde.co.uk/Beacon/BeaconX", postBeaconXcontent);

                string postBeaconYjson = JsonSerializer.Serialize<PostBeaconY>(postBeaconY);
                StringContent postBeaconYcontent = new(postBeaconYjson, Encoding.UTF8, "application/json");
                response = await httpClient.PostAsync("https://universityapi.jessicawylde.co.uk/Beacon/BeaconY", postBeaconYcontent);
            }

        }
    }
}
