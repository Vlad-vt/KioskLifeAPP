using CoreScanner;
using System.Diagnostics;
using System.Management;

namespace Kiosk_Life.Scanner.Zebra
{
    public class ZebraCore : IZebraCoreCommands
    {
        public List<ZebraScanner> ZebraScanners;

        /// <summary>
        /// Default core for working with scanners
        /// </summary>
        private CCoreScannerClass _zebraCore;

        private short[] _zebraTypes;

        private short _zebraNumberOfTypes;

        private bool[] _zebraSelectedTypes;

        private bool _connectionOpened;

        private int _selectedScannerType;
        public ZebraCore()
        {
            ZebraScanners = new List<ZebraScanner>();
            try
            {
                _zebraCore = new CCoreScannerClass();
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.Message);
                Thread.Sleep(1000);
                _zebraCore = new CCoreScannerClass();
            }
            _selectedScannerType = 1;
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        private void Connect()
        {
            if (_connectionOpened)
            {
                return;
            }
            int appHandle = 0;
            GetSelectedScannerTypes();
            int status = IZebraCoreDefinitions.STATUS_FALSE;

            try
            {
                _zebraCore.Open(appHandle, _zebraTypes, _zebraNumberOfTypes, out status);
                DisplayResult(status, "OPEN");
                if (IZebraCoreDefinitions.STATUS_SUCCESS == status)
                {
                    _connectionOpened = true;
                }
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("Error OPEN - " + exp.Message);
            }
        }

        private void DisplayResult(int status, string strCmd)
        {
            switch (status)
            {
                case IZebraCoreDefinitions.STATUS_SUCCESS:
                    UpdateResults(strCmd + " - Command success.");
                    break;
                case IZebraCoreDefinitions.STATUS_LOCKED:
                    UpdateResults(strCmd + " - Command failed. Device is locked by another application.");
                    break;
                case IZebraCoreDefinitions.ERROR_CDC_SCANNERS_NOT_FOUND:
                    UpdateResults(strCmd + " - No CDC device found. - Error:" + status.ToString());
                    break;
                case IZebraCoreDefinitions.ERROR_UNABLE_TO_OPEN_CDC_COM_PORT:
                    UpdateResults(strCmd + " - Unable to open CDC port. - Error:" + status.ToString());
                    break;
                default:
                    UpdateResults(strCmd + " - Command failed. Error:" + status.ToString());
                    break;
            }
        }

        private void UpdateResults(string strOut)
        {
            Console.WriteLine(strOut);
        }

        private void FilterScannerList()
        {
            for (int index = 0; index < IZebraCoreCommands.TOTAL_SCANNER_TYPES; index++)
            {
                _zebraSelectedTypes[index] = false;
            }

            switch (_selectedScannerType)
            {
                case 0:
                    _zebraSelectedTypes[IZebraCoreDefinitions.SCANNER_TYPES_ALL - 1] = true;
                    break;
                case 1:
                    _zebraSelectedTypes[IZebraCoreDefinitions.SCANNER_TYPES_HIDKB - 1] = true;
                    break;
                case 2:
                    _zebraSelectedTypes[IZebraCoreDefinitions.SCANNER_TYPES_IBMHID - 1] = true;
                    break;
                case 3:
                    _zebraSelectedTypes[IZebraCoreDefinitions.SCANNER_TYPES_SNAPI - 1] = true;
                    break;
            }
        }

