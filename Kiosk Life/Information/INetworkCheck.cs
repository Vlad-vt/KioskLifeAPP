using Kiosk_Life.Network;

namespace Kiosk_Life.Information
{
    internal interface INetworkCheck
    {
        public NetworkDevicedata NetworkData { get; set; }
        public void CheckDeviceConnection();
        public void NetworkConnection(string ip, bool status);
    }
}
