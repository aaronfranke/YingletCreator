using System;
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

		string GameRoot
		{
			get
			{
				string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				return Path.Combine(documentsPath, "My Games", Application.productName);
			}

		}

		string _customFolderRoot;
		public string CustomFolderRoot
		{
			get
			{
				if (_customFolderRoot == null)
				{
					string folder = Path.Combine(GameRoot, "CustomYings");
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
					string folder = Path.Combine(GameRoot, "Photos");
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