using System.IO;
using UnityEngine;

namespace Character.Creator
{
	public interface ICustomizationSaveFolderProvider
	{
		string PresetFolderRoot { get; }
		string CustomFolderRoot { get; }
		string PhotoRoot { get; }
	}

	public class CustomizationSaveFolderProvider : MonoBehaviour, ICustomizationSaveFolderProvider
	{
		private ISaveFolderProvider _saveFolderProvider;

		private void Awake()
		{
			_saveFolderProvider = Singletons.GetSingleton<ISaveFolderProvider>();
		}

		string _presetFolderRoot;
		public string PresetFolderRoot
		{
			get
			{
				if (_presetFolderRoot == null)
				{
					string streamingAssetsPath = Application.streamingAssetsPath;
					string folder = Path.Combine(streamingAssetsPath, "PresetYings");
					_presetFolderRoot = folder;
				}
				return _presetFolderRoot;
			}
		}

		string _customFolderRoot;
		public string CustomFolderRoot
		{
			get
			{
				if (_customFolderRoot == null)
				{
					string folder = Path.Combine(_saveFolderProvider.GameRootFolderPath, "CustomYings");
					if (!Directory.Exists(folder))
					{
						Directory.CreateDirectory(folder);
					}
					_customFolderRoot = folder;
				}
				return _customFolderRoot;
			}
		}

		string _photoRoot;

		public string PhotoRoot
		{
			get
			{
				if (_photoRoot == null)
				{
					string folder = Path.Combine(_saveFolderProvider.GameRootFolderPath, "Photos");
					if (!Directory.Exists(folder))
					{
						Directory.CreateDirectory(folder);
					}
					_photoRoot = folder;
				}
				return _photoRoot;
			}
		}
	}
}