namespace Kiosk_Life.Network
{
    public struct NetworkDevicedata
    {
        public string IP { get; set; }
        public string MacAddress { get; set; }
        public string ManufactoryName { get; set; }
        public bool ConnectedToNetwork { get; set; }
        public NetworkDevicedata(NetworkDevicedata network, bool connectionStatus)
        {
            IP = network.IP;
            MacAddress = network.MacAddress;
            ManufactoryName = network.ManufactoryName;
            ConnectedToNetwork = connectionStatus;
        }
    }
}
