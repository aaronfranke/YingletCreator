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

		public CachedYingletReference(string path, SerializableCustomizationData cachedData, CustomizationYingletGroup group)
		{
			Path = path;
			_cachedData = new Observable<SerializableCustomizationData>(cachedData);
			Group = group;
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

		public CustomizationYingletGroup Group { get; }
	}

	/// <summary>
	/// Provides mechanisms for reading / writing character customization data from the disk
	/// </summary>
	public interface ICustomizationDiskIO
	{
		/// <summary>
		/// Returns true if the save went through
		/// </summary>
		bool SaveSelected();
		void DuplicateSelected();
		void DeleteSelected();
		IEnumerable<CachedYingletReference> LoadInitialYingData(CustomizationYingletGroup group);

		/// <summary>
		/// Event fired when a yinglet is saved to disk
		/// Text is the name of the file
		/// </summary>
		event Action<string> OnSaved;

		/// <summary>
		/// Event fired when a yinglet is deleted
		/// Text is the name of the file
		/// </summary>
		event Action<string> OnDeleted;

	}

	public class CustomizationDiskIO : MonoBehaviour, ICustomizationDiskIO
	{
		const string EXTENSION = ".yingsave";
		const string DUPLICATE_SUFFIX = " - Copy";

		private ICustomizationSelection _selectionReference;
		private ICustomizationSelectedDataRepository _selectionData;
		private ICustomizationSaveFolderProvider _locationProvider;
		private ICustomizationYingletRepository _yingletRepository;

		public event Action<string> OnSaved = delegate { };
		public event Action<string> OnDeleted = delegate { }; // Added event implementation

		void Awake()
		{
			_selectionReference = this.GetComponent<ICustomizationSelection>();
			_selectionData = this.GetComponent<ICustomizationSelectedDataRepository>();
			_locationProvider = this.GetComponent<ICustomizationSaveFolderProvider>();
			_yingletRepository = this.GetComponent<ICustomizationYingletRepository>();
		}

		public bool SaveSelected()
		{
			if (_selectionReference.Selected == null) return false;
			var isPreset = _selectionReference.Selected.Group == CustomizationYingletGroup.Preset;
			bool debugButtonsHeld = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt);
			if (isPreset && !debugButtonsHeld) return false;

			// Serialize the data
			var data = _selectionData.CustomizationData;
			var serializedData = new SerializableCustomizationData(data);

			// Write it to disk
			string rootFolder = isPreset ? _locationProvider.PresetFolderRoot : _locationProvider.CustomFolderRoot;
			string newYingletName = data.Name.Val;
			var lastFilePath = _selectionReference.Selected.Path;
			var newFilePath = GetUniqueAlphanumericFilePath(newYingletName, lastFilePath, rootFolder);
			WriteToDisk(newFilePath, serializedData);

			// Clean up the old path (if applicable)
			bool pathIsTheSame = newFilePath == lastFilePath;
			if (!pathIsTheSame)
			{
				File.Delete(lastFilePath);
			}

			// Update our own reference
			_selectionReference.Selected.CachedData = serializedData;
			_selectionReference.Selected.Path = newFilePath;

			OnSaved(Path.GetFileName(newFilePath));
			return true;
		}

		public void DuplicateSelected()
		{
			// Serialize the data
			var data = _selectionData.CustomizationData;
			bool debugButtonsHeld = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl);
			if (!debugButtonsHeld)
			{
				data.Name.Val += DUPLICATE_SUFFIX;
			}
			var serializedData = new SerializableCustomizationData(data);
			serializedData.CreationTime = debugButtonsHeld ? data.CreationTime : DateTime.Now;

			// Write it to disk
			string rootFolder = _locationProvider.CustomFolderRoot;
			string newYingletName = data.Name.Val;
			var newFilePath = GetUniqueAlphanumericFilePath(newYingletName, null, rootFolder);
			WriteToDisk(newFilePath, serializedData);

			// Create a new reference and select it
			var newReference = new CachedYingletReference(newFilePath, serializedData, CustomizationYingletGroup.Custom);
			_yingletRepository.AddNewCustom(newReference);
			_selectionReference.Selected = newReference;

			OnSaved(Path.GetFileName(newFilePath));
		}

		public void DeleteSelected()
		{
			var filePath = _selectionReference.Selected.Path;

			// Delete the file off disk
			File.Delete(filePath);

			// Remove the reference
			int index = _yingletRepository.DeleteCustom(_selectionReference.Selected);

			// Select an adjacent item
			var customYinglets = _yingletRepository.GetYinglets(CustomizationYingletGroup.Custom);
			if (customYinglets.Any())
			{
				int elementId = Mathf.Max(0, (index - 1) % customYinglets.Count());
				_selectionReference.Selected = customYinglets.ElementAt(elementId);
			}
			else
			{
				_selectionReference.Selected = _yingletRepository.GetYinglets(CustomizationYingletGroup.Preset).First();
			}

			// Fire the OnDeleted event
			OnDeleted(Path.GetFileName(filePath));
		}

		public IEnumerable<CachedYingletReference> LoadInitialYingData(CustomizationYingletGroup group)
		{
			var filePaths = GetYingPaths(group);
			var references = filePaths.Select(path => new CachedYingletReference(path, LoadData(path), group)).ToList();
			references.Sort((a, b) => DateTime.Compare(a.CachedData.CreationTime, b.CachedData.CreationTime));
			return references;
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

		string GetUniqueAlphanumericFilePath(string newYingletName, string lastFilePath, string folderPath)
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
				if (fullPath == lastFilePath)
				{
					break; // Same name as last time; we're good here
				}

				fileName = $"{baseName}_{counter}{EXTENSION}";
				fullPath = Path.Combine(folderPath, fileName);
				counter++;
			}

			return fullPath;
		}

		void WriteToDisk(string newFilePath, SerializableCustomizationData serializedData)
		{
			string json = JsonUtility.ToJson(serializedData, true);
			File.WriteAllText(newFilePath, json);
		}
	}
}
