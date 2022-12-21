using Newtonsoft.Json;

namespace KioskLife.MVVM.Model.Scanner.Zebra
{
    public class ZebraScanner
    {
        public ZebraScanner()
        {
            Error = "";
        }

        public void ClearValues()
        {
            CLAIMED = false;
            useHID = false;
            SCANNERNAME = "";
            SCANNERID = "";
            SERIALNO = "";
            MODELNO = "";
            GUID = "";
            SCANNERTYPE = "";
            SCANNERMNFDATE = "";
            SCANNERFIRMWARE = "";
        }

        public void ShowAllInfo()
        {
            System.Diagnostics.Trace.WriteLine("Scanner ID: " + SCANNERID + "\n" +
                                "Model #: " + MODELNO + "\n" +
                                "Scanner serial number: " + SERIALNO + "\n" +
                                "Scanner firmware: " + SCANNERFIRMWARE + "\n" +
                                "Scanner guid: " + GUID + "\n");
        }

        #region Private Members
        private string error;
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
        [JsonProperty("Error")]
        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        [JsonIgnore]
        public string SCANNERMNFDATE
        {
            get { return mnfdate; }
            set { mnfdate = value; }
        }
        [JsonIgnore]
        public string SCANNERFIRMWARE
        {
            get { return firmware; }
            set { firmware = value; }
        }
        [JsonIgnore]
        public string SCANNERNAME
        {
            get { return scannerName; }
            set { scannerName = value; }
        }
        [JsonProperty("Scanner ID")]
        public string SCANNERID
        {
            get { return scannerID; }
            set { scannerID = value; }
        }
        [JsonProperty("Scanner Type")]
        public string SCANNERTYPE
        {
            get { return scannerType; }
            set { scannerType = value; }
        }
        [JsonIgnore]
        public int HANDLE
        {
            get { return handle; }
            set { handle = value; }
        }
        [JsonProperty("Serial number")]
        public string SERIALNO
        {
            get { return serialNo; }
            set { serialNo = value; }
        }
        [JsonIgnore]
        public string MODELNO
        {
            get { return modelNo; }
            set { modelNo = value; }
        }
        [JsonIgnore]
        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }
        [JsonIgnore]
        public string PORT
        {
            get { return port; }
            set { port = value; }
        }
        [JsonIgnore]
        public bool CLAIMED
        {
            get { return claimed; }
            set { claimed = value; }
        }
        [JsonIgnore]
        public bool UseHID
        {
            get { return useHID; }
            set { useHID = value; }
        }
        #endregion
    }
}
