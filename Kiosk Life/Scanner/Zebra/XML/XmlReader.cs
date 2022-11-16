using System.Xml;

namespace Kiosk_Life.Scanner.Zebra.XML
{
    public class XmlReader
    {
        const string TAG_MAXCOUNT = "maxcount";
        const string TAG_PROGRESS = "progress";
        const string TAG_PNP = "pnp";

        public void ReadXmlString_GetScanners(string strXml, List<ZebraScanner> zebraScanners, int nTotal, out int nScannerCount)
        {
            nScannerCount = 0;
            if (1 > nTotal || String.IsNullOrEmpty(strXml))
            {
                return;
            }
            try
            {
                XmlTextReader xmlRead = new XmlTextReader(new StringReader(strXml));
                // Skip non-significant whitespace   
                xmlRead.WhitespaceHandling = WhitespaceHandling.Significant;

                string sElementName = "", sElmValue = "";
                //ZebraScanner scanr = null;
                int nIndex = 0;
                bool bScanner = false;
                while (xmlRead.Read())
                {
                    switch (xmlRead.NodeType)
                    {
                        case XmlNodeType.Element:
                            sElementName = xmlRead.Name;
                            if (IZebraCoreDefinitions.TAG_SCANNER == sElementName)
                            {
                                bScanner = false;
                            }

                            string strScannerType = xmlRead.GetAttribute(IZebraCoreDefinitions.TAG_SCANNER_TYPE);
                            if (xmlRead.HasAttributes && (
                                (IZebraCoreDefinitions.TAG_SCANNER_SNAPI == strScannerType) ||
                                (IZebraCoreDefinitions.TAG_SCANNER_SSI == strScannerType) ||
                                (IZebraCoreDefinitions.TAG_SCANNER_NIXMODB == strScannerType) ||
                                (IZebraCoreDefinitions.TAG_SCANNER_IBMHID == strScannerType) ||
                                (IZebraCoreDefinitions.TAG_SCANNER_OPOS == strScannerType) ||
                                (IZebraCoreDefinitions.TAG_SCANNER_IMBTT == strScannerType) ||
                                (IZebraCoreDefinitions.TAG_SCALE_IBM == strScannerType) ||
                                (IZebraCoreDefinitions.SCANNER_SSI_BT == strScannerType) ||
                                (IZebraCoreDefinitions.CAMERA_UVC == strScannerType) ||
                                (IZebraCoreDefinitions.TAG_SCANNER_HIDKB == strScannerType)))//n = xmlRead.AttributeCount;
                            {
                                if(zebraScanners.Count > nIndex)
                                {
                                    bScanner = true;
                                    if (zebraScanners[nIndex] != null)
                                    {
                                        zebraScanners[nIndex].ClearValues();
                                        zebraScanners[nIndex].SCANNERTYPE = strScannerType;
                                        nIndex++;
                                    }
                                    //scanr = zebraScanners[nIndex++];
                                    //if(scanr != null)
                                    //{
                                    //    scanr.ClearValues();
                                    //    nScannerCount++;
                                    //    scanr.SCANNERTYPE = strScannerType;
                                   // }
                                }
                            }
                            break;

                        case XmlNodeType.Text:
                            if (bScanner && (zebraScanners[nIndex - 1] != null))
                            {
                                sElmValue = xmlRead.Value;
                                switch (sElementName)
                                {
                                    case IZebraCoreDefinitions.TAG_SCANNER_ID:
                                        zebraScanners[nIndex - 1].SCANNERID = sElmValue;
                                        break;
                                    case IZebraCoreDefinitions.TAG_SCANNER_SERIALNUMBER:
                                        zebraScanners[nIndex - 1].SERIALNO = sElmValue;
                                        break;
                                    case IZebraCoreDefinitions.TAG_SCANNER_MODELNUMBER:
                                        zebraScanners[nIndex - 1].MODELNO = sElmValue;
                                        break;
                                    case IZebraCoreDefinitions.TAG_SCANNER_GUID:
                                        zebraScanners[nIndex - 1].GUID = sElmValue;
                                        break;
                                    case IZebraCoreDefinitions.TAG_SCANNER_PORT:
                                        zebraScanners[nIndex - 1].PORT = sElmValue;
                                        break;
                                    case IZebraCoreDefinitions.TAG_SCANNER_FW:
                                        zebraScanners[nIndex - 1].SCANNERFIRMWARE = sElmValue;
                                        break;
                                    case IZebraCoreDefinitions.TAG_SCANNER_DOM:
                                        zebraScanners[nIndex - 1].SCANNERMNFDATE = sElmValue;
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }
    }
}
