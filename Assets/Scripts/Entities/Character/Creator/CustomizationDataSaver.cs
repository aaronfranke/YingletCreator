using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Character.Creator
{
    public interface ICustomizationDataSaver
    {
        void Save(ICustomizationData data);
        // LoadFromName
        // LoadAllNames
    }

    public class CustomizationDataSaver : MonoBehaviour, ICustomizationDataSaver
    {
        const string EXTENSION = ".yingsave";

        string _folderRoot;

        void Awake()
        {
            _folderRoot = GetFolderRoot();
        }

        public void Save(ICustomizationData data)
        {
            var fileName = SanitizeNameToFilepath(data.Name);
            var pathOnDisk = Path.Combine(_folderRoot, fileName);
            string json = JsonUtility.ToJson(new CustomizationSavedData(data), true);
            File.WriteAllText(pathOnDisk, json);
        }



        string SanitizeNameToFilepath(string actualName)
        {
            var sanitized = Regex.Replace(actualName, "[^a-zA-Z]", "");
            if (sanitized.Length == 0)
            {
                sanitized = "Unnamed";
            }
            return sanitized + EXTENSION;
        }

        static string GetFolderRoot()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = Path.Combine(documentsPath, "My Games", Application.productName);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }
    }
}
