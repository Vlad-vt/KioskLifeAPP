namespace Kiosk_Life.Scanner.Zebra
{
    public class ZebraScanner : Scanner
    {
        public ZebraScanner()
        {

        }

        #region Private Members
        private int handle;
        private string scannerName;// now scannerName = scannerID
        private string scannerID;// a unique id
        private string scannerType;//SCANNER_SNAPI, SCANNER_SSI
        private string serialNo;
        private string modelNo;
        private string guid;
        private string port;
        private string firmware;
        private string mnfdate; //manufacture date
        private bool claimed;//scanner is claimed by this client-app
        private bool useHID; // Scanner is using HID channel for Binary Data transfer
        #endregion

        #region Public Getters and Setters
        public string SCANNERMNFDATE
        {
            get { return mnfdate; }
            set { mnfdate = value; }
        }
        public string SCANNERFIRMWARE
        {
            get { return firmware; }
            set { firmware = value; }
        }
        public string SCANNERNAME
        {
            get { return scannerName; }
            set { scannerName = value; }
        }
        public string SCANNERTYPE
        {
            get { return scannerType; }
            set { scannerType = value; }
        }
        public int HANDLE
        {
            get { return handle; }
            set { handle = value; }
        }
        public string SCANNERID
        {
            get { return scannerID; }
            set { scannerID = value; }
        }
        public string SERIALNO
        {
            get { return serialNo; }
            set { serialNo = value; }
        }
        public string MODELNO
        {
            get { return modelNo; }
            set { modelNo = value; }
        }
        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }
        public string PORT
        {
            get { return port; }
            set { port = value; }
        }
        public bool CLAIMED
        {
            get { return claimed; }
            set { claimed = value; }
        }
        public bool UseHID
        {
            get { return useHID; }
            set { useHID = value; }
        }
        #endregion
    }
}
