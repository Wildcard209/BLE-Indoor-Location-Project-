namespace Shared.Objects
{
    public class PostBeacon
    {
        private string name = "";
        private string macAddress = "";
        private string uuid = "";
        private string majour = "";
        private string minor = "";
        private string rssi1m = "";
        private string rssi = "";

        public string Name { get => name; set => name = value; }
        public string MacAddress { get => macAddress; set => macAddress = value; }
        public string UUID { get => uuid; set => uuid = value; }
        public string Major { get => majour; set => majour = value; }
        public string Minor { get => minor; set => minor = value; }
        public string RSSI1M { get => rssi1m; set => rssi1m = value; }
        public string RSSI { get => rssi; set => rssi = value; }
        public float LocationX { get; set; }
        public float LocationY { get; set; }
    }
}
