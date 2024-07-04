
namespace MobileUserClient.Platforms.iOS.PlatformPermissions
{
    internal class BluetoothPermission : Permissions.BasePlatformPermission
    {
        public static (string iosPermission, bool isRuntime)[] GetiOSPermissions()
        {
            return new List<(string iosPermission, bool isRuntime)>
            {
                ("NSBluetoothPeripheralUsageDescription", true),
                ("NSBluetoothAlwaysUsageDescription", true),
                ("NSBluetoothPeripheralUsageDescription", true),
                ("NSBluetoothAlwaysUsageDescription", true),
                ("NSBluetoothPeripheralUsageDescription", true),
                ("NSBluetoothAlwaysUsageDescription", true),
                ("NSLocationWhenInUseUsageDescription", true),
                ("NSLocationAlwaysUsageDescription", true),
            }.ToArray();
        }
    }
}