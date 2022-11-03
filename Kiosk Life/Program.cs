using Kiosk_Life.Printer;
using Kiosk_Life.Terminal;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Management;
using System.Printing;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

///
/// TCP Example
///
using System.Net.NetworkInformation;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Kiosk_Life.Network;
using Kiosk_Life.Kiosk;
using Kiosk_Life.Camera;
using Kiosk_Life.Information;
using Newtonsoft.Json;
using Kiosk_Life.Dispenser;

namespace Kiosk
{

    public class KioskLife
    {

        private static HttpListener _httpListener = new HttpListener();
        public static void Main(string[] args)
        {
            _httpListener.Prefixes.Add("http://localhost:7000/kiosklife/");
            _httpListener.Prefixes.Add("http://localhost:7000/dispensersHealth/");
            _httpListener.Start();
            Thread serverThread = new Thread(ResponseThread);
            serverThread.IsBackground = true;
            serverThread.Start();
            Thread monitorThread = new Thread(() =>
            {
                int count;
                List<Printer> printers = new List<Printer>();
                LocalPrintServer server = new LocalPrintServer();
                Console.WriteLine("Analyzing network...");
                Terminal terminal = new Terminal(true);
                Camera camera = new Camera(true);
                Console.Clear();
                Console.WriteLine("Start working!");
                while (true)
                {
                    count = 0;
                    for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                    {
                        if (PrinterSettings.InstalledPrinters[i].Contains("Boca"))
                        {
                            PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                            printers.Add(new NetworkPrinter(printQueue.Name, PrinterSettings.InstalledPrinters[i].ToString()));
                        }
                        if (PrinterSettings.InstalledPrinters[i].Contains(server.DefaultPrintQueue.Name))
                        {
                            PrintQueue printQueue = server.GetPrintQueue(PrinterSettings.InstalledPrinters[i].ToString());
                            printers.Add(new USBPrinter(printQueue.Name, PrinterSettings.InstalledPrinters[i].ToString()));
                        }
                    }
                    Console.Clear();
                    do
                    {
                        foreach (Printer printer in printers)
                        {
                            if (printer.GetType() == typeof(USBPrinter))
                            {
                                PrintQueue printQueue = server.GetPrintQueue(printer.FullName);
                                if (printQueue.IsPrinting)
                                    printer.PrinterProcess = "Printing";
                                else if (printQueue.IsPaused)
                                    printer.PrinterProcess = "Paused";
                                else if (printQueue.IsBusy)
                                    printer.PrinterProcess = "Busy";
                                else
                                    printer.PrinterProcess = "Working";
                                (printer as USBPrinter).CheckForErrors(printQueue);
                            }
                            else if (printer.GetType() == typeof(NetworkPrinter))
                            {
                                (printer as NetworkPrinter).CheckDeviceConnection();
                                (printer as NetworkPrinter).ConnectToDevice();
                                (printer as NetworkPrinter).readData();
                            }
                        }
                        terminal.CheckForErrors("");
                        terminal.CheckDeviceConnection();
                        KioskConfiguration.GetInstance()._printers = printers;
                        KioskConfiguration.GetInstance()._terminal = terminal;
                        KioskConfiguration.GetInstance()._camera = camera;
                        KioskConfiguration.GetInstance().TestSend();
                        count++;
                        Thread.Sleep(1000);
                        Console.Clear();
                    } while (count != 10000);
                    printers.Clear();
                    server.Refresh();
                    GC.Collect();
                    Thread.Sleep(5000);
                }
            });
            monitorThread.Start();
            Console.ReadLine();
        }

        private static void ResponseThread()
        {
            while (true)
            {
                HttpListenerContext context = _httpListener.GetContext(); // get a context
                context.Response.ContentType = "text/plain, charset=UTF-8";
                context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                context.Response.ContentEncoding = Encoding.UTF8;
                switch (context.Request.RawUrl)
                {
                    case "/kiosklife":
                        KioskConfiguration.GetInstance().SendFileToBrowser(ref context);
                        break;
                    case "/dispensersHealth/":
                        using (var reader = new StreamReader(context.Request.InputStream))
                        {
                            DispensersHealth dispensersHealth = JsonConvert.DeserializeObject<DispensersHealth>(reader.ReadToEnd());
                            KioskConfiguration.GetInstance()._dispensers = dispensersHealth.Dispensers;
                        }
                        break;
                }
                context.Response.KeepAlive = false;
                context.Response.Close();
            }
        }
    }
}





