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

        public string Path { get; }
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
            var fileName = SanitizeNameToFilepath(data.Name.Val);
            var pathOnDisk = Path.Combine(_locationProvider.CustomFolderRoot, fileName);
            string json = JsonUtility.ToJson(serializedData, true);
            File.WriteAllText(pathOnDisk, json);

            // Update our own reference
            _selectionReference.Selected.CachedData = serializedData;
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
