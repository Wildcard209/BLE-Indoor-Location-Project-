namespace MobileUserClient.Platforms.Android.PlatformPermissions
{
    public class BluetoothPermission : Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string androidPermission, bool isRuntime)>
            {
                ("android.permission.BLUETOOTH_SCAN", true),
                ("android.permission.BLUETOOTH_CONNECT", true),
                ("android.permission.BLUETOOTH_ADVERTISE", true),
                ("android.permission.BLUETOOTH", true),
                ("android.permission.BLUETOOTH_ADMIN", true),
                ("android.permission.ACCESS_FINE_LOCATION", true),
                ("android.permission.ACCESS_COARSE_LOCATION", true)
            }.ToArray();
    }
}
