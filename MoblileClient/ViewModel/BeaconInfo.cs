
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MobileClient.ViewModel
{
    public class BeaconInfo
    {
        public string Name { get; set; }
        public string MacAdress { get; set; }
        public string UUID { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string RSSI1M { get; set; }
        public string RSSI { get; set; }
    }
}
