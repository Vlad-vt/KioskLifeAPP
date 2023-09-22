﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace KioskLife
{
    internal class AppManager
    {
        private string filePath;

        private ReloadData reloadData;

        private class ReloadData
        {
            [JsonProperty("Current Day")]
            public DateTime Date;

            [JsonProperty("First time program reload")]
            public bool FirstReload { get; set; }

            [JsonProperty("Second time program reload")]
            public bool LastReload { get; set; }

            public ReloadData()
            {
                Date = DateTime.Now;
                FirstReload = false;
                LastReload = false;
            }

        }

        object lockObject = new object();

        public AppManager()
        {
            var folderPath = AppDomain.CurrentDomain.BaseDirectory + @"\ReloadsLog\";
            Directory.CreateDirectory(folderPath);
            filePath = folderPath + "log.json";
            if (!File.Exists(filePath))
            {
                reloadData = new ReloadData();
                WriteToJson();
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

        public void CheckANewDay()
        {
            if(DateTime.Now.Day != reloadData.Date.Day)
            {
                reloadData.FirstReload = false;
                reloadData.LastReload = false;
                reloadData.Date = DateTime.Now;
            }
        }

        public void FirstReloadActivate()
        {
            reloadData.FirstReload = true;
            WriteToJson();
        }

        public bool FirstReload()
        {
            if (reloadData.FirstReload == false)
            {
                return true;
            }
            return false;
        }

        public bool SecondReload()
        {
            if (reloadData.LastReload == false)
            {
                return true;
            }
            return false;
        }

        public void SecondReloadActivate()
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
