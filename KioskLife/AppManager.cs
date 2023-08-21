using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskLife
{
    internal class AppManager
    {
        private string filePath;

        private ReloadData reloadData;

        private class ReloadData
        {
            [JsonProperty("First time program reload")]
            public bool FirstReload { get; set; }

            [JsonProperty("Second time program reload")]
            public bool LastReload { get; set; }

        }

        public AppManager()
        {
            var folderPath = AppDomain.CurrentDomain.BaseDirectory + @"\ReloadsLog\";
            Directory.CreateDirectory(folderPath);
            filePath = folderPath + "log.json";
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                reloadData = new ReloadData();
            }
            else
            {
                var json = File.ReadAllText(filePath);
                try
                {
                    reloadData = JsonConvert.DeserializeObject<ReloadData>(json);
                }
                catch (Exception ex)
                {
                    File.AppendAllText(folderPath + "error.txt", ex.Message);
                }
            }
        }

        public void FirstReload()
        {
            reloadData.FirstReload = true;
            WriteToJson();
        }

        public void SecondReload()
        {
            reloadData.LastReload = true;
            WriteToJson();
        }

        private void WriteToJson()
        {
            var json = JsonConvert.SerializeObject(reloadData);
            File.WriteAllText(filePath, json);
        }
        
    }
}
