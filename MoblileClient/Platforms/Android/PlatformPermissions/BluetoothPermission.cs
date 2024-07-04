namespace MobileClient.Platforms.Android.PlatformPermissions
{
    internal class BluetoothPermission : Permissions.BasePlatformPermission
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
                ("android.permission.ACCESS_COARSE_LOCATION", true),
                ("android.permission.READ_EXTERNAL_STORAGE", true),
        ("android.permission.WRITE_EXTERNAL_STORAGE", true),
        ("android.permission.READ_INTERNAL_STORAGE", true),
            }.ToArray();
    }
}
