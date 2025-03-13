using System;
using System.IO;
using Newtonsoft.Json;
using ROStandalone.Controllers;
using ROStandalone.Models;

namespace ROStandalone.Helpers
{
    public class JsonHandler
    {
        public static void ReadFlagFile(string fileName, string resourceFolderName)
        {
            string filePath = Path.Combine(Plugin.resourcePath, resourceFolderName, fileName);
            filePath += ".json";
            string json = File.ReadAllText(filePath);

            var data = JsonConvert.DeserializeObject<Flags>(json);

            ConfigController.flags = data;
        }

        public static string SerializeObject(object data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        public static bool CheckFilePath(string fileName, string resourceFolderName)
        {
            string filePath = Path.Combine(Plugin.resourcePath, resourceFolderName, fileName);
            filePath += ".json";

            return File.Exists(filePath);
        }

        public static void SaveToJson(object data, string fileName, string resourceFolderName)
        {
            if (data == null)
            {
                return;
            }

            try
            {
                if (!CheckFilePath(fileName, resourceFolderName))
                {
                    string filePath = Path.Combine(Plugin.resourcePath, resourceFolderName, fileName);
                    filePath += ".json";
                    string jsonString = SerializeObject(data);
                    File.Create(filePath).Dispose();

                    StreamWriter streamWriter = new StreamWriter(filePath);
                    streamWriter.Write(jsonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                else if (CheckFilePath(fileName, resourceFolderName))
                {
                    string filePath = Path.Combine(Plugin.resourcePath, resourceFolderName, fileName);
                    filePath += ".json";
                    string jsonString = SerializeObject(data);
                    File.Delete(filePath);
                    File.Create(filePath).Dispose();

                    StreamWriter streamWriter = new StreamWriter(filePath);
                    streamWriter.Write(jsonString);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError(ex);
            }
        }
    }
}