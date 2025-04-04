using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


namespace Character.Creator
{
    public sealed class CustomizationCachedYingletReference
    {
        public CustomizationCachedYingletReference(string path, ICustomizationData cachedData)
        {
            Path = path;
            CachedData = cachedData;
        }

        public string Path { get; }
        public ICustomizationData CachedData { get; }
    }

    /// <summary>
    /// Provides mechanisms for reading / writing character customization data from the disk
    /// </summary>
    public interface ICustomizationDiskIO
    {
        void SaveSelected();
        void DeleteSelected();
        IEnumerable<CustomizationCachedYingletReference> LoadInitialYingData(CustomizationYingletGroup group);

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
            var fileName = SanitizeNameToFilepath(data.Name);
            var pathOnDisk = Path.Combine(_locationProvider.CustomFolderRoot, fileName);
            string json = JsonUtility.ToJson(new CustomizationSavedData(data), true);
            File.WriteAllText(pathOnDisk, json);
        }
        public void DeleteSelected()
        {
            // TODO: Continue from here
            throw new System.NotImplementedException();
        }

        public IEnumerable<CustomizationCachedYingletReference> LoadInitialYingData(CustomizationYingletGroup group)
        {
            var filePaths = GetYingPaths(group);
            return filePaths.Select(path => new CustomizationCachedYingletReference(path, LoadData(path)));
        }
        IEnumerable<string> GetYingPaths(CustomizationYingletGroup group)
        {
            var folder = GetFolderForGroup(group);
            return Directory.GetFiles(folder, $"*{EXTENSION}", SearchOption.TopDirectoryOnly);

        }
        string GetFolderForGroup(CustomizationYingletGroup group)
        {
            return group switch
            {
                CustomizationYingletGroup.Custom => _locationProvider.CustomFolderRoot,
                _ => throw new ArgumentException("No folder for group")
            };
        }
        ICustomizationData LoadData(string filePath)
        {
            return null; // TODO implement
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
