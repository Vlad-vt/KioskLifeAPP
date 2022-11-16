namespace Kiosk_Life.Scanner.Zebra
{
    public interface IZebraCoreDefinitions
    {
        /// <summary>
        /// Maximum number of scanners to be connected
        /// </summary>
        const int MAX_NUM_DEVICES = 255;

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
        const int CLAIM_DEVICE = 1500;
        #endregion

        #region Scanners Subscribes
        const int SUBSCRIBE_BARCODE = 1;
        const int SUBSCRIBE_IMAGE = 2;
        const int SUBSCRIBE_VIDEO = 4;
        const int SUBSCRIBE_RMD = 8;
        const int SUBSCRIBE_PNP = 16;
        const int SUBSCRIBE_OTHER = 32;
        #endregion

        #region Scanners
        public const string SCANNER_SNAPI = "SNAPI";
        public const string SCANNER_SSI = "SSI";
        public const string SCANNER_NIXMODB = "NIXMODB";
        public const string SCANNER_IBMHID = "USBIBMHID";
        public const string SCANNER_IBMTT = "USBIBMTT";
        public const string SCALE_IBM = "USBIBMSCALE";
        public const string SCANNER_SSI_BT = "SSI_BT";
        public const string SCANNER_OPOS = "USBOPOS";
        public const string SCANNER_HIDKB = "USBHIDKB";
        public const string CAMERA_UVC = "UVC_CAMERA";
        #endregion

        #region Scanners Tags
        public const string TAG_SCANNER = "scanner";
        public const string TAG_SCANNER_SNAPI = SCANNER_SNAPI;
        public const string TAG_SCANNER_SSI = SCANNER_SSI;
        public const string TAG_SCANNER_NIXMODB = SCANNER_NIXMODB;
        public const string TAG_SCANNER_IBMHID = SCANNER_IBMHID;
        public const string TAG_SCANNER_OPOS = SCANNER_OPOS;
        public const string TAG_SCANNER_HIDKB = SCANNER_HIDKB;
        public const string TAG_SCANNER_IMBTT = SCANNER_IBMTT;
        public const string TAG_SCALE_IBM = SCALE_IBM;
        public const string TAG_SCANNER_SSI_BT = SCANNER_SSI_BT;
        public const string TAG_SCANNER_ID = "scannerID";
        public const string TAG_SCANNER_TYPE = "type";
        public const string TAG_SCANNER_SERIALNUMBER = "serialnumber";
        public const string TAG_SCANNER_MODELNUMBER = "modelnumber";
        public const string TAG_SCANNER_GUID = "GUID";
        public const string TAG_SCANNER_PORT = "port";
        public const string TAG_SCANNER_VID = "VID";
        public const string TAG_SCANNER_PID = "PID";
        public const string TAG_SCANNER_DOM = "DoM";
        public const string TAG_SCANNER_FW = "firmware";
        #endregion
    }
}
