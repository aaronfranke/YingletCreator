using System.IO;
using UnityEngine;

namespace Character.Creator
{
	public interface ICharacterCreatorFolderProvider
	{
		string PresetFolderRoot { get; }
		string CustomFolderRoot { get; }
		string PhotoRoot { get; }
	}

	public class CharacterCreatorFolderProvider : MonoBehaviour, ICharacterCreatorFolderProvider
	{
		private ISaveFolderProvider _saveFolderProvider;

		private void Awake()
		{
			_saveFolderProvider = Singletons.GetSingleton<ISaveFolderProvider>();
			PresetFolderRoot = PathUtils.EnsureDirectoryExists(Path.Combine(Application.streamingAssetsPath, "PresetYings"));
			CustomFolderRoot = PathUtils.EnsureDirectoryExists(Path.Combine(_saveFolderProvider.GameRootFolderPath, "CustomYings"));
			PhotoRoot = PathUtils.EnsureDirectoryExists(Path.Combine(_saveFolderProvider.GameRootFolderPath, "Photos"));
		}

		public string PresetFolderRoot { get; private set; }
		public string CustomFolderRoot { get; private set; }
		public string PhotoRoot { get; private set; }
	}
}