namespace Kiosk_Life.Scanner.Zebra
{
    public interface IZebraCoreDefinitions
    {
        #region Scanners Events
        const int NUM_SCANNER_EVENTS = 6;
        #endregion

        #region Scanners Statuses
        const int STATUS_SUCCESS = 0;
        const int STATUS_FALSE = 1;
        const int STATUS_LOCKED = 10;
        const int ERROR_CDC_SCANNERS_NOT_FOUND = 150;
        const int ERROR_UNABLE_TO_OPEN_CDC_COM_PORT = 151;
        #endregion

        #region Scannners Types
        const short SCANNER_TYPES_ALL = 1;
        const short SCANNER_TYPES_SNAPI = 2;
        const short SCANNER_TYPES_SSI = 3;
        const short SCANNER_TYPES_RSM = 4;
        const short SCANNER_TYPES_IMAGING = 5;
        const short SCANNER_TYPES_IBMHID = 6;
        const short SCANNER_TYPES_NIXMODB = 7;
        const short SCANNER_TYPES_HIDKB = 8;
        const short SCANNER_TYPES_IBMTT = 9;
        const short SCALE_TYPES_IBM = 10;
        const short SCALE_TYPES_SSI_BT = 11;
        const short CAMERA_TYPES_UVC = 14;
        #endregion

        #region Scanners Protocols
        const int REGISTER_FOR_EVENTS = 1001;
        #endregion

        #region Scanners Subscribes
        const int SUBSCRIBE_BARCODE = 1;
        const int SUBSCRIBE_IMAGE = 2;
        const int SUBSCRIBE_VIDEO = 4;
        const int SUBSCRIBE_RMD = 8;
        const int SUBSCRIBE_PNP = 16;
        const int SUBSCRIBE_OTHER = 32;
        #endregion
    }
}
