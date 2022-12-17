namespace KioskLife.Network
{
    public struct NetworkDeviceData
    {
        public string IP { get; set; }
        public string MacAddress { get; set; }
        public string ManufactoryName { get; set; }
        public bool ConnectedToNetwork { get; set; }
        public NetworkDeviceData(NetworkDeviceData network, bool connectionStatus)
        {
            IP = network.IP;
            MacAddress = network.MacAddress;
            ManufactoryName = network.ManufactoryName;
            ConnectedToNetwork = connectionStatus;
        }
    }
}
