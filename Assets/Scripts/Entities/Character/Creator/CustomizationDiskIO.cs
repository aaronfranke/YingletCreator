using Reactivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


namespace Character.Creator
{
    public sealed class CachedYingletReference
    {
        Observable<SerializableCustomizationData> _cachedData;
        public CachedYingletReference(string path, SerializableCustomizationData cachedData)
        {
            Path = path;
            _cachedData = new Observable<SerializableCustomizationData>(cachedData);
        }

        public string Path { get; set; }
        public SerializableCustomizationData CachedData
        {
            get
            {
                return _cachedData.Val;
            }
            set
            {
                _cachedData.Val = value;
            }
        }
    }

    /// <summary>
    /// Provides mechanisms for reading / writing character customization data from the disk
    /// </summary>
    public interface ICustomizationDiskIO
    {
        void SaveSelected();
        void DeleteSelected();
        IEnumerable<CachedYingletReference> LoadInitialYingData(CustomizationYingletGroup group);

    }

    public class CustomizationDiskIO : MonoBehaviour, ICustomizationDiskIO
    {
        const string EXTENSION = ".yingsave";

        private ICustomizationSelection _selectionReference;
        private ICustomizationSelectedDataRepository _selectionData;
        private ICustomizationSaveFolderProvider _locationProvider;

        void Awake()
        {
            _selectionReference = this.GetComponent<ICustomizationSelection>();
            _selectionData = this.GetComponent<ICustomizationSelectedDataRepository>();
            _locationProvider = this.GetComponent<ICustomizationSaveFolderProvider>();
        }

        public void SaveSelected()
        {
            // Serialize the data
            var data = _selectionData.CustomizationData;
            var serializedData = new SerializableCustomizationData(data);

            // Write it to disk
            string rootFolder = _locationProvider.CustomFolderRoot;
            string newYingletName = data.Name.Val;
            var lastFilePath = _selectionReference.Selected.Path;
            var newFilePath = GetUniqueAlphanumericFilePath(newYingletName, lastFilePath, rootFolder);
            var pathOnDisk = Path.Combine(_locationProvider.CustomFolderRoot, newFilePath);
            string json = JsonUtility.ToJson(serializedData, true);
            File.WriteAllText(pathOnDisk, json);

            // Clean up the old path (if applicable)
            bool pathIsTheSame = newFilePath == lastFilePath;
            if (!pathIsTheSame)
            {
                File.Delete(lastFilePath);
            }

            // Update our own reference
            _selectionReference.Selected.CachedData = serializedData;
            _selectionReference.Selected.Path = newFilePath;
        }

        public void DeleteSelected()
        {
            // TODO
            throw new System.NotImplementedException();
        }

        public IEnumerable<CachedYingletReference> LoadInitialYingData(CustomizationYingletGroup group)
        {
            var filePaths = GetYingPaths(group);
            return filePaths.Select(path => new CachedYingletReference(path, LoadData(path)));
        }
        IEnumerable<string> GetYingPaths(CustomizationYingletGroup group)
        {
            var folder = GetFolderForGroup(group);
            if (!Directory.Exists(folder))
            {
                Debug.LogWarning($"No folder for group {group} at path {folder}");
                return Enumerable.Empty<string>();
            }
            return Directory.GetFiles(folder, $"*{EXTENSION}", SearchOption.TopDirectoryOnly);

        }
        string GetFolderForGroup(CustomizationYingletGroup group)
        {
            return group switch
            {
                CustomizationYingletGroup.Preset => _locationProvider.PresetFolderRoot,
                CustomizationYingletGroup.Custom => _locationProvider.CustomFolderRoot,
                _ => throw new ArgumentException("No folder for group")
            };
        }
        SerializableCustomizationData LoadData(string filePath)
        {
            string text = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SerializableCustomizationData>(text);
        }

        string GetUniqueAlphanumericFilePath(string newYingletName, string lastFileName, string folderPath)
        {
            // Step 1: Make string alphanumeric
            string baseName = Regex.Replace(newYingletName, "[^a-zA-Z0-9]", "");

            if (string.IsNullOrWhiteSpace(baseName))
            {
                baseName = "unnamed";
            }

            // Step 2: Prepare full file path
            string fileName = baseName + EXTENSION;
            string fullPath = Path.Combine(folderPath, fileName);

            int counter = 1;
            while (File.Exists(fullPath))
            {
                if (fullPath == lastFileName)
                {
                    break; // Same name as last time; we're good here
                }

                fileName = $"{baseName}_{counter}{EXTENSION}";
                fullPath = Path.Combine(folderPath, fileName);
                counter++;
            }

            return fullPath;
        }
    }
}
