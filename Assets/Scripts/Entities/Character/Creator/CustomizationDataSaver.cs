using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Character.Creator
{
    public interface ICustomizationDataSaver
    {
        void SaveCurrent();
        void DeleteCurrent();

        string FolderRoot { get; }
    }

    public class CustomizationDataSaver : MonoBehaviour, ICustomizationDataSaver
    {
        const string EXTENSION = ".yingsave";

        string _folderRoot;
        private ICharacterCreatorDataRepository _dataRepository;

        public string FolderRoot => _folderRoot;

        void Awake()
        {
            _folderRoot = GetFolderRoot();
            _dataRepository = this.GetComponent<ICharacterCreatorDataRepository>();
        }

        public void SaveCurrent()
        {
            var data = _dataRepository.CustomizationData;
            var fileName = SanitizeNameToFilepath(data.Name);
            var pathOnDisk = Path.Combine(_folderRoot, fileName);
            string json = JsonUtility.ToJson(new CustomizationSavedData(data), true);
            File.WriteAllText(pathOnDisk, json);
        }
        public void DeleteCurrent()
        {
            // TODO
        }

        string SanitizeNameToFilepath(string actualName)
        {
            if (string.IsNullOrWhiteSpace(actualName))
            {
                actualName = "Unnamed";
            }
            var sanitized = Regex.Replace(actualName, "[^a-zA-Z]", "");
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
