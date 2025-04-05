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
        public CachedYingletReference(string path, SerializableCustomizationData cachedData)
        {
            Path = path;
            CachedData = cachedData;
        }

        public string Path { get; }
        public SerializableCustomizationData CachedData { get; }
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

        private ICustomizationSelectedDataRepository _dataRepository;
        private ICustomizationSaveFolderProvider _locationProvider;

        void Awake()
        {
            _dataRepository = this.GetComponent<ICustomizationSelectedDataRepository>();
            _locationProvider = this.GetComponent<ICustomizationSaveFolderProvider>();
        }

        public void SaveSelected()
        {
            var data = _dataRepository.CustomizationData;

            // TODO: Figure out how to handle name changes
            var fileName = SanitizeNameToFilepath(data.Name.Val);


            var pathOnDisk = Path.Combine(_locationProvider.CustomFolderRoot, fileName);
            string json = JsonUtility.ToJson(new SerializableCustomizationData(data), true);
            File.WriteAllText(pathOnDisk, json);
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


        string SanitizeNameToFilepath(string actualName)
        {
            if (string.IsNullOrWhiteSpace(actualName))
            {
                actualName = "Unnamed";
            }
            var sanitized = Regex.Replace(actualName, "[^a-zA-Z]", "");
            return sanitized + EXTENSION;
        }
    }
}
