namespace Kiosk_Life.Scanner.Zebra
{
    public interface IZebraCoreCommands
    {
        public const short CAMERA_TYPES_UVC = 14;
        /// <summary>
        /// Total number of scanner types
        /// </summary>
        public const short TOTAL_SCANNER_TYPES = CAMERA_TYPES_UVC;
        void Connect();
        void Disconnect();
    }
}