        private void GetSelectedScannerTypes()
        {
            _zebraNumberOfTypes = 0;
            for (int index = 0, k = 0; index < IZebraCoreCommands.TOTAL_SCANNER_TYPES; index++)
            {
                if (_zebraSelectedTypes[index])
                {
                    _zebraNumberOfTypes++;
                    switch (index + 1)
                    {
                        case IZebraCoreDefinitions.SCANNER_TYPES_ALL:
                            _zebraTypes[k++] = IZebraCoreDefinitions.SCANNER_TYPES_ALL;
                            return;

                        case IZebraCoreDefinitions.SCANNER_TYPES_SNAPI:
                            _zebraTypes[k++] = IZebraCoreDefinitions.SCANNER_TYPES_SNAPI;
                            break;

                        case IZebraCoreDefinitions.SCANNER_TYPES_SSI:
                            _zebraTypes[k++] = IZebraCoreDefinitions.SCANNER_TYPES_SSI;
                            break;

                        case IZebraCoreDefinitions.SCANNER_TYPES_NIXMODB:
                            _zebraTypes[k++] = IZebraCoreDefinitions.SCANNER_TYPES_NIXMODB;
                            break;

                        case IZebraCoreDefinitions.SCANNER_TYPES_RSM:
                            _zebraTypes[k++] = IZebraCoreDefinitions.SCANNER_TYPES_RSM;
                            break;

                        case IZebraCoreDefinitions.SCANNER_TYPES_IMAGING:
                            _zebraTypes[k++] = IZebraCoreDefinitions.SCANNER_TYPES_IMAGING;
                            break;

                        case IZebraCoreDefinitions.SCANNER_TYPES_IBMHID:
                            _zebraTypes[k++] = IZebraCoreDefinitions.SCANNER_TYPES_IBMHID;
                            break;

                        case IZebraCoreDefinitions.SCANNER_TYPES_HIDKB:
                            _zebraTypes[k++] = IZebraCoreDefinitions.SCANNER_TYPES_HIDKB;
                            break;

                        case IZebraCoreDefinitions.SCALE_TYPES_SSI_BT:
                            _zebraTypes[k++] = IZebraCoreDefinitions.SCALE_TYPES_SSI_BT;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private void registerForEvents()
        {
            if (_connectionOpened)
            {
                int nEvents = 0;
                string strEvtIDs = GetRegUnregIDs(out nEvents);
                string inXml = "<inArgs>" +
                                    "<cmdArgs>" +
                                    "<arg-int>" + nEvents + "</arg-int>" +
                                    "<arg-int>" + strEvtIDs + "</arg-int>" +
                                    "</cmdArgs>" +
                                    "</inArgs>";

                int opCode = IZebraCoreDefinitions.REGISTER_FOR_EVENTS;
                string outXml = "";
                int status = IZebraCoreDefinitions.STATUS_FALSE;
                ExecCmd(opCode, ref inXml, out outXml, out status);
                DisplayResult(status, "REGISTER_FOR_EVENTS");
            }
        }

        /// <summary>
        /// Sends ExecCommand(Sync) or ExecCommandAsync
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="inXml"></param>
        /// <param name="outXml"></param>
        /// <param name="status"></param>
        private void ExecCmd(int opCode, ref string inXml, out string outXml, out int status)
        {
            outXml = "";
            status = IZebraCoreDefinitions.STATUS_FALSE;
            if (_connectionOpened)
            {
                try
                {
                    _zebraCore.ExecCommand(opCode, ref inXml, out outXml, out status);
                }
                catch (Exception ex)
                {
                    DisplayResult(status, "EXEC_COMMAND");
                    UpdateResults("..." + ex.Message.ToString());
                }
            }
        }

        private string GetRegUnregIDs(out int nEvents)
        {
            string strIDs = "";
            nEvents = IZebraCoreDefinitions.NUM_SCANNER_EVENTS;
            strIDs = IZebraCoreDefinitions.SUBSCRIBE_BARCODE.ToString();
            strIDs += "," + IZebraCoreDefinitions.SUBSCRIBE_IMAGE.ToString();
            strIDs += "," + IZebraCoreDefinitions.SUBSCRIBE_VIDEO.ToString();
            strIDs += "," + IZebraCoreDefinitions.SUBSCRIBE_RMD.ToString();
            strIDs += "," + IZebraCoreDefinitions.SUBSCRIBE_PNP.ToString();
            strIDs += "," + IZebraCoreDefinitions.SUBSCRIBE_OTHER.ToString();
            return strIDs;
        }

        /// <summary>
        /// Calls GetScanners command
        /// </summary>
        private void ShowScanners()
        {
            int opCode = CLAIM_DEVICE;
            string inXml = String.Empty;
            string outXml = "";
            int status = STATUS_FALSE;
            lstvScanners.Items.Clear();
            combSlcrScnr.Items.Clear();

            m_arScanners.Initialize();
            if (m_bSuccessOpen)
            {
                m_nTotalScanners = 0;
                short numOfScanners = 0;
                int nScannerCount = 0;
                string outXML = "";
                int[] scannerIdList = new int[MAX_NUM_DEVICES];
                try
                {
                    m_pCoreScanner.GetScanners(out numOfScanners, scannerIdList, out outXML, out status);
                    DisplayResult(status, "GET_SCANNERS");
                    if (STATUS_SUCCESS == status)
                    {
                        m_nTotalScanners = numOfScanners;
                        m_xml.ReadXmlString_GetScanners(outXML, m_arScanners, numOfScanners, out nScannerCount);
                        for (int index = 0; index < m_arScanners.Length; index++)
                        {
                            for (int i = 0; i < claimlist.Count; i++)
                            {
                                if (string.Compare(claimlist[i], m_arScanners[index].SERIALNO) == 0)
                                {
                                    Scanner objScanner = (Scanner)m_arScanners.GetValue(index);
                                    objScanner.CLAIMED = true;
                                }
                            }
                        }

                        FillScannerList();
                        UpdateOutXml(outXML);
                        for (int index = 0; index < m_nTotalScanners; index++)
                        {
                            Scanner objScanner = (Scanner)m_arScanners.GetValue(index);
                            string[] strItems = new string[] { "", "", "", "", "" };

                            inXml = "<inArgs><scannerID>" + objScanner.SCANNERID + "</scannerID></inArgs>";

                            for (int i = 0; i < claimlist.Count; i++)
                            {
                                if (string.Compare(claimlist[i], objScanner.SERIALNO) == 0)
                                {
                                    ExecCmd(opCode, ref inXml, out outXml, out status);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error GETSCANNERS - " + ex.Message, APP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void IZebraCoreCommands.Connect()
        {
            throw new NotImplementedException();
        }
    }
}
